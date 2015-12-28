using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Iterates through all values skipping null one.
        /// </summary>
        [ContractAnnotation ("values:null => stop")]
        public static IEnumerable<T> SkipNull<T> (this IEnumerable<T> values) where T : class
        {
            if (values == null)
                yield break;

            foreach (var value in values)
            {
                if (value != null)
                    yield return value;
            }
        }


        /// <summary>
        ///     Iterates through all values skipping null or empty one.
        /// </summary>
        [ContractAnnotation ("values:null => stop")]
        public static IEnumerable<string> SkipNullOrEmpty (this IEnumerable<string> values)
        {
            if (values == null)
                yield break;

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty (value))
                    yield return value;
            }
        }


        /// <summary>
        ///     Iterates through the all values returning trim of each value.
        ///     Note: does not check for nulls (use <see cref="SkipNull{T}" /> or <see cref="SkipNullOrEmpty" />).
        /// </summary>
        [ContractAnnotation ("values:null => stop")]
        public static IEnumerable<string> TrimAll (this IEnumerable<string> values)
        {
            if (values == null)
                yield break;

            foreach (var value in values)
                yield return value.Trim ();
        }


        /// <summary>
        ///     Converts values list to array of <typeparamref name="T"/>.
        /// </summary>
        [ContractAnnotation ("values:null => null"), MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static T[] ConvertToArray<T> ([CanBeNull] this IEnumerable<T> values)
        {
            if (values == null)
                return null;

            var list = values as List<T>;
            if (list != null)
                return list.ToArray ();

            return values.ToArray ();
        }


        /// <summary>
        ///     Converts values list to <see cref="IList{T}" />.
        /// </summary>
        [ContractAnnotation ("values:null => null"), MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static IList<T> ConvertToList<T> ([CanBeNull] this IEnumerable<T> values)
        {
            if (values == null)
                return null;

            var list = values as IList<T>;
            if (list != null)
                return list;

            return values.ToList ();
        }


        /// <summary>
        ///     Converts values list to <see cref="IReadOnlyList{T}" />.
        /// </summary>
        [ContractAnnotation ("values:null => null"), MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<T> ConvertToReadOnlyList<T> ([CanBeNull] this IEnumerable<T> values)
        {
            if (values == null)
                return null;

            var list = values as List<T>;
            if (list != null)
                return list.AsReadOnly ();

            var ilist = values as IList<T>;
            if (ilist != null)
                return new ReadOnlyCollection<T> (ilist);

            return values.ToList ().AsReadOnly ();
        }


        /// <summary>
        ///     Converts values list to <see cref="IReadOnlyCollection{T}" />.
        /// </summary>
        [ContractAnnotation ("values:null => null"), MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<T> ConvertToReadOnlyCollection<T> (this IEnumerable<T> values)
        {
            if (values == null)
                return null;

            var list = values as List<T>;
            if (list != null)
                return list.AsReadOnly ();

            var ilist = values as IList<T>;
            if (ilist != null)
                return new ReadOnlyCollection<T> (ilist);

            return values.ToList ().AsReadOnly ();
        }


        /// <summary>
        ///     Returns true if specified enumeration is null or contains no elements (will guarantee to get only the first
        ///     element).
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="items">Enumeration items.</param>
        [ContractAnnotation ("items:null => true"), MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T> (this T items) where T : class, IEnumerable
        {
            if (items == null)
                return true;

            var collection = items as ICollection;
            if (collection != null)
                return collection.Count == 0;

            var e = items.GetEnumerator ();
            if (e.MoveNext ())
                return false;

            return true;
        }


        /// <summary>
        ///     Returns true if specified enumeration is null or contains no elements (will guarantee to get only the first
        ///     element).
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="items">Enumeration items.</param>
        [ContractAnnotation ("items:null => true"), MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T> (this ICollection<T> items)
        {
            return items == null || items.Count == 0;
        }


        /// <summary>
        ///     Returns null if <paramref name="items" /> is null or has no elements.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="items">Source items.</param>
        public static T? FirstOrNull<T> (this IEnumerable<T> items) where T : struct
        {
            if (items == null)
                return null;

            var list = items as IList<T>;
            if (list != null)
            {
                if (list.Count > 0)
                    return list[0];
            }
            else
            {
                using (var enumerator = items.GetEnumerator ())
                {
                    if (enumerator.MoveNext ())
                        return enumerator.Current;
                }
            }

            return null;
        }


        /// <summary>
        ///     Removes items by <paramref name="predicate"/> from the list.
        ///     Returns the list of removed items.
        /// </summary>
        public static IList<T> Remove<T> (this ICollection<T> items, Func<T, bool> predicate)
        {
            var items_to_remove = items.Where (predicate).ToList ();

            foreach (var item in items_to_remove)
            {
                if (!items.Remove (item))
                    throw new InvalidOperationException ("Failed to remove item: " + item);
            }

            return items_to_remove;
        }


        /// <summary>
        ///     Sorts the <paramref name="items"/> in the same order as passed <paramref name="ids"/> list.
        /// </summary>
        public static IEnumerable<TItem> SortById<TItem, TId> (this IEnumerable<TItem> items, IEnumerable<TId> ids, Func<TItem, TId> getId)
        {
            if (items == null)
                yield break;

            var items_list = items as IList<TItem>;
            if (items_list == null)
                items_list = items.ToList ();

            if (items_list.Count == 0)
                yield break;

            var id_index = new Dictionary<TId, int> ();
            var index = 0;

            foreach (var id in ids)
                id_index[id] = index++;

            foreach (var item in items_list.OrderBy (x => id_index[getId (x)]))
                yield return item;
        }


        /// <summary>
        ///     Distincts source items by specified key.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <param name="items">Source items.</param>
        /// <param name="keySelector">Distinct key selector.</param>
        /// <param name="comparer">Key comparer.</param>
        public static IEnumerable<T> DistinctBy<T, TKey> (this IEnumerable<T> items,
                                                          Func<T, TKey> keySelector,
                                                          IEqualityComparer<TKey> comparer = null)
        {
            if (items == null)
                throw new ArgumentNullException ("items");

            if (keySelector == null)
                throw new ArgumentNullException ("keySelector");

            var keys = new HashSet<TKey> (comparer);

            foreach (var x in items)
            {
                if (keys.Add (keySelector (x)))
                    yield return x;
            }
        }


        /// <summary>
        /// Search <paramref name="items"/> and returns first index of the item for which <paramref name="selector"/> returned true.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="items">Source items.</param>
        /// <param name="selector">Selector function.</param>
        public static int IndexOf<T> (this IEnumerable<T> items, Func<T, bool> selector)
        {
            var index = 0;
            foreach (var item in items)
            {
                if (selector (item))
                    return index;

                index++;
            }

            return -1;
        }


        /// <summary>
        ///     Splits <paramref name="items" /> to arrays of <paramref name="chunkSize" />.
        /// </summary>
        public static IEnumerable<T[]> SplitToChunks<T> (this IEnumerable<T> items, int chunkSize)
        {
            if (items == null)
                yield break;

            using (var iterator = items.GetEnumerator ())
            {
                while (iterator.MoveNext ())
                {
                    var chunk = new T[chunkSize];
                    int k;

                    chunk[0] = iterator.Current;
                    for (k = 1; k < chunkSize && iterator.MoveNext (); k++)
                        chunk[k] = iterator.Current;

                    Array.Resize (ref chunk, k);

                    yield return chunk;
                }
            }
        }


        /// <summary>
        ///     Adds <paramref name="items" /> to the <paramref name="source" /> collection.
        /// </summary>
        public static void AddRange<T> (this ICollection<T> source, [CanBeNull] IEnumerable<T> items)
        {
            if (items == null)
                return;

            var list = source as List<T>;
            if (list != null)
            {
                list.AddRange (items);
                return;
            }

            foreach (var item in items)
                source.Add (item);
        }


        /// <summary>
        ///     Compares two collections using <paramref name="comparer"/> function.
        /// </summary>
        [NotNull]
        public static CollectionComparisonResult<T> CompareTo<T> ([CanBeNull] this IEnumerable<T> source,
                                                                  [CanBeNull] IEnumerable<T> destination,
                                                                  [NotNull] Func<T, T, bool> comparer)
        {
            comparer.RequiredNotNull ("comparer");

            var result = new CollectionComparisonResult<T> ();

            if (source == null && destination == null)
                return result;

            if (source != null && destination == null)
            {
                result.OnlyInSource.AddRange (source);
                return result;
            }

            if (source == null)
            {
                result.OnlyInDestination.AddRange (destination);
                return result;
            }

            var source_list = source.ConvertToList ();
            Debug.Assert (source_list != null, "source_list != null");

            var destination_list = destination.ConvertToList ();
            Debug.Assert (destination_list != null, "destination_list != null");

            result.OnlyInSource.AddRange (source_list.Where (s => !destination_list.Any (d => comparer (s, d))));
            result.SourceInBoth.AddRange (source_list.Where (s => destination_list.Any (d => comparer (s, d))));
            result.DestinationInBoth.AddRange (destination_list.Where (d => source_list.Any (s => comparer (s, d))));
            result.OnlyInDestination.AddRange (destination_list.Where (d => !source_list.Any (s => comparer (s, d))));

            return result;
        }


        /// <summary>
        ///     Compares two collections using <paramref name="key"/> function to retrieve key for comparison.
        /// </summary>
        [NotNull]
        public static CollectionComparisonResult<TItem> CompareTo<TItem, TKey> ([CanBeNull] this IEnumerable<TItem> source,
                                                                                [CanBeNull] IEnumerable<TItem> destination,
                                                                                [NotNull] Func<TItem, TKey> key)
        {
            key.RequiredNotNull ("key");

            var result = new CollectionComparisonResult<TItem> ();

            if (source == null && destination == null)
                return result;

            if (source != null && destination == null)
            {
                result.OnlyInSource.AddRange (source);
                return result;
            }

            if (source == null)
            {
                result.OnlyInDestination.AddRange (destination);
                return result;
            }

            var source_list = source.ConvertToList ();
            Debug.Assert (source_list != null, "source_list != null");

            var destination_list = destination.ConvertToList ();
            Debug.Assert (destination_list != null, "destination_list != null");

            var source_keys = new HashSet<TKey> (source_list.Select (key));
            var destination_keys = new HashSet<TKey> (destination_list.Select (key));

            result.OnlyInSource.AddRange (source_list.Where (s => !destination_keys.Contains (key (s))));
            result.SourceInBoth.AddRange (source_list.Where (s => destination_keys.Contains (key (s))));
            result.DestinationInBoth.AddRange (destination_list.Where (d => source_keys.Contains (key (d))));
            result.OnlyInDestination.AddRange (destination_list.Where (d => !source_keys.Contains (key (d))));

            return result;
        }


        /// <summary>
        ///     <para>
        ///         Compares <paramref name="newItems"/> and <paramref name="existedItems"/>
        ///         using the <paramref name="compare"/> function.
        ///     </para>
        ///     <para>
        ///         If specified, executes <paramref name="insert"/> action for
        ///         every item only in <paramref name="newItems"/>.
        ///     </para>
        ///     <para>
        ///         If specified, executes <paramref name="update"/> action for
        ///         every item existed boeth in <paramref name="newItems"/> and <paramref name="existedItems"/>,
        ///         passing existed item from <paramref name="newItems"/> as first argument
        ///         and existed item from <paramref name="existedItems"/> as the second argument.
        ///     </para>
        ///     <para>
        ///         If specified, executes <paramref name="delete"/> action for
        ///         every item only in <paramref name="existedItems"/>.
        ///     </para>
        /// </summary>
        public static void MergeInto<T> ([CanBeNull] this IEnumerable<T> newItems,
                                         [CanBeNull] IEnumerable<T> existedItems,
                                         [NotNull] Func<T, T, bool> compare,
                                         [CanBeNull] Action<T> insert,
                                         [CanBeNull] Action<T, T> update,
                                         [CanBeNull] Action<T> delete)
        {
            var comparison = newItems.CompareTo (existedItems, compare);

            if (insert != null)
            {
                foreach (var item in comparison.OnlyInSource)
                    insert (item);
            }

            if (update != null)
            {
                foreach (var item in comparison.SourceInBoth)
                {
                    var dest_item = comparison.DestinationInBoth.First (x => compare (item, x));
                    update (item, dest_item);
                }
            }

            if (delete != null)
            {
                foreach (var item in comparison.OnlyInDestination)
                    delete (item);
            }
        }


        /// <summary>
        ///     <para>
        ///         Compares <paramref name="newItems"/> and <paramref name="existedItems"/>
        ///         using the <paramref name="key"/> function to retrieve key for comparison.
        ///     </para>
        ///     <para>
        ///         If specified, executes <paramref name="insert"/> action for
        ///         every item only in <paramref name="newItems"/>.
        ///     </para>
        ///     <para>
        ///         If specified, executes <paramref name="update"/> action for
        ///         every item existed boeth in <paramref name="newItems"/> and <paramref name="existedItems"/>,
        ///         passing existed item from <paramref name="newItems"/> as first argument
        ///         and existed item from <paramref name="existedItems"/> as the second argument.
        ///     </para>
        ///     <para>
        ///         If specified, executes <paramref name="delete"/> action for
        ///         every item only in <paramref name="existedItems"/>.
        ///     </para>
        /// </summary>
        public static void MergeInto<TItem, TKey> ([CanBeNull] this IEnumerable<TItem> newItems,
                                                   [CanBeNull] IEnumerable<TItem> existedItems,
                                                   [NotNull] Func<TItem, TKey> key,
                                                   [CanBeNull] Action<TItem> insert,
                                                   [CanBeNull] Action<TItem, TItem> update,
                                                   [CanBeNull] Action<TItem> delete)
        {
            var comparison = newItems.CompareTo (existedItems, key);

            if (insert != null)
            {
                foreach (var item in comparison.OnlyInSource)
                    insert (item);
            }

            if (update != null)
            {
                foreach (var item in comparison.SourceInBoth)
                {
                    var item_key = key (item);
                    var dest_item = comparison.DestinationInBoth.First (x => key (x).Equals (item_key));
                    update (item, dest_item);
                }
            }

            if (delete != null)
            {
                foreach (var item in comparison.OnlyInDestination)
                    delete (item);
            }
        }
    }
}