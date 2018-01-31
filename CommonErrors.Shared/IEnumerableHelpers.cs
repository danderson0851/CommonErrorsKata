using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonErrors.Shared
{
    public static class IEnumerableHelpers
    {
        /// <summary>
        /// Gets a random element out of the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            if (collection == null || collection.Count() == 0)
            {
                return default(T);
            }

            var rand = new Random(DateTime.Now.Millisecond);
            var index = rand.Next(collection.Count());
            return collection.ToArray()[index];
        }
    }
}
