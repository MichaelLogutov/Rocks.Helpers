using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
#if NET461 || NET471
            var connection = DbProviderFactories.GetFactory(providerName).CreateConnection();
#endif
#if NETSTANDARD2_0
            var connection = DbFactory.Get(providerName).CreateConnection();
#endif
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
        ///     Reads long string from <paramref name="reader"/> by concatinating all rows
        ///     strings from column with <paramref name="ordinal"/> index.
        /// </summary>
        [CanBeNull]
        public static string GetLongString ([NotNull] this DbDataReader reader, int ordinal)
        {
            if (!reader.HasRows)
                return null;

            var result = new StringBuilder ();

            while (reader.Read ())
                result.Append (reader.GetString (ordinal));

            return result.ToString ();
        }


        /// <summary>
        ///     Reads long string from <paramref name="reader"/> by concatinating all rows
        ///     strings from column with <paramref name="ordinal"/> index.
        /// </summary>
        [CanBeNull]
        public static async Task<string> GetLongStringAsync ([NotNull] this DbDataReader reader,
                                                             int ordinal,
                                                             CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!reader.HasRows)
                return null;

            var result = new StringBuilder ();

            while (await reader.ReadAsync (cancellationToken).ConfigureAwait (false))
                result.Append (reader.GetString (ordinal));

            return result.ToString ();
        }
    }
}