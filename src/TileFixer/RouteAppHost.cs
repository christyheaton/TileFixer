using System;
using ServiceStack.CacheAccess;
using ServiceStack.Logging;
using ServiceStack.Redis;
using ServiceStack.WebHost.Endpoints;

namespace TileFixer.Spectrum
{
  public class RouteAppHost : AppHostBase
  {
    private const string RedisServerUrl = @"192.168.3.210:6379";

    //Tell Service Stack the name of your application and where to find your web services
    // if this is not the current assembly the routes will not appear properly.
    public RouteAppHost()
      : base("TileFixer", typeof(Global).Assembly)
    {
    }

    public override void Configure(Funq.Container container)
    {
      container.Register<IRedisClientsManager>(c => new PooledRedisClientManager(RedisServerUrl));
      container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient()).ReusedWithin(Funq.ReuseScope.None);
    }
  }

  public static class ICacheClientExtensions
  {
    public static T ToResultUsingCache<T>(this ICacheClient cache,
      string cacheKey,
      Func<T> fn,
      int hours = 168) where T : class
    {
      var cacheResult = cache.Get<T>(cacheKey);
      if (cacheResult != null)
      {
        cache.Log().DebugFormat("Retrieving from cache");
        return cacheResult;
      }

      var result = fn();
      if (result == null)
      {
        return null;        
      }
      cache.Set(cacheKey, result, TimeSpan.FromDays(hours));
      return result;
    }
  }

  public static class LogExtensions
  {
    public static ILog Log(this object current)
    {
      if (current == null)
      {
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      }
      if (current != null)
      {
        return LogManager.GetLogger(current.GetType());
      }
      return null;
    }
  }
}