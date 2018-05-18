using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Rocks.Helpers
{
    /// <summary>
    ///     A global static <see cref="IConfiguration"/> provider that can be used to get active configuration instance.<br />
    ///     Before using confugration provider must be set with method <see cref="Set"/>.
    /// </summary>
    public static class GlobalConfigurationProvider
    {
        [CanBeNull]
        private static Func<IConfiguration> Provider { get; set; }


        /// <summary>
        ///     Returns current <see cref="IConfiguration"/> instance, using previously set with method <see cref="Set"/> provider.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public static IConfiguration Get() => Provider?.Invoke();


        /// <summary>
        ///     Sets current <see cref="IConfiguration"/> provider that will be called in method <see cref="Get"/>.
        /// </summary>
        public static void Set([CanBeNull] Func<IConfiguration> provider) => Provider = provider;
    }
}