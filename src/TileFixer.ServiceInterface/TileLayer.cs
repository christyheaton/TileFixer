using System;
using System.Drawing;
using System.Drawing.Imaging;
using RestSharp;
using ServiceStack;
using ServiceStack.Configuration;
using TileFixer.ServiceModel;

namespace TileFixer.ServiceInterface
{
  public class TileLayer : Service
  {
    private const string BaseUrl = @"TileServer";
    private const string ParamUrl = @"rest/Spatial/MapTilingService/NamedTiles/{0}/{1}/{2}:{3}/{4}";
    private const string CacheKeyFormat = @"{0}-{1}-{2}-{3}-{4}";

    private static string RestParams(GetTile request)
    {
      return String.Format(ParamUrl,
        request.LayerName,
        request.zIndex + 1,
        request.xIndex + 1,
        request.yIndex + 1,
        request.StaticResource);
    }

    private static string CacheKey(GetTile request)
    {
      return String.Format(CacheKeyFormat,
        request.LayerName,
        request.zIndex,
        request.xIndex,
        request.yIndex,
        request.StaticResource);
    }

    [AddHeader(ContentType = "image/png")]
    public object Get(GetMetaTile request)
    {
      using (var image = new Bitmap(TileRequest.TileSize, TileRequest.TileSize, PixelFormat.Format32bppArgb))
      {
        var g = Graphics.FromImage(image);
        g.FillRectangle(Brushes.White, 0f, 0f, image.Width, image.Height);
        g.DrawRectangle(new Pen(Color.Blue), 0f, 0f, image.Width, image.Height);
        var text = String.Format("Z: {1}{0}X: {2}{0}Y: {3}{0}",
          Environment.NewLine,
          request.zIndex,
          request.xIndex,
          request.yIndex);
        g.DrawString(text, new Font("Consolas", 14), Brushes.Blue, 0f, 0f);
        image.MakeTransparent(Color.White);
        var converter = new ImageConverter();
        var data = (byte[]) converter.ConvertTo(image, typeof (byte[]));
        return data;
      }
    }

    [AddHeader(ContentType = "image/png")]
    public object Get(GetTile request)
    {
      var log = this.Log();
      var cacheKey = CacheKey(request);
      log.DebugFormat("Cache Id: {0}", cacheKey);
      var result = Cache.ToResultUsingCache(cacheKey, RawTile(request));
      return result.Image;
    }

    private IRestResponse GetTile(GetTile request)
    {
      var client = new RestClient(new AppSettings().Get(BaseUrl, "http://localhost:8080"));
      var tileRequest = new RestRequest(RestParams(request));
      var log = this.Log();
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

    private Func<CachedTile> RawTile(GetTile request)
    {
      return () =>
      {
        var log = this.Log();
        var bounds = TileBoundingBox.GetTileBounds(request.xIndex, request.yIndex, request.zIndex);
        log.DebugFormat("Tile bounds: {0}", bounds);

        var tileResponse = GetTile(request);
        log.DebugFormat("Content Type: {0}, size in bytes {1}", tileResponse.ContentType,
          tileResponse.RawBytes.LongLength);

        return new CachedTile {Image = tileResponse.RawBytes, Bounds = bounds};
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
}