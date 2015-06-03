using System;
using RestSharp;
using ServiceStack;
using ServiceStack.Configuration;
using TileFixer.ServiceModel;

namespace TileFixer.Caching
{
  public static class FixedTile
  {
    private const string BaseUrl = @"TileServer";
    private const string ParamUrl = @"rest/Spatial/MapTilingService/NamedTiles/{0}/{1}/{2}:{3}/{4}";   

    private static string RestParams(GetTile request)
    {
      return String.Format(ParamUrl,
        request.LayerName,
        request.zIndex + 1,
        request.xIndex + 1,
        request.yIndex + 1,
        request.StaticResource);
    }

    private static IRestResponse GetTile(GetTile request)
    {
      var client = new RestClient(new AppSettings().Get(BaseUrl, "http://localhost:8080"));
      var tileRequest = new RestRequest(RestParams(request));
      var log = typeof(FixedTile).Log();
      log.DebugFormat("Tile request: {0}", client.BuildUri(tileRequest));
      IRestResponse tileResponse;
      try
      {
        tileResponse = client.ExecuteAsGet(tileRequest, HttpMethods.Get);
      }
      catch (Exception exc)
      {
        log.ErrorFormat("Issue: {0} at {1}", exc.Message, exc.StackTrace);
        throw;
      }
      return tileResponse;
    }

    public static Func<CachedTile> RawTile(GetTile request)
    {
      return () =>
      {
        var log = typeof(FixedTile).Log();
        var bounds = TileBoundingBox.GetTileBounds(request.xIndex, request.yIndex, request.zIndex);
        log.DebugFormat("Tile bounds: {0}", bounds);

        var tileResponse = GetTile(request);
        log.DebugFormat("Content Type: {0}, size in bytes {1}", tileResponse.ContentType,
          tileResponse.RawBytes.LongLength);

        return new CachedTile { Image = tileResponse.RawBytes, Bounds = bounds };
      };
    }
  }
}