using System;
using System.Collections.Generic;
using System.Data.Common;


namespace Rocks.Helpers
{
    public static class DbFactory
    {
        private static readonly Dictionary<string, Type> m_Descriptions = new Dictionary<string, Type>();
        
        public static Func<Type, DbProviderFactory> CreateInstance
        {
            private get;
            set;
        }

        public static void Set<TFactory>(string providerName)
            where TFactory : DbProviderFactory
        {
            var factoryType = typeof (TFactory);

            lock (m_Descriptions)
            {
                if (m_Descriptions.ContainsKey(providerName))
                {
                    m_Descriptions[providerName] = factoryType;
                }
                else
                {
                    m_Descriptions.Add(providerName,
                                       factoryType);
                }
            }
        }

        public static DbProviderFactory Get(string providerName)
        {
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
            var providerName = dbConnection.GetType()
                                           .FullName;

            lock (m_Descriptions)
            {
                if (m_Descriptions.ContainsKey(providerName))
                {
                    return ConstructInstance(m_Descriptions[providerName]);
                }
            }

            return null;
        }

        private static DbProviderFactory ConstructInstance(Type type)
        {
            return (DbProviderFactory) (CreateInstance(type) ?? Activator.CreateInstance(type));
        }
    }
}