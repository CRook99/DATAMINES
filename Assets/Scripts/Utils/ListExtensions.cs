using System;
using System.Collections.Generic;

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

            int index = _random.Next(list.Count); // Generate a random index
            return list[index];
        }
    }
}