using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Memory
{
    public class MemoryCacheHelper : Core.ICacheHelper
    {
        private static readonly MemoryCache mc = new MemoryCache(new MemoryCacheOptions() { });
        public MemoryCacheHelper(bool enable)
        {
            this.Enable = enable;
        }
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        public bool Enable { get; }
        public void Set(string key, object value, TimeSpan timeSpan)
        {
            if (Enable)
                mc.Set(key, value, timeSpan);
        }

        public object Get(string key)
        {
            if (Enable)
                return mc.Get(key);
            else
                return null;
        }
        public T Get<T>(string key)
        {
            if (!Enable)
                return default;
            var o = (T)Get(key);
            return o;
        }
        public object Get(string key, Type type)
        {
            return Get(key);
        }

        public void Delete(string key)
        {
            if (Enable)
                mc.Remove(key);
        }

        public void DeleteByPattern(string key)
        {
            if (Enable)
                foreach (var m in GetKeysMethod(mc))
                {
                    if (m.Contains(key))
                        mc.Remove(m);
                }
        }


        private Func<MemoryCache, IEnumerable<string>> func;
        private IEnumerable<string> GetKeysMethod(MemoryCache cache)
        {
            if (func == null)
                func = GetKeys;
            return func(cache);
        }

        private IEnumerable<string> GetKeys(MemoryCache cache)
        {
            var p = typeof(MemoryCache).GetField("_entries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var v = p.GetValue(cache) as IDictionary;
            foreach (DictionaryEntry i in v)
            {
                yield return i.Key.ToString();
            }
        }
    }
}
