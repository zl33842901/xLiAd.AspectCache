using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Redis
{
    public interface IRedisCacheOption : Core.ICacheOption
    {
        /// <summary>
        /// 缓存开关
        /// </summary>
        bool EnableCache { get; }

        /// <summary>
        /// Redis 地址
        /// </summary>
        string RedisUrl { get; }

        /// <summary>
        /// 缓存前辍
        /// </summary>
        string CachekeyPrefix { get; }
    }
}
