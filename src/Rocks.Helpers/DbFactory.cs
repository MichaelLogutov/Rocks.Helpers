using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.SqlClient;

namespace Rocks.Helpers
{
    /// <summary>
    /// Db factory class.
    /// </summary>
    public static class DbFactory
    {
        private static bool IsInitialized = false;
        private static readonly ConcurrentDictionary<string, DbProviderFactory> Descriptions = new ConcurrentDictionary<string, DbProviderFactory>();
        private static Func<DbProviderFactory, DbProviderFactory> ConstructInstanceInterceptor = null;

        /// <summary>
        /// Set construct instance interceptor.
        /// </summary>
        /// <param name="func">Interceptor function.</param>
        public static void SetConstructInstanceInterceptor(Func<DbProviderFactory, DbProviderFactory> func)
        {
            ConstructInstanceInterceptor = func;
        }
        
        /// <summary>
        /// Set DbProviderFactory instance by provider name. 
        /// </summary>
        /// <param name="providerName">Provider name.</param>
        /// <param name="providerInstance">DbProviderFactory instance.</param>
        /// <typeparam name="T">Type of DbProviderFactory.</typeparam>
        public static void Set<T>(string providerName, T providerInstance) where T : DbProviderFactory
        {
            EnsureIsInitialized();
            
            SetInternal(providerName, providerInstance);
        }

        /// <summary>
        /// Get DbProviderFactory instance by provider name. 
        /// </summary>
        /// <param name="providerName">Provider name.</param>
        /// <returns>DbProviderFactory instance.</returns>
        public static DbProviderFactory Get(string providerName)
        {
            EnsureIsInitialized();

            return GetInternal(providerName);
        }

        /// <summary>
        /// Get DbProviderFactory by DbConnection.
        /// </summary>
        /// <param name="dbConnection">DbConnection instance.</param>
        /// <returns>DbProviderFactory instance.</returns>
        public static DbProviderFactory Get(DbConnection dbConnection)
        {
            return Get(dbConnection.GetType().Namespace);
        }

        private static void SetInternal<T>(string providerName, T providerInstance) where T : DbProviderFactory
        {
            Descriptions.AddOrUpdate(providerName,
                                       providerInstance,
                                       (key, value) => providerInstance );
        }
        
        private static DbProviderFactory GetInternal(string providerName)
        {
            if (Descriptions.TryGetValue(providerName, out var result))
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
            if (!IsInitialized)
            {
                lock (Descriptions)
                {
                    if (!IsInitialized)
                    {
                        IncludeBuiltInFactoryClasses();
                        IsInitialized = true;
                    }
                }
            }
        }

        private static void IncludeBuiltInFactoryClasses()
        {
            SetInternal("System.Data.SqlClient", SqlClientFactory.Instance);
        }
    }
}