using System;
using System.Collections.Generic;
using System.Linq;

namespace oiat.saferinternetbot.Business.Extensions
{
    public static class QueryExtensions
    {
        private static readonly Random RandomObject = new Random();

        public static TItem Random<TItem>(this IEnumerable<TItem> items)
        {
            var list = items.ToList();
            return list[RandomObject.Next(0, list.Count - 1)];
        }
    }
}