using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

namespace Yarde.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerator<T> EmptyEnumerator<T>()
        {
            yield break;
        }

        public static void CopyFrom<T>(this HashSet<T> a, HashSet<T> b)
        {
            if (a == null)
            {
                return;
            }

            a.Clear();

            if (b == null)
            {
                return;
            }

            foreach (var bItem in b)
            {
                a.Add(bItem);
            }
        }

        public static void CopyFrom(this IList a, IList b)
        {
            if (a == null)
            {
                return;
            }

            a.Clear();

            if (b == null)
            {
                return;
            }

            for (int i = 0; i < b.Count; ++i)
            {
                a.Add(b[i]);
            }
        }

        public static void CopyFrom([CanBeNull] this IDictionary a, IDictionary b)
        {
            if (a == null)
            {
                return;
            }

            a.Clear();

            if (b == null)
            {
                return;
            }

            foreach (var k in b.Keys)
            {
                a.Add(k, b[k]);
            }
        }

        public static void RemoveAll<TK, TV>(this Dictionary<TK, TV> d, Func<TV, bool> p)
        {
            List<TK> toRemove = new List<TK>();
            foreach (var k in d.Keys)
            {
                var e = d[k];
                if (p(e))
                {
                    toRemove.Add(k);
                }
            }

            for (int i = 0; i < toRemove.Count; ++i)
            {
                d.Remove(toRemove[i]);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> cb)
        {
            foreach (var item in items)
            {
                cb(item);
            }
        }

        public static async UniTask ForEachAsync<T>(this IEnumerable<T> items, Func<T, UniTask> cb)
        {
            foreach (var item in items)
            {
                try
                {
                    await cb(item);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValueProvider();
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static TScore Max<TScore, TItem>(this List<TItem> items, Func<TItem, TScore> scoreFunc, out TItem maxItem) where TScore : IComparable
        {
            if (items.Count == 0)
            {
                maxItem = default(TItem);
                return default(TScore);
            }

            var maxScore = scoreFunc(items[0]);
            maxItem = items[0];

            for (int i = 1; i < items.Count; ++i)
            {
                var score = scoreFunc(items[i]);
                if (score.CompareTo(maxScore) > 0)
                {
                    maxScore = score;
                    maxItem = items[i];
                }
            }

            return maxScore;
        }

        public static int IndexOf(this IEnumerable items, Func<object, bool> pred)
        {
            var index = 0;
            foreach (var i in items)
            {
                if (pred(i))
                {
                    return index;
                }
                ++index;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> pred)
        {
            var index = 0;
            foreach (var i in items)
            {
                if (pred(i))
                {
                    return index;
                }
                ++index;
            }

            return -1;
        }

        public static int IndexOf<T>(this IReadOnlyList<T> items, T item) where T : IEquatable<T>
        {
            int index = 0;
            while (index < items.Count && !items[index].Equals(item))
                ++index;

            return index < items.Count ? index : -1;
        }

        public static IList<T> Shuffle<T>(this IList<T> items)
        {
            return Shuffle(items, 0, new Random(Environment.TickCount));
        }

        public static IList<T> Shuffle<T>(this IList<T> items, int start)
        {
            return Shuffle(items, start, new Random(Environment.TickCount));
        }

        public static IList<T> Shuffle<T>(this IList<T> items, Random rng)
        {
            return Shuffle(items, 0, rng);
        }

        public static IList<T> Shuffle<T>(this IList<T> items, int start, Random rng)
        {
            int n = items.Count;
            while (n > start + 1)
            {
                n--;
                int k = rng.Next(start, n + 1);
                (items[k], items[n]) = (items[n], items[k]);
            }
            return items;
        }

        public static T Random<T>(this IReadOnlyList<T> items) => Random(items, new Random(Environment.TickCount));
        public static T Random<T>(this IReadOnlyList<T> items, Random rng) => items.Count == 0 ? default : items[rng.Next(items.Count)];

        public static List<T> Resize<T>(this List<T> items, int count)
        {
            if (items.Count < count)
            {
                if (items.Capacity < count)
                    items.Capacity = count;

                items.AddRange(Enumerable.Repeat(default(T), count - items.Count));
            }
            else if (items.Count > count)
            {
                items.RemoveRange(count, items.Count - count);
            }

            return items;
        }
    }
}
