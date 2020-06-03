using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace xLiAd.AspectCache.Memory.Test
{
    public class UnitTest2
    {
        private IServiceProvider Before()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMemoryAspectCache(x =>
            {
                x.EnableCache = true;
                x.CacheDefaultLifeTime = TimeSpan.FromSeconds(1);
            });
            services.AddScoped<ISomeService, SomeService>();
            services.ConfigureDynamicProxy();
            ServiceProvider sp = services.BuildDynamicProxyProvider();
            return sp;
        }

        [Fact]
        public void TestService()
        {
            var sp = Before();

            var service = sp.GetService<ISomeService>();
            var rst1 = service.GetData();
            var rst2 = service.GetData();
            Assert.Equal(rst1.Length, rst2.Length);

            Task.Delay(TimeSpan.FromSeconds(1.1)).Wait();

            var rst3 = service.GetData();
            Assert.NotEqual(rst3.Length, rst1.Length);
        }
    }
}
