using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public class CollectionComparisonResult<T>
    {
        /// <summary>
        ///     Items, present only in the source collection.
        /// </summary>
        [CanBeNull]
        public IList<T> OnlyInSource { get; set; }

        /// <summary>
        ///     Source items, present in both collections.
        /// </summary>
        [CanBeNull]
        public IList<T> SourceInBoth { get; set; }

        /// <summary>
        ///     Destination items, present in both collections.
        /// </summary>
        [CanBeNull]
        public IList<T> DestinationInBoth { get; set; }

        /// <summary>
        ///     Items, present only in the destination collection.
        /// </summary>
        [CanBeNull]
        public IList<T> OnlyInDestination { get; set; }
    }
}