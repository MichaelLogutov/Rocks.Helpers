using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public class CollectionComparisonResult<T>
    {
        /// <summary>
        ///     Items, present only in the source collection.
        /// </summary>
        [NotNull]
        public IList<T> OnlyInSource { get; set; }

        /// <summary>
        ///     Source items, present in both collections.
        /// </summary>
        [NotNull]
        public IList<T> SourceInBoth { get; set; }

        /// <summary>
        ///     Destination items, present in both collections.
        /// </summary>
        [NotNull]
        public IList<T> DestinationInBoth { get; set; }

        /// <summary>
        ///     Items, present only in the destination collection.
        /// </summary>
        [NotNull]
        public IList<T> OnlyInDestination { get; set; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="CollectionComparisonResult{T}"/> class.
        /// </summary>
        public CollectionComparisonResult ()
        {
            this.OnlyInSource = new List<T> ();
            this.SourceInBoth = new List<T> ();
            this.DestinationInBoth = new List<T> ();
            this.OnlyInDestination = new List<T> ();
        }
    }
}