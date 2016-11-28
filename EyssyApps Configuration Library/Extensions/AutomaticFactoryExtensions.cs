namespace Zagorapps.Configuration.Library.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using SimpleInjector;

    public static class AutomaticFactoryExtensions
    {
        public static void BindFactory<TFactory>(this Container container)
        {
            container.BindFactory(typeof(TFactory));
        }

        public static void BindFactory(this Container container, Type factory)
        {
            if (!factory.IsInterface)
            {
                throw new ArgumentException(factory.Name + " is no interface");
            }

            container.ResolveUnregisteredType += (s, e) =>
            {
                if (e.UnregisteredServiceType == factory)
                {
                    e.Register(Expression.Constant(
                        value: CreateFactory(factory, container),
                        type: factory));
                }
            };
        }

        private static object CreateFactory(Type factoryType, Container container)
        {
            AutomaticFactoryProxy proxy = new AutomaticFactoryProxy(factoryType, container);

            return proxy.GetTransparentProxy();
        }

        private sealed class AutomaticFactoryProxy : RealProxy
        {
            private readonly Type factoryType;
            private readonly Container container;

            public AutomaticFactoryProxy(Type factoryType, Container container)
                : base(factoryType)
            {
                this.factoryType = factoryType;
                this.container = container;
            }

            public override IMessage Invoke(IMessage msg)
            {
                if (msg is IMethodCallMessage)
                {
                    return this.InvokeFactory(msg as IMethodCallMessage);
                }

                return msg;
            }

            private IMessage InvokeFactory(IMethodCallMessage msg)
            {
                if (msg.MethodName == "GetType")
                {
                    return new ReturnMessage(this.factoryType, null, 0, null, msg);
                }

                if (msg.MethodName == "ToString")
                {
                    return new ReturnMessage(this.factoryType.Name, null, 0, null, msg);
                }

                MethodInfo method = (MethodInfo)msg.MethodBase;

                object instance = this.container.GetInstance(method.ReturnType);

                return new ReturnMessage(instance, null, 0, null, msg);
            }
        }
    }
}