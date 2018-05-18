using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.SqlClient;

namespace Rocks.Helpers
{
    /// <summary>
    ///     Db factory class.
    /// </summary>
    public static class GlobalDbFactoriesProvider
    {
        private static bool IsInitialized;
        private static readonly ConcurrentDictionary<string, DbProviderFactory> Factories = new ConcurrentDictionary<string, DbProviderFactory>();
        private static Func<DbProviderFactory, DbProviderFactory> ConstructInstanceInterceptor;


        /// <summary>
        ///     Set construct instance interceptor.
        /// </summary>
        public static void SetConstructInstanceInterceptor(Func<DbProviderFactory, DbProviderFactory> func)
        {
            ConstructInstanceInterceptor = func;
        }


        /// <summary>
        ///     Set DbProviderFactory instance by provider name. 
        /// </summary>
        public static void Set<T>(string providerName, T providerInstance) where T : DbProviderFactory
        {
            EnsureIsInitialized();

            SetInternal(providerName, providerInstance);
        }


        /// <summary>
        ///     Get DbProviderFactory instance by <paramref name="providerName"/>.
        /// </summary>
        public static DbProviderFactory Get(string providerName)
        {
            EnsureIsInitialized();
            return GetInternal(providerName);
        }


        /// <summary>
        ///     Get DbProviderFactory by DbConnection namespace.
        /// </summary>
        public static DbProviderFactory Get(DbConnection dbConnection)
        {
            return Get(dbConnection.GetType().Namespace);
        }


        private static void SetInternal<T>(string providerName, T providerInstance) where T : DbProviderFactory
        {
            Factories.AddOrUpdate(providerName,
                                  providerInstance,
                                  (key, value) => providerInstance);
        }


        private static DbProviderFactory GetInternal(string providerName)
        {
            if (Factories.TryGetValue(providerName, out var result))
                return ConstructInstance(result);

            return null;
        }


        private static DbProviderFactory ConstructInstance(DbProviderFactory instance)
        {
            return ConstructInstanceInterceptor != null
                       ? ConstructInstanceInterceptor(instance)
                       : instance;
        }


        private static void EnsureIsInitialized()
        {
            if (IsInitialized)
                return;

            lock (Factories)
            {
                if (IsInitialized)
                    return;

                IncludeBuiltInFactoryClasses();
                IsInitialized = true;
            }
        }


        private static void IncludeBuiltInFactoryClasses()
        {
            SetInternal("System.Data.SqlClient", SqlClientFactory.Instance);
        }
    }
}