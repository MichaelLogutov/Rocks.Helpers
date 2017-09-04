using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Rocks.Helpers.Internal
{
    /// <summary>
    /// Port from .NET Framework
    /// </summary>
    internal static class DbProviderFactories
    {
        private static object m_LockObj = new object();

        private static ConnectionState m_InitState;
        private static DataTable m_ProviderTable;

        public static DbProviderFactory GetFactory(string providerInvariantName)
        {
            ADP.CheckArgumentLength(providerInvariantName, "providerInvariantName");
            var providerTable = GetProviderTable();
            if (providerTable != null)
            {
                var providerRow = providerTable.Rows.Find(providerInvariantName);
                if (providerRow != null)
                {
                    return GetFactory(providerRow);
                }
            }
            throw ADP.ConfigProviderNotFound();
        }

        public static DbProviderFactory GetFactory(DataRow providerRow)
        {
            ADP.CheckArgumentNull((object) providerRow, "providerRow");
            var column = providerRow.Table.Columns["AssemblyQualifiedName"];
            if (column != null)
            {
                var str = providerRow[column] as string;
                if (!ADP.IsEmpty(str))
                {
                    var type = Type.GetType(str);
                    if (null != type)
                    {
                        var field = type.GetField("Instance",
                            BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
                        if (null != field && field.FieldType.IsSubclassOf(typeof(DbProviderFactory)))
                        {
                            var obj = field.GetValue(null);
                            if (obj != null)
                            {
                                return (DbProviderFactory) obj;
                            }
                        }
                        throw ADP.ConfigProviderInvalid();
                    }
                    throw ADP.ConfigProviderNotInstalled();
                }
            }
            throw ADP.ConfigProviderMissing();
        }

        private static DataTable IncludeFrameworkFactoryClasses(DataTable configDataTable)
        {
            var providerDataTable = DbProviderFactoriesConfigurationHandler.CreateProviderDataTable();
            var factoryConfigSectionArray = new[]
            {
                new DbProviderFactoryConfigSection("OracleClient Data Provider", "System.Data.OracleClient",
                    ".Net Framework Data Provider for Oracle",
                    typeof(SqlClientFactory).AssemblyQualifiedName.Replace(
                        "System.Data.SqlClient.SqlClientFactory, System.Data,",
                        "System.Data.OracleClient.OracleClientFactory, System.Data.OracleClient,")),
                new DbProviderFactoryConfigSection(typeof(SqlClientFactory), "SqlClient Data Provider",
                    ".Net Framework Data Provider for SqlServer")
            };
            for (var index = 0; index < factoryConfigSectionArray.Length; ++index)
            {
                if (!factoryConfigSectionArray[index].IsNull())
                {
                    var flag = false;
                    if (index == 2)
                    {
                        var type = Type.GetType(factoryConfigSectionArray[index].AssemblyQualifiedName);
                        if (type != null)
                        {
                            var field = type.GetField("Instance",
                                BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
                            if (null != field && field.FieldType.IsSubclassOf(typeof(DbProviderFactory)) &&
                                field.GetValue(null) != null)
                            {
                                flag = true;
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        var row = providerDataTable.NewRow();
                        row["Name"] = factoryConfigSectionArray[index].Name;
                        row["InvariantName"] = factoryConfigSectionArray[index].InvariantName;
                        row["Description"] = factoryConfigSectionArray[index].Description;
                        row["AssemblyQualifiedName"] = factoryConfigSectionArray[index].AssemblyQualifiedName;
                        providerDataTable.Rows.Add(row);
                    }
                }
            }
            var index1 = 0;
            while (configDataTable != null)
            {
                if (index1 < configDataTable.Rows.Count)
                {
                    try
                    {
                        var flag = false;
                        if (configDataTable.Rows[index1]["AssemblyQualifiedName"].ToString().ToLowerInvariant()
                            .Contains("System.Data.OracleClient".ToLowerInvariant()))
                        {
                            var type = Type.GetType(configDataTable.Rows[index1]["AssemblyQualifiedName"].ToString());
                            if (type != null)
                            {
                                var field = type.GetField("Instance",
                                    BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
                                if (null != field &&
                                    field.FieldType.IsSubclassOf(typeof(DbProviderFactory)) &&
                                    field.GetValue(null) != null)
                                {
                                    flag = true;
                                }
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            providerDataTable.Rows.Add(configDataTable.Rows[index1].ItemArray);
                        }
                    }
                    catch (ConstraintException)
                    {
                    }
                    ++index1;
                }
                else
                    break;
            }
            return providerDataTable;
        }

        private static DataTable GetProviderTable()
        {
            Initialize();
            return m_ProviderTable;
        }

        private static void Initialize()
        {
            if (ConnectionState.Open == m_InitState)
            {
                return;
            }

            lock (m_LockObj)
            {
                switch (m_InitState)
                {
                    case ConnectionState.Closed:
                        m_InitState = ConnectionState.Connecting;
                        try
                        {
                            var section = ConfigurationManager.GetSection("system.data") as DataSet;
                            m_ProviderTable = section != null
                                ? IncludeFrameworkFactoryClasses(section.Tables["DbProviderFactories"])
                                : IncludeFrameworkFactoryClasses(null);
                            break;
                        }
                        finally
                        {
                            m_InitState = ConnectionState.Open;
                        }
                }
            }
        }
    }
}