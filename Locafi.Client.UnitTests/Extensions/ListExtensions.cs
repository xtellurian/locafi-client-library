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
    }
}
