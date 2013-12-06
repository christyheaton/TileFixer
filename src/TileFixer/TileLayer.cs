using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace SGSI.Spectrum
{
  public class TileLayer : Service
  {
    private const string ParamUrl = @"http://192.168.3.200:8080/rest/Spatial/MapTilingService/NamedTiles/{0}/{1}/{2}:{3}/{4}";
    private const string CacheKeyFormat = @"{0}-{1}-{2}-{3}-{4}";

    private string RequestUrl(GetTile request)
    {
      return String.Format(ParamUrl,
        request.LayerName,
        request.zIndex + 1,
        request.xIndex + 1,
        request.yIndex + 1,
        request.StaticResource);
    }

    private string CacheKey(GetTile request)
    {
      return String.Format(CacheKeyFormat,
        request.LayerName,
        request.zIndex,
        request.xIndex,
        request.yIndex,
        request.StaticResource);
    }

    [AddHeader(ContentType = "image/png")]
    public object Get(GetTile request)
    {
      var log = this.Log();
      var cacheKey = CacheKey(request);
      log.DebugFormat("Cache Id: {0}", cacheKey);

      Func<CachedTile> fn = () =>
      {
        var tileRequest = (HttpWebRequest)WebRequest.Create(RequestUrl(request));
        log.DebugFormat("Tile request: {0}", tileRequest.Address.AbsoluteUri);
        var tileResponse = tileRequest.GetResponse();

        var bounds = TileBoundingBox.GetTileBounds(request.xIndex, request.yIndex, request.zIndex);
        log.DebugFormat("Tile bounds: {0}", bounds);

        var imageStream = tileResponse.GetResponseStream();
        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
          if (imageStream != null)
          {
            imageStream.CopyTo(memoryStream);
          }
          bytes = memoryStream.ToArray();
        }
        return new CachedTile { Image = bytes, Bounds = bounds };
      };
      var result = Cache.ToResultUsingCache(cacheKey, fn);
      return result.Image;
    }

    public object Get(GetTileBounds request)
    {
      return TileBoundingBox.GetTileBounds(request.xIndex, request.yIndex, request.zIndex);
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

  public class CachedTile
  {
    public byte[] Image { get; set; }
    public TileBoundingBox Bounds { get; set; }
  }

  [Route("/getTileRequests/{Latitude}/{Longitude}", "GET")]
  public class GetTileRequests : IReturn<List<GetTileBounds>>
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }

  [Route("/getBounds/{zIndex}/{xIndex}/{yIndex}", "GET")]
  public class GetTileBounds : IReturn<TileBoundingBox>
  {
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }

  [Route("/getTile/{LayerName}/{zIndex}/{xIndex}/{yIndex}/{StaticResource}", "GET")]
  public class GetTile : IReturn<Stream>
  {
    public string LayerName { get; set; }
    public string StaticResource { get; set; }
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }
}