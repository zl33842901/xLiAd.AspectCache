using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Redis
{
    public class RedisCacheOption : IRedisCacheOption
    {
        public bool EnableCache { get; set; } = true;

        public string RedisUrl { get; set; } = "127.0.0.1:6379";

        public string CachekeyPrefix { get; set; }

        public TimeSpan CacheDefaultLifeTime { get; set; } = TimeSpan.FromMinutes(5);
    }
}
