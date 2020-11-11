using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace xLiAd.AspectCache.Core
{
    public class AspectCache : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 缓存键（不设置的话，会默认生成一个）
        /// </summary>
        public string CacheKey { get; set; }
        /// <summary>
        /// 缓存生命周期（不设置的话，会使用默认的缓存生命周期）
        /// </summary>
        public int CacheLifeTimeByMinute { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            ICacheHelper cacheHelper = context.ServiceProvider.GetService(typeof(ICacheHelper)) as ICacheHelper;
            ICacheOption config = context.ServiceProvider.GetService(typeof(ICacheOption)) as ICacheOption;
            IKeyProvider keyProvider = context.ServiceProvider.GetService(typeof(IKeyProvider)) as IKeyProvider;

            string key = keyProvider.ProvideKey(CacheKey, context.Parameters, context.Implementation, context.ProxyMethod);

            object o = cacheHelper.Get(key, context.ImplementationMethod.ReturnType);
            if (o == null)
            {
                await next(context);
                cacheHelper.Set(key, context.ReturnValue, CacheLifeTimeByMinute <= 0 ? config.CacheDefaultLifeTime : TimeSpan.FromMinutes(CacheLifeTimeByMinute));
            }
            else
            {
                context.ReturnValue = o;
            }
        }
    }
}
