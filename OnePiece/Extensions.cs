using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OnePiece
{
    public static class Extensions
    {
        // takes an enumerable source and returns a comma separate string.
        // handy for building SQL Statements (for example with IN () statements) from object collections
        public static string CommaSeparate<T, U>(this IEnumerable<T> source, Func<T, U> func)
        {
            return string.Join(",", source.Select(s => func(s).ToString()).ToArray());
        }
    }
}