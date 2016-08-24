using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.Extensions
{
    public static class ListExtensions
    {
        public static void AddUnique<T>(this List<T> list, T value)
        {
            if (!list.Contains(value))
                list.Add(value);
        }

        public static void AddUnique<T>(this IList<T> list, T value)
        {
            if (!list.Contains(value))
                list.Add(value);
        }

        public static void AddRangeUnique<T>(this List<T> list, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                if (!list.Contains(value))
                    list.Add(value);
            }
        }

        public static void AddRangeUnique<T>(this IList<T> list, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                if (!list.Contains(value))
                    list.Add(value);
            }
        }
    }
}
