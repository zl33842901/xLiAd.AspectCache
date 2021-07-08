using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.AspectCache.Core;

namespace xLiAd.AspectCache.Redis
{
    public static class RedisCacheExtension
    {
        public static void AddRedisAspectCache(this IServiceCollection services, Action<RedisCacheOption> optionSetting)
        {
            RedisCacheOption option = new RedisCacheOption();
            optionSetting(option);

            services.AddScoped<ICacheOption>(x => option);
            services.AddScoped<IRedisCacheOption>(x => option);
            services.AddScoped<ICacheHelper>(x =>
            {
                var opt = x.GetService<IRedisCacheOption>();
                var csredis = new CSRedis.CSRedisClient(opt.RedisUrl);
                RedisHelper.Initialization(csredis);
                return new RedisCacheHelper(opt.EnableCache, opt.CachekeyPrefix);
            });
            services.AddScoped<IKeyProvider, DefaultKeyProvider>();
        }
        public static void AddRedisAspectCache(this IServiceCollection services, Action<IServiceProvider, RedisCacheOption> optionSetting = null)
        {
            services.AddScoped<IRedisCacheOption>(x =>
            {
                RedisCacheOption option = new RedisCacheOption();
                optionSetting?.Invoke(x, option);
                return option;
            });

            services.AddScoped<ICacheOption>(x => x.GetService<IRedisCacheOption>());
            services.AddScoped<ICacheHelper>(x =>
            {
                var opt = x.GetService<IRedisCacheOption>();
                var csredis = new CSRedis.CSRedisClient(opt.RedisUrl);
                RedisHelper.Initialization(csredis);
                return new RedisCacheHelper(opt.EnableCache, opt.CachekeyPrefix);
            });
            services.AddScoped<IKeyProvider, DefaultKeyProvider>();
        }
    }
}
