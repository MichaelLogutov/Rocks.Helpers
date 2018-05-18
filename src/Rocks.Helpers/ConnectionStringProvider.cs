using System;
using System.Configuration;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class ConnectionStringProvider
    {
        /// <summary>
        ///     Custom provider that can be used to get the connection string in the method <see cref="GetConnectionString"/>.<br />
        ///     It value will have a priority before using app configuration.<br />
        ///     Default value null.
        /// </summary>
        [CanBeNull]
        public static Func<string, string> CustomProvider { get; set; }


        /// <summary>
        ///     Gets the connection string with specified <paramref name="connectionStringName"/>.<br />
        ///     Throws an exception if connection string was not found.
        /// </summary>
        [NotNull]
        public static string GetConnectionString([NotNull] string connectionStringName)
        {
            if (connectionStringName == null)
                throw new ArgumentNullException(nameof(connectionStringName));

            var result = CustomProvider?.Invoke(connectionStringName);
            if (!string.IsNullOrWhiteSpace(result))
                return result;

            var css = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (css != null && !string.IsNullOrWhiteSpace(css.ConnectionString))
                return css.ConnectionString;

#if !NET471
            var configuration = ConfigurationProvider.Get();
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