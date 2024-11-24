using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class ListExtensions
    {
        private static readonly Random _random = new Random();

        public static T Random<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new InvalidOperationException("Cannot get a random element from an empty or null list.");
            }

            int index = _random.Next(list.Count);
            return list[index];
        }
        
        public static bool TryGetRandomElement<T>(this List<T> list, Func<T, bool> predicate, out T result)
        {
            var filtered = list.Where(predicate).ToList();

            if (filtered.Count > 0)
            {
                result = filtered[_random.Next(filtered.Count)];
                return true;
            }
            
            result = default;
            return false;
        }
    }
}