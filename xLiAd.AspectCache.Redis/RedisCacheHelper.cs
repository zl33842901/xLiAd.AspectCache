using System;

namespace xLiAd.AspectCache.Redis
{
    /// <summary>
    /// REDIS缓存类（不支持防止缓存穿透）
    /// </summary>
    public class RedisCacheHelper : Core.ICacheHelper
    {
        private readonly bool Enable;
        private readonly string Prefix;
        public RedisCacheHelper(bool enable, string prefix)
        {
            Enable = enable;
            Prefix = prefix;
        }
        public void Delete(string key)
        {
            if (Enable)
                RedisHelper.Del(key);
        }

        public void DeleteByPattern(string key)
        {
            if (Enable)
            {
                var ks = GetKeys(key);
                RedisHelper.Del(ks);
            }
        }
        private static string[] GetKeys(string key)
        {
            var sc = RedisHelper.Keys("*" + key + "*");
            return sc;
        }

        public object Get(string key, Type type)
        {
            if (Enable)
            {
                var str = RedisHelper.Get(Prefix + key);
                if (string.IsNullOrEmpty(str))
                    return null;
                try
                {
                    if (type == typeof(string))
                        return str;
                    if (type.IsValueType)
                        return Convert.ChangeType(str, type);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject(str, type);
                    return result;
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }

        public T Get<T>(string key)
        {
            if (!Enable)
                return default;
            var o = RedisHelper.Get<T>(Prefix + key);
            return o;
        }

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            if (Enable)
                RedisHelper.Set(Prefix + key, value, Convert.ToInt32(timeSpan.TotalSeconds));
        }
    }
}
