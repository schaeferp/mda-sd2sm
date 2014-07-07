using System.Collections.Generic;

namespace Egp.Mda.Transformation.Core
{
    public static class DictionaryExtensions
    {
        public static bool Replace<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            var existed = dictionary.Remove(key);
            dictionary.Add(key, value);
            return existed;
        }
    }
}