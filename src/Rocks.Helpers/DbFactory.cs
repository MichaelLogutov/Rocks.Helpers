using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;


namespace Rocks.Helpers
{
    public static class DbFactory
    {
        private static bool m_IsInitialized = false;

        private static readonly Dictionary<string, DbProviderFactory> m_Descriptions = new Dictionary<string, DbProviderFactory>();

        public static Func<DbProviderFactory, DbProviderFactory> CreateInstance { private get; set; }

        public static void Set<T>(string providerName, T providerInstance) where T : DbProviderFactory
        {
            lock (m_Descriptions)
            {
                if (m_Descriptions.ContainsKey(providerName))
                {
                    m_Descriptions[providerName] = providerInstance;
                }
                else
                {
                    m_Descriptions.Add(providerName, providerInstance);
                }
            }
        }

        public static DbProviderFactory Get(string providerName)
        {
            EnsureIsInitialized();

            lock (m_Descriptions)
            {
                if (m_Descriptions.ContainsKey(providerName))
                {
                    return ConstructInstance(m_Descriptions[providerName]);
                }
            }

            return null;
        }

        public static DbProviderFactory Get(DbConnection dbConnection)
        {
            var providerName = dbConnection.GetType().Namespace;
            return Get(providerName);
        }

        private static DbProviderFactory ConstructInstance(DbProviderFactory instance)
        {
            return CreateInstance(instance) ?? instance;
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
            Set("System.Data.SqlClient", SqlClientFactory.Instance);
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
                    Set(providerName, instance);
                }
            }
        }
    }
}