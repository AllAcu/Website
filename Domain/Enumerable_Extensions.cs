using System;
using System.Collections.Generic;

namespace Domain
{
    public static class Enumerable_Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }
    }
}
