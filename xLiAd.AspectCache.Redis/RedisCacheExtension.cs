using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Redis
{
    public static class RedisCacheExtension
    {
        public static void AddRedisAspectCache(this IServiceCollection services, Action<RedisCacheOption> optionSetting)
        {
            RedisCacheOption option = new RedisCacheOption();
            optionSetting(option);

            services.AddScoped<Core.ICacheOption>(x => option);
            services.AddScoped<IRedisCacheOption>(x => option);
            services.AddScoped<Core.ICacheHelper>(x =>
            {
                var opt = x.GetService<IRedisCacheOption>();
                var csredis = new CSRedis.CSRedisClient(opt.RedisUrl);
                RedisHelper.Initialization(csredis);
                return new RedisCacheHelper(opt.EnableCache, opt.CachekeyPrefix);
            });
        }
    }
}
