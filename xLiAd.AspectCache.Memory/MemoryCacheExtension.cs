﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using xLiAd.AspectCache.Core;

namespace xLiAd.AspectCache.Memory
{
    public static class MemoryCacheExtension
    {
        public static void AddMemoryAspectCache(this IServiceCollection services, Action<MemoryCacheOption> optionSetting)
        {
            MemoryCacheOption option = new MemoryCacheOption();
            optionSetting(option);

            services.AddScoped<ICacheOption>(x => option);
            services.AddScoped<IMemoryCacheOption>(x => option);
            services.AddScoped<ICacheHelper>(x =>
            {
                var opt = x.GetService<IMemoryCacheOption>();
                return new MemoryCacheHelper(opt.EnableCache);
            });
            services.AddScoped<IKeyProvider, DefaultKeyProvider>();
        }

        public static void AddMemoryAspectCache(this IServiceCollection services, Action<IServiceProvider, MemoryCacheOption> optionSetting = null)
        {
            services.AddScoped<IMemoryCacheOption>(x => {
                MemoryCacheOption option = new MemoryCacheOption();
                optionSetting?.Invoke(x, option);
                return option;
            });

            services.AddScoped<ICacheOption>(x => x.GetService<IMemoryCacheOption>());
            services.AddScoped<ICacheHelper>(x =>
            {
                var opt = x.GetService<IMemoryCacheOption>();
                return new MemoryCacheHelper(opt.EnableCache);
            });
            services.AddScoped<IKeyProvider, DefaultKeyProvider>();
        }
    }
}
