using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Rocks.Helpers
{
    public static class TypeExtensions
    {
        #region Static methods

        [SuppressMessage ("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool Implements<T> (this Type inspectedType)
        {
            return Implements (inspectedType, typeof (T));
        }


        public static bool Implements (this Type inspectedType, Type desiredType)
        {
            if (!desiredType.IsGenericTypeDefinition)
                return inspectedType.GetInterfaces ().Any (desiredType.IsAssignableFrom);

            return inspectedType.ImplementingOpenGeneric (desiredType);
        }


        public static IEnumerable<SelfImplementingTypeInfo> GetSelfImplementingTypes (this Assembly assembly,
                                                                                      Func<Type, bool> instancePredicate = null,
                                                                                      Func<Type, bool> interfacePredicate = null,
                                                                                      bool onlyPublicInterfaces = true)
        {
            var types = assembly
                .GetTypes ()
                .Where (t => t.IsClass && !t.IsAbstract && (instancePredicate == null || instancePredicate (t)));

            foreach (var type in types)
            {
                var self_interface_name = "I" + type.Name;

                var i = type.GetInterfaces ().FirstOrDefault (x => x.Name == self_interface_name &&
                                                                   (interfacePredicate == null || interfacePredicate (x)) &&
                                                                   (!onlyPublicInterfaces || x.IsPublic));

                if (i == null)
                    continue;

                yield return new SelfImplementingTypeInfo
                             {
                                 InterfaceType = i,
                                 InstanceType = type
                             };
            }
        }

        #endregion

        #region Private methods

        private static bool ImplementingOpenGeneric (this Type examinedType, Type openGenericType)
        {
            if (openGenericType.IsInterface)
                return examinedType.GetInterfaces ().Any (x => x.IsGenericType && x.GetGenericTypeDefinition () == openGenericType);

            if (examinedType.IsGenericType && examinedType.GetGenericTypeDefinition () == openGenericType)
                return true;

            return examinedType.BaseType != null && ImplementingOpenGeneric (examinedType.BaseType, openGenericType);
        }

        #endregion
    }
}