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
        public TimeSpan? CacheLifeTime { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            ICacheHelper cacheHelper = context.ServiceProvider.GetService(typeof(ICacheHelper)) as ICacheHelper;
            ICacheOption config = context.ServiceProvider.GetService(typeof(ICacheOption)) as ICacheOption;

            string key;
            if (string.IsNullOrEmpty(CacheKey))
            {
                key = context.Implementation.GetType().FullName + ":" + context.ProxyMethod.Name + ":";
                string ka = Newtonsoft.Json.JsonConvert.SerializeObject(context.Parameters);
                key += ka;
            }
            else
            {
                key = CacheKey.Replace("{class}", context.Implementation.GetType().FullName)
                    .Replace("{method}", context.ProxyMethod.Name);
                for (var i = 0; i < context.Parameters.Length; i++)
                {
                    if (key.Contains($"{{arg{i}}}"))
                        key = key.Replace($"{{arg{i}}}", context.Parameters[i].ToString());
                }
            }

            object o = cacheHelper.Get(key, context.ImplementationMethod.ReturnType);
            if (o == null)
            {
                await next(context);
                cacheHelper.Set(key, context.ReturnValue, CacheLifeTime ?? config.CacheDefaultLifeTime);
            }
            else
            {
                context.ReturnValue = o;
            }
        }
    }
}
