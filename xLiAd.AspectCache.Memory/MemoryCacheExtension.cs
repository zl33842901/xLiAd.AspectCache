using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Memory
{
    public static class MemoryCacheExtension
    {
        public static void AddMemoryAspectCache(this IServiceCollection services, Action<MemoryCacheOption> optionSetting)
        {
            MemoryCacheOption option = new MemoryCacheOption();
            optionSetting(option);

            services.AddScoped<Core.ICacheOption>(x => option);
            services.AddScoped<IMemoryCacheOption>(x => option);
            services.AddScoped<Core.ICacheHelper>(x =>
            {
                var opt = x.GetService<IMemoryCacheOption>();
                return new MemoryCacheHelper(opt.EnableCache);
            });
        }
    }
}
