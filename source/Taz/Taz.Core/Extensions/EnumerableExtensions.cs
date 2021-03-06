﻿using System;
using System.Collections.Generic;
using System.Linq;

using Taz.Core.Models;

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

        public static IEnumerable<Message> OrderByTrending(this IEnumerable<Message> source)
        {
            return source.OrderByDescending(x => x.Reactions?.Select(y => y.Users.Length).Sum() ?? 0);
        }

        public static IEnumerable<Message> WhereMentioned(this IEnumerable<Message> source, SlackCommand command)
        {
            return source.Where(x =>
                x.Text.Contains("@channel") ||
                x.Text.Contains("<!channel>") ||
                x.Text.Contains($"@{command.UserId}"));
        }

        public static IEnumerable<Message> WhereNotBot(this IEnumerable<Message> source)
        {
            return source.Where(x => x.UserId != null);
        } 

        #endregion
    }
}