using System;
using System.Collections;
using System.Collections.Generic;

namespace WindowsFirewallHelper.Helpers
{
    internal static class EnumerableHelper
    {
        public static T[] EnumerableToArray<T>(IEnumerable<T> enumerable)
        {
            return EnumerableToArray<T>((IEnumerable) enumerable);
        }

        public static bool Contains<T>(IEnumerable<T> enumerable, T item)
        {
            foreach (var eItem in enumerable)
            {
                if (eItem.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static T[] EnumerableToArray<T>(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var objects = new List<T>();
            foreach (var o in enumerable)
            {
                if (o is T)
                {
                    objects.Add((T) o);
                }
            }
            return objects.ToArray();
        }
    }
}