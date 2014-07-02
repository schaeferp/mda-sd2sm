﻿using System;
using System.Collections.Generic;

namespace Egp.Mda.Transformation.Core.IOAutomaton
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }
    }
}