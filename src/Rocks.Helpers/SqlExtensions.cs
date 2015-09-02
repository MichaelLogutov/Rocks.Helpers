using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class SqlExtensions
    {
        /// <summary>
        ///     Creates new <see cref="DbConnection"/> using specified
        ///     <paramref name="connectionString"/> and <paramref name="providerName"/>.
        /// </summary>
        public static DbConnection CreateDbConnection ([NotNull] this string connectionString,
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
        public static DbConnection CreateDbConnection ([NotNull] this ConnectionStringSettings connectionStringSettings)
        {
            return connectionStringSettings.ConnectionString.CreateDbConnection (connectionStringSettings.ProviderName);
        }


        /// <summary>
        ///     Creates new <see cref="DbConnection"/> using connection string with the name <paramref name="connectionStringName"/>.
        /// </summary>
        public static DbConnection CreateDbConnection ([NotNull] this ConnectionStringSettingsCollection connectionStrings,
                                                       [NotNull] string connectionStringName)
        {
            var connection_string_settings = connectionStrings[connectionStringName];
            if (connection_string_settings == null)
                throw new InvalidOperationException ("Unable to find connection string \"" + connectionStringName + "\".");

            return connection_string_settings.CreateDbConnection ();
        }


        /// <summary>
        ///     Creates new <see cref="DbCommand"/> with specified <paramref name="commandText"/>.
        /// </summary>
        public static DbCommand CreateCommand ([NotNull] this DbConnection connection, string commandText)
        {
            var command = connection.CreateCommand ();
            command.CommandText = commandText;

            return command;
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


        /// <summary>
        ///     Adds new parameter to the command.
        /// </summary>
        [NotNull]
        public static DbParameter AddParameter ([NotNull] this DbCommand command,
                                                [NotNull] string parameterName,
                                                SqlDbType type,
                                                object value)
        {
            var parameter = new SqlParameter ();

            parameter.ParameterName = parameterName;
            parameter.SqlDbType = type;
            parameter.Value = value;

            command.Parameters.Add (parameter);

            return parameter;
        }


        /// <summary>
        ///     Adds new parameter to the <paramref name="parameters"/> collection.
        /// </summary>
        [NotNull]
        public static DbParameter Add ([NotNull] this DbParameterCollection parameters,
                                       [NotNull] string parameterName,
                                       SqlDbType type,
                                       object value)
        {
            var parameter = new SqlParameter ();

            parameter.ParameterName = parameterName;
            parameter.SqlDbType = type;
            parameter.Value = value;

            parameters.Add (parameter);

            return parameter;
        }
    }
}