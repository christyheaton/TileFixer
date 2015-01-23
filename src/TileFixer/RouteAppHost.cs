using System;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Redis;

namespace TileFixer.Spectrum
{
  public class RouteAppHost : AppHostBase
  {
    //Tell Service Stack the name of your application and where to find your web services
    // if this is not the current assembly the routes will not appear properly.
    public RouteAppHost()
      : base("TileFixer", typeof(Global).Assembly)
    {
    }

    private const string RedisHost = @"RedisServer";

    public override void Configure(Funq.Container container)
    {
      var RedisServerUrl = (new AppSettings()).Get(RedisHost, "localhost:6379");
      container.Register<IRedisClientsManager>(c => new RedisManagerPool(RedisServerUrl));
      container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient()).ReusedWithin(Funq.ReuseScope.None);
			Plugins.Add(new ServerEventsFeature());
			// This enables cross-domain calls from Javascript client.
			Plugins.Add(new CorsFeature());
    }
  }

  public class RedisConfig : Config
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public int Database { get; set; }
    public int Timeout { get; set; }
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
      return (current != null) ? LogManager.GetLogger(current.GetType()) : null;
    }
  }
}