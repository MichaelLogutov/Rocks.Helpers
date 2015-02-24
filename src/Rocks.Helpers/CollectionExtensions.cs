using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static T[] AsArray<T> ([CanBeNull] this IEnumerable<T> values)
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
        public static IList<T> AsList<T> ([CanBeNull] this IEnumerable<T> values)
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
        public static IReadOnlyList<T> AsReadOnlyList<T> ([CanBeNull] this IEnumerable<T> values)
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
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T> (this IEnumerable<T> values)
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


        public static int Remove<T> (this ICollection<T> items, Func<T, bool> predicate)
        {
            var items_to_remove = items.Where (predicate).ToList ();

            foreach (var item in items_to_remove)
            {
                if (!items.Remove (item))
                    throw new InvalidOperationException ("Failed to remove item: " + item);
            }

            return items_to_remove.Count;
        }


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
    }
}