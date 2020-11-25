using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xLiAd.AspectCache.Memory.Test
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
        [AspectCache.Core.AspectCache(CacheKey = "SomeService:GetString", CacheLifeTimeByMinute = 1)]
        public async Task<string> GetString()
        {
            return "5";
        }
    }

    public interface ISomeService
    {
        int[] GetData();
        Task<string> GetString();
    }
}
