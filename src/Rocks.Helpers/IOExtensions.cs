using System.IO;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    // ReSharper disable once InconsistentNaming
    [UsedImplicitly]
    public static class IOExtensions
    {
        /// <summary>
        ///     Ensures that directory of the file with specified <paramref name="path" /> is
        ///     exists. If it's not - it will be created.
        /// </summary>
        public static void EnsureFileDirectoryExists (this string path)
        {
            path.RequiredNotNullOrEmpty ("path");

            var directory = Path.GetDirectoryName (path);
            if (!string.IsNullOrEmpty (directory) && !Directory.Exists (directory))
                Directory.CreateDirectory (directory);
        }
    }
}