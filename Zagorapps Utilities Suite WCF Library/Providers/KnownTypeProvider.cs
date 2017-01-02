namespace Zagorapps.Utilities.Suite.WCF.Library.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Core.Library.Communications;
    using Utilities.Suite.Library;

    public static class KnownTypeProvider<TCentralisedType>
    {
        public static IEnumerable<Type> GetTypes(ICustomAttributeProvider provider)
        {
            Type baseType = typeof(IDataMessage);
            Type serializableType = typeof(ISerializable);

            IEnumerable<Type> types = typeof(TCentralisedType)
                .Assembly
                .GetTypes()
                .Union(baseType
                    .Assembly
                    .GetTypes());

            IEnumerable<Type> knownTypes = types.Where(ta =>
                ta.IsClass &&
                ta.IsPublic &&
                !ta.IsAbstract &&
                serializableType.IsAssignableFrom(ta) &&
                baseType.IsAssignableFrom(ta));

            return knownTypes.Union(new[]
            {
                typeof(SuiteRoute),
                typeof(SuiteRoute[])
            }).Distinct().ToArray();
        }
    }
}