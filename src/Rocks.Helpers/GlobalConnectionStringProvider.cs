using System;
using System.Configuration;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class GlobalConnectionStringProvider
    {
        /// <summary>
        ///     Custom provider that can be used to get the connection string in the method <see cref="Get"/>.<br />
        ///     It value will have a priority before using app configuration.<br />
        ///     Default value null.
        /// </summary>
        [CanBeNull]
        public static Func<string, string> CustomProvider { get; set; }
        
        /// <summary>
        ///     Optional custom function that if set will be called on each successfull result of <see cref="Get"/> method
        ///     and it's value will be used as the result. If this function will return null - an exception will be thrown.<br />
        ///     Default is null.
        /// </summary>
        public static Func<string, string> ConnectionStringTransform { get; set; }


        /// <summary>
        ///     Gets the connection string with specified <paramref name="connectionStringName"/>.<br />
        ///     Throws an exception if connection string was not found.
        /// </summary>
        [NotNull]
        public static string Get([NotNull] string connectionStringName)
        {
            var result = GetConnectionStringByName(connectionStringName);

            var connection_string_transform = ConnectionStringTransform;
            if (connection_string_transform != null)
            {
                result = connection_string_transform(result);
                if (result == null)
                    throw new InvalidOperationException("ConnectionStringTransform returned null for a connection string named " + connectionStringName);
            }

            return result;
        }
        
        
        private static string GetConnectionStringByName([NotNull] string connectionStringName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringName))
                throw new ArgumentNullException(nameof(connectionStringName));

            var result = CustomProvider?.Invoke(connectionStringName);
            if (!string.IsNullOrWhiteSpace(result))
                return result;

            var css = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (css != null && !string.IsNullOrWhiteSpace(css.ConnectionString))
                return css.ConnectionString;

#if NETSTANDARD
            var configuration = GlobalConfigurationProvider.Get();
            if (configuration != null)
            {
                var cs = configuration.GetSection("ConnectionStrings")?[connectionStringName];
                if (!string.IsNullOrWhiteSpace(cs))
                    return cs;

                cs = configuration[$"connectionStrings:add:{connectionStringName}:connectionString"];
                if (!string.IsNullOrWhiteSpace(cs))
                    return cs;
            }
#endif

            throw new InvalidOperationException("Unable to find connection string " + connectionStringName + " in app configuration.");
        }
    }
}