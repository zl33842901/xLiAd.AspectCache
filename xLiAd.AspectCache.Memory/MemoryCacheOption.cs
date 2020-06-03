using System;

namespace xLiAd.AspectCache.Memory
{
    public class MemoryCacheOption : IMemoryCacheOption
    {
        public bool EnableCache { get; set; } = true;

        public TimeSpan CacheDefaultLifeTime { get; set; } = TimeSpan.FromMinutes(5);
    }
}
