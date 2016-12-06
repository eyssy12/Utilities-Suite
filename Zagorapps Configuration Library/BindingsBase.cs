namespace Zagorapps.Configuration.Library
{
    using System;
    using System.Collections.Generic;
    using Core.Library.Extensions;
    using Extensions;
    using SimpleInjector;

    public abstract class BindingsBase
    {
        protected readonly IList<RegistrationMetadata> Registrations;

        protected BindingsBase()
        {
            this.Registrations = new List<RegistrationMetadata>();

            this.RegisterBindings();
        }

        protected void BindFactory<TFactory>()
        {
            this.Register(typeof(TFactory), null, true, Lifestyle.Singleton);
        }

        protected abstract void RegisterBindings();

        public virtual void RegisterBindingsToContainer(Container container)
        {
            this.Registrations.ForEach(binding =>
            {
                if (binding.IsFactoy)
                {
                    container.BindFactory(binding.Service);
                }
                else
                {
                    if (binding.InstanceCreator == null)
                    {
                        container.Register(binding.Service, binding.Implementation, binding.Lifestyle);
                    }
                    else
                    {
                        container.Register(binding.Service, () => binding.InstanceCreator(container), binding.Lifestyle);
                    }
                }
            });

            this.Registrations.Clear();
        }

        protected void Register<TService, TImplementation>(bool isFactory = false, Lifestyle lifestyle = null)
            where TService : class
            where TImplementation : class, TService
        {
            Type service = typeof(TService);
            Type implementation = typeof(TImplementation);

            this.Register(service, implementation, isFactory, lifestyle);
        }

        protected void Register<TService>(Func<Container, object> instanceCreator, Lifestyle lifestyle = null)
        {
            this.Register(typeof(TService), instanceCreator, lifestyle);
        }

        protected void Register(Type service, Func<Container, object> instanceCreator, Lifestyle lifestyle = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service type has not been provided - The container would not be able to register the implementation to the service."); // TODO: resources
            }

            if (instanceCreator == null)
            {
                throw new ArgumentNullException(nameof(instanceCreator), "Implementation type has not been provided - The container would not be able to register the service to the requested implementation");
            }

            RegistrationMetadata metadata = this.CreateMetadata(service, null, instanceCreator, false, lifestyle ?? Lifestyle.Transient);

            this.Registrations.Add(metadata);
        }

        protected void Register(Type service, Type implementation, bool isFactory = false, Lifestyle lifesyle = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service type has not been provided - The container would not be able to register the implementation to the service.");
            }

            RegistrationMetadata metadata;
            if (isFactory)
            {
                metadata = this.CreateMetadata(service, null, null, true, lifesyle ?? Lifestyle.Transient);
            }
            else
            {
                if (implementation == null)
                {
                    throw new ArgumentNullException(nameof(implementation), "Implementation type has not been provided - The container would not be able to register the service to the requested implementation");
                }

                metadata = this.CreateMetadata(service, implementation, null, false, lifesyle ?? Lifestyle.Transient);
            }

            this.Registrations.Add(metadata);
        }
        
        protected RegistrationMetadata CreateMetadata(Type service, Type implementation, Func<Container, object> instanceCreator, bool isFactory, Lifestyle lifesyle)
        {
            return new RegistrationMetadata
            {
                Service = service,
                Implementation = implementation,
                InstanceCreator = instanceCreator,
                IsFactoy = isFactory,
                Lifestyle = lifesyle
            };
        }

        protected sealed class RegistrationMetadata
        {
            public bool IsFactoy { get; set; }

            public Func<Container, object> InstanceCreator { get; set; }

            public Type Service { get; set; }

            public Type Implementation { get; set; }

            public Lifestyle Lifestyle { get; set; }
        }
    }
}