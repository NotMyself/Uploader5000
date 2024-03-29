﻿using System;
using System.Collections.Generic;

namespace Web.Helpers
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> target, Action<T> action)
        {
            if (target == null)
                return;
            foreach (var v in target)
                action(v);
        }

        public static void Each<T>(this IEnumerable<T> target, Action<T, int> action)
        {
            if (target == null)
                return;

            var indexCount = 0;

            foreach (var v in target)
                action(v, indexCount++);
        }

    }
}