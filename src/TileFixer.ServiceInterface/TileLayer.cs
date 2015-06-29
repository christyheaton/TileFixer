using System;
using System.Drawing;
using System.Drawing.Imaging;
using ServiceStack;
using Tile.Caching;
using Tile.ServiceModel;

namespace Tile.ServiceInterface
{
  public class TileLayer : Service
  {
    [AddHeader(ContentType = "image/png")]
    public object Get(MetaTile request)
    {
      return TileFactory.MetaTile(request);
    }

    [AddHeader(ContentType = "image/png")]
    public object Get(SpectrumMetaTile request)
    {
      return TileFactory.SpectrumMetaTile(request);
    }

    [AddHeader(ContentType = "image/png")]
    public object Get(GetTile request)
    {
      var log = this.Log();
      var cacheKey = request.CacheKey();
      log.DebugFormat("Cache Id: {0}", cacheKey);
      var result = Cache.ToResultUsingCache(cacheKey, TileFactory.RawTile(request));
      return result.Image;
    }

    public object Get(GetTileBounds request)
    {
      return TileCompute.GetBounds(request.xIndex, request.yIndex, request.zIndex);
    }

    public object Get(GetTileRequests request)
    {
      var point = new GeoPoint
      {
        Latitude = request.Latitude,
        Longitude = request.Longitude
      };
      return TileRequest.LatLongToTiles(point);
    }
  }
}