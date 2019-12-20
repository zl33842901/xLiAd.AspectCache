using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace xLiAd.AspectCache.Core
{
    public class AspectClearCache : AbstractInterceptorAttribute
    {

        public string KeyPattern { get; set; }
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            ICacheHelper cacheHelper = context.ServiceProvider.GetService(typeof(ICacheHelper)) as ICacheHelper;
            string key;
            if (string.IsNullOrEmpty(KeyPattern))
            {
                key = context.Implementation.GetType().FullName;
            }
            else
            {
                key = KeyPattern.Replace("{class}", context.Implementation.GetType().FullName)
                    .Replace("{method}", context.ProxyMethod.Name);
            }
            cacheHelper.DeleteByPattern(key);
            var t = context.Invoke(next);
            t.Wait();
            return t;
        }
    }
}
