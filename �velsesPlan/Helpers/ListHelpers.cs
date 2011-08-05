using System;
using System.Collections.Generic;
using System.Linq;
 
namespace ØvelsesPlan.Helpers
{
    public static class ListHelpers
    {
        private static int seed = (int) (DateTime.Now.Ticks % 1009); 

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumeration)
        {
            var asList = enumeration.ToList();
            var length = asList.Count();
            var indexes = Enumerable.Range(0, length).ToArray();
            var generator = new Random(seed++);

            for (int i = 0; i < length; ++i)
            {
                var position = generator.Next(i, length);

                yield return asList[indexes[position]];

                indexes[position] = indexes[i];
            }
        }
    }
}