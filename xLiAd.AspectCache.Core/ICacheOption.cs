using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Core
{
    public interface ICacheOption
    {
        TimeSpan CacheDefaultLifeTime { get; }
    }
}
