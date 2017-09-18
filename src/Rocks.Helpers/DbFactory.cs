using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;


namespace Rocks.Helpers
{
    /// <summary>
    /// Db factory class.
    /// </summary>
    public static class DbFactory
    {
        private static bool m_IsInitialized = false;

        private static readonly ConcurrentDictionary<string, DbProviderFactory> m_Descriptions = new ConcurrentDictionary<string, DbProviderFactory>();

        private static Func<DbProviderFactory, DbProviderFactory> m_ConstructInstanceInterceptor = null;

        /// <summary>
        /// Set construct instance interceptor.
        /// </summary>
        /// <param name="func">Interceptor function.</param>
        public static void SetConstructInstanceInterceptor(Func<DbProviderFactory, DbProviderFactory> func)
        {
            m_ConstructInstanceInterceptor = func;
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
            m_Descriptions.AddOrUpdate(providerName,
                                       providerInstance,
                                       (key, value) =>
                                       {
                                           m_Descriptions[key] = value;
                                           return value;
                                       });
        }
        
        private static DbProviderFactory GetInternal(string providerName)
        {
            if (m_Descriptions.TryGetValue(providerName, out var result))
            {
                return ConstructInstance(result);
            }

            return null;
        }
        
        private static DbProviderFactory ConstructInstance(DbProviderFactory instance)
        {
            return m_ConstructInstanceInterceptor != null
                       ? m_ConstructInstanceInterceptor(instance)
                       : instance;
        }

        private static void EnsureIsInitialized()
        {
            if (!m_IsInitialized)
            {
                lock (m_Descriptions)
                {
                    if (!m_IsInitialized)
                    {
                        IncludeBuiltInFactoryClasses();
                        IncludeFactoryClassesFromConfig();

                        m_IsInitialized = true;
                    }
                }
            }
        }

        private static void IncludeBuiltInFactoryClasses()
        {
            SetInternal("System.Data.SqlClient", SqlClientFactory.Instance);
        }

        private static void IncludeFactoryClassesFromConfig()
        {
            var section = ConfigurationManager.GetSection("system.data") as DataSet;

            if (section == null
                || !section.Tables.Contains("DbProviderFactories")
                || !section.Tables["DbProviderFactories"].Columns.Contains("InvariantName")
                || !section.Tables["DbProviderFactories"].Columns.Contains("AssemblyQualifiedName"))
            {
                return;
            }

            for (var i = 0; i < section.Tables["DbProviderFactories"].Rows.Count; i++)
            {
                var providerName = section.Tables["DbProviderFactories"].Rows[i]["InvariantName"] as string;
                var providerType = section.Tables["DbProviderFactories"].Rows[i]["AssemblyQualifiedName"] as string;

                DbProviderFactory instance = null;

                var instanceType = Type.GetType(providerType);
                var instanceField = instanceType.GetField("Instance", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);

                if (instanceField != null
                    && instanceField.FieldType.IsSubclassOf(typeof (DbProviderFactory)))
                {
                    try
                    {
                        instance = (DbProviderFactory) instanceField.GetValue(null);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (instance == null)
                {
                    try
                    {
                        instance = (DbProviderFactory) Activator.CreateInstance(instanceType);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (instance != null)
                {
                    SetInternal(providerName, instance);
                }
            }
        }
    }
}