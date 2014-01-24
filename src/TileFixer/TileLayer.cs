using System;
using System.Collections.Generic;
using System.IO;
using RestSharp;
using ServiceStack;
using ServiceStack.Configuration;

namespace TileFixer.Spectrum
{
  public class TileLayer : Service
  {
    private const string BaseUrl = @"TileServer";
    private const string ParamUrl = @"rest/Spatial/MapTilingService/NamedTiles/{0}/{1}/{2}:{3}/{4}";
    private const string CacheKeyFormat = @"{0}-{1}-{2}-{3}-{4}";

    private string RestParams(GetTile request)
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
      var result = Cache.ToResultUsingCache(cacheKey, TileSearch(request));
      return result.Image;
    }

    private Func<CachedTile> TileSearch(GetTile request)
    {
      return () =>
      {
        var client = new RestClient(new AppSettings().Get(BaseUrl, "http://localhost:8080"));
        var tileRequest = new RestRequest(RestParams(request));
        var log = this.Log();
        log.DebugFormat("Tile request: {0}", client.BuildUri(tileRequest));
        var tileResponse = client.ExecuteAsGet(tileRequest, HttpMethods.Get);

        var bounds = TileBoundingBox.GetTileBounds(request.xIndex, request.yIndex, request.zIndex);
        log.DebugFormat("Tile bounds: {0}", bounds);
        log.DebugFormat("Content Type: {0}, size in bytes {1}", tileResponse.ContentType, tileResponse.RawBytes.LongLength);

        return new CachedTile { Image = tileResponse.RawBytes, Bounds = bounds };
      };
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