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
    public object Get(GetMetaTile request)
    {
      using (var image = new Bitmap(TileRequest.TileSize, TileRequest.TileSize, PixelFormat.Format32bppArgb))
      {
        var g = Graphics.FromImage(image);
        g.FillRectangle(Brushes.White, 0f, 0f, image.Width, image.Height);
        g.DrawRectangle(new Pen(Color.Blue), 0f, 0f, image.Width, image.Height);
        var text = "Z: {1}{0}X: {2}{0}Y: {3}{0}".Fmt(
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
      var cacheKey = request.CacheKey();
      log.DebugFormat("Cache Id: {0}", cacheKey);
      var result = Cache.ToResultUsingCache(cacheKey, FixedTile.RawTile(request));
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