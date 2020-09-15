using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace User.API.Utilities
{
    public static partial class Extensions
    {
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}