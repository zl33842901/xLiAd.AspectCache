using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Memory
{
    public interface IMemoryCacheOption : Core.ICacheOption
    {
        bool EnableCache { get; }
    }
}
