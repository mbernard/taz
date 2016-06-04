using System;
using System.Collections.Generic;
using System.Linq;

namespace Taz.Core.Extensions
{
    public static class EnumerableExtensions
    {
        #region Methods

        // Ex: collection.TakeLast(5);
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            var sourceArray = source as T[] ?? source.ToArray();
            return sourceArray.Skip(Math.Max(0, sourceArray.Count() - count));
        }

        #endregion
    }
}