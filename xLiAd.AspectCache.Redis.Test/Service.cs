using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Redis.Test
{
    public class SomeService : ISomeService
    {
        private static List<int> li = new List<int>();
        private static int current = 1;

        [AspectCache.Core.AspectCache]
        public int[] GetData()
        {
            li.Add(current++);
            return li.ToArray();
        }
    }

    public interface ISomeService
    {
        int[] GetData();
    }
}
