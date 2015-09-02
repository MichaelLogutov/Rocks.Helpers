using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class SqlExtensions
    {
        /// <summary>
        ///     Creates new <see cref="DbConnection"/> using specified
        ///     <paramref name="connectionString"/> and <paramref name="providerName"/>.
        /// </summary>
        public static DbConnection CreateConnection ([NotNull] this string connectionString,
                                                     [NotNull] string providerName = "System.Data.SqlClient")
        {
            var connection = DbProviderFactories.GetFactory (providerName).CreateConnection ();
            if (connection == null)
                throw new InvalidOperationException ("Unable to create DbConnection.");

            connection.ConnectionString = connectionString;

            return connection;
        }


        /// <summary>
        ///     Creates new <see cref="DbConnection"/> using specified <paramref name="connectionStringSettings"/>.
        /// </summary>
        public static DbConnection CreateConnection ([NotNull] this ConnectionStringSettings connectionStringSettings)
        {
            return connectionStringSettings.ConnectionString.CreateConnection (connectionStringSettings.ProviderName);
        }


        /// <summary>
        ///     Creates new <see cref="DbConnection"/> using connection string with the name <paramref name="connectionStringName"/>.
        /// </summary>
        public static DbConnection CreateConnection ([NotNull] this ConnectionStringSettingsCollection connectionStrings,
                                                     [NotNull] string connectionStringName)
        {
            var connection_string_settings = connectionStrings[connectionStringName];
            if (connection_string_settings == null)
                throw new InvalidOperationException ("Unable to find connection string \"" + connectionStringName + "\".");

            return connection_string_settings.CreateConnection ();
        }


        /// <summary>
        ///     Adds new parameter to the command.
        /// </summary>
        public static DbParameter AddParameter ([NotNull] this DbCommand command, [NotNull] string name, object value)
        {
            var parameter = command.CreateParameter ();
            parameter.ParameterName = name;
            parameter.Value = value;

            command.Parameters.Add (parameter);

            return parameter;
        }


        /// <summary>
        ///     Adds new parameter to the command.
        /// </summary>
        [NotNull]
        public static DbParameter AddParameter ([NotNull] this DbCommand command,
                                                [NotNull] string parameterName,
                                                DbType type,
                                                object value)
        {
            var parameter = command.CreateParameter ();

            parameter.ParameterName = parameterName;
            parameter.DbType = type;
            parameter.Value = value;

            command.Parameters.Add (parameter);

            return parameter;
        }
    }
}