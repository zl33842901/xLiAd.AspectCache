# xLiAd.AspectCache

基于 AspectCore 实现的 Aop Cache 切片缓存框架

### 安装程序包：

1，内存缓存：

dotnet add package xLiAd.AspectCache.Memory

2，Redis 缓存：

dotnet add package xLiAd.AspectCache.Redis

### 使用方法：

1，需要先集成 AspectCore AOP框架。https://github.com/dotnetcore/AspectCore-Framework

2，在 StartUp 类的 ConfigureServices 方法内增加如下代码：

```csharp
services.AddMemoryAspectCache(x =>
{
    x.EnableCache = true;
    x.CacheDefaultLifeTime = TimeSpan.FromMinutes(60);
});
```
或者
```csharp
services.AddRedisAspectCache(x =>
{
    x.RedisUrl = "127.0.0.1:6379";
    x.EnableCache = true;
    x.CachekeyPrefix = "Test:";
    x.CacheDefaultLifeTime = TimeSpan.FromMinutes(1);
});
```

3，在你的服务代码中，为方法加上 AspectCache 特性或 AspectClearCache 特性。

加了 AspectCache 特性的方法会使用缓存，加了 AspectClearCache 特性的方法在调用时会清空缓存。

如：

```csharp
public class SomeService
{
    [AspectCache]
    public int[] GetData()
    {
        return // Query From Db
    }

    [AspectClearCache]
    public bool Update()
    {
        return // Write To Db
    }
}
```
注意：SomeService 需要使用依赖注入，并集成了 AspectCore 的动态代理。