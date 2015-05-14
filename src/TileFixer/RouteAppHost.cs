using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Redis;
using TileFixer.ServiceInterface;

namespace TileFixer
{
  public class RouteAppHost : AppHostBase
  {
    //Tell Service Stack the name of your application and where to find your web services
    // if this is not the current assembly the routes will not appear properly.
    public RouteAppHost()
      : base("TileFixer", typeof(TileLayer).Assembly)
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
}