using System;
using System.Linq;
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
            if (context.ImplementationMethod.ReturnType == typeof(void) || context.ImplementationMethod.ReturnType == typeof(Task))
            {
                await next(context);
                return;
            }
            ICacheHelper cacheHelper = context.ServiceProvider.GetService(typeof(ICacheHelper)) as ICacheHelper;
            ICacheOption config = context.ServiceProvider.GetService(typeof(ICacheOption)) as ICacheOption;
            IKeyProvider keyProvider = context.ServiceProvider.GetService(typeof(IKeyProvider)) as IKeyProvider;

            string key = keyProvider.ProvideKey(CacheKey, context.Parameters, context.Implementation, context.ProxyMethod);
            bool isTask = context.ImplementationMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
            if (isTask)
            {
                var realType = context.ImplementationMethod.ReturnType.GetGenericArguments().First();
                object o = cacheHelper.Get(key, realType);
                if (o == null)
                {
                    await next(context);
                    var realResult = context.ImplementationMethod.ReturnType.GetProperty("Result", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(context.ReturnValue);
                    cacheHelper.Set(key, realResult, CacheLifeTimeByMinute <= 0 ? config.CacheDefaultLifeTime : TimeSpan.FromMinutes(CacheLifeTimeByMinute));
                }
                else
                {
                    var method = typeof(Task).GetMethod("FromResult", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    method = method.MakeGenericMethod(realType);
                    context.ReturnValue = method.Invoke(null, new object[] { o });
                }
            }
            else
            {
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
}
