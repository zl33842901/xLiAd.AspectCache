using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace xLiAd.AspectCache.Redis.Test
{
    public class UnitTest1
    {
        private IServiceProvider Before()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddRedisAspectCache(x =>
            {
                x.RedisUrl = "172.16.101.114:6379";
                x.EnableCache = true;
                x.CachekeyPrefix = "Test:";
                x.CacheDefaultLifeTime = TimeSpan.FromMinutes(60);
            });
            var sp = services.BuildServiceProvider();
            return sp;
        }

        [Fact]
        public void TestExtension()
        {
            var sp = Before();

            var cacheOption = sp.GetService<Core.ICacheOption>();
            Assert.NotNull(cacheOption);
            var redisCacheOption = sp.GetService<IRedisCacheOption>();
            Assert.NotNull(redisCacheOption);
            var cacheHelper = sp.GetService<Core.ICacheHelper>();
            Assert.NotNull(cacheHelper);

            Assert.Equal("172.16.101.114:6379", redisCacheOption.RedisUrl);
            Assert.True(redisCacheOption.EnableCache);
            Assert.Equal("Test:", redisCacheOption.CachekeyPrefix);
        }

        [Fact]
        public void TestAddAndReadString()
        {
            var sp = Before();
            var cacheHelper = sp.GetService<Core.ICacheHelper>();
            cacheHelper.Set("testkey", "testvalue", TimeSpan.FromSeconds(60));

            var rst = cacheHelper.Get<string>("testkey");
            Assert.Equal("testvalue", rst);
            var rst2 = cacheHelper.Get("testkey", typeof(string));
            Assert.Equal("testvalue", rst2);
        }

        [Fact]
        public void TestAddAndReadValueType()
        {
            var sp = Before();
            var key = "testkeyint";
            var cacheHelper = sp.GetService<Core.ICacheHelper>();
            cacheHelper.Set(key, int.MaxValue - 1, TimeSpan.FromSeconds(60));

            var rst = cacheHelper.Get<int>(key);
            Assert.Equal(int.MaxValue - 1, rst);
            var rst2 = cacheHelper.Get(key, typeof(int));
            Assert.Equal(int.MaxValue - 1, rst2);
        }

        [Fact]
        public void TestAddAndReadClass()
        {
            var sp = Before();
            var key = "testkeyclass";
            var cacheHelper = sp.GetService<Core.ICacheHelper>();
            cacheHelper.Set(key, new Model(), TimeSpan.FromSeconds(60));

            var rst = cacheHelper.Get<Model>(key);
            Assert.Equal("Jim Green", rst.Name);
            var rst2 = cacheHelper.Get(key, typeof(Model)) as Model;
            Assert.Equal(rst.Id, rst2.Id);
            Assert.Equal(rst.DateTime, rst2.DateTime);
        }
    }
}
