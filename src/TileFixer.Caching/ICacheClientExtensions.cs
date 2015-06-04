using System;
using ServiceStack.Caching;
using Tile.ServiceModel;

namespace Tile.Caching
{
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
}