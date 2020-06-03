using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Memory.Test
{
    public class Model
    {
        public int Id { get; set; } = 128;
        public string Name { get; set; } = "Jim Green";
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
    }
}
