using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection.Metadata;
using Xunit;

namespace xLiAd.AspectCache.Core.Test
{
    public class UnitTest1
    {
        private IServiceProvider Before()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IKeyProvider, DefaultKeyProvider>();
            var sp = services.BuildServiceProvider();
            return sp;
        }
        private IKeyProvider keyProvider => Before().GetService<IKeyProvider>();
        [Fact]
        public void Test1()
        {
            var kp = keyProvider;
            var key = kp.ProvideKey("{class}_{method}", new object[0], this, this.GetType().GetMethod(nameof(Test1)));
            Assert.Equal($"{this.GetType().FullName}_{nameof(Test1)}", key);
        }
        [Fact]
        public void Test2()
        {
            var kp = keyProvider;
            var key = kp.ProvideKey("{arg0}_{arg1}", new object[] { 2, 3 }, this, this.GetType().GetMethod(nameof(Test1)));
            Assert.Equal("2_3", key);
        }
        [Fact]
        public void Test3()
        {
            var kp = keyProvider;
            var p1 = new A() { FirstName = "jhon" };
            var p2 = new Class() { FirstStudent = p1, StudentsCount = 5 };
            var key = kp.ProvideKey("{arg0.FirstName}${arg0.Age}_{arg1.StudentsCount}-{arg1.FirstStudent.FirstName}", new object[] { p1, p2 }, this, this.GetType().GetMethod(nameof(Test1)));
            Assert.Equal("jhon$0_5-jhon", key);
        }
    }


    public class A
    {
        public string FirstName { get; set; }
        private int Age;
    }

    public  class Class
    {
        internal int StudentsCount { get; set; }
        public A FirstStudent { get; set; }
    }
}
