using System;
using System.Collections.Generic;
using System.Linq;

namespace sem2
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<List<T>> GroupByN<T>(this IEnumerable<T> source, int quantityInGroup)
            => source.Select((x, index) => (x, index / 3)).GroupBy(key => key.Item2, element => element.Item1)
                .Select(x => x.ToList());
    }
}