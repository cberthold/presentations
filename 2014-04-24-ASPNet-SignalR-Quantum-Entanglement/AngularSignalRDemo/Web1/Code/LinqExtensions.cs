using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1.Code
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            // error checking etc removed for brevity

            Random rng = new Random();
            T[] sourceArray = source.ToArray();

            for (int n = 0; n < sourceArray.Length; n++)
            {
                int k = rng.Next(n, sourceArray.Length);
                yield return sourceArray[k];

                sourceArray[k] = sourceArray[n];
            }
        }

    }
}