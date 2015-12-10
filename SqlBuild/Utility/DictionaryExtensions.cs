using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

namespace SqlBuild.Utility
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TValue>(this IDictionary<string, TValue> dictionary, string key)
            where TValue : KeyedModel, new()
        {
            TValue value = null;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = new TValue()
                            {
                                Key = key
                            };
                dictionary.Add(key, value);
            }

            return value;
        }
    }
}
