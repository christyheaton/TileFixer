using System;
using System.IO;
using ServiceStack;

namespace Tile.ServiceModel
{
  [Route("/getTile/{LayerName}/{zIndex}/{xIndex}/{yIndex}/{StaticResource}", "GET")]
  public class GetTile : IReturn<Stream>
  {
    public string LayerName { get; set; }
    public string StaticResource { get; set; }
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }

  public static class GetTileExtensions
  {
    private const string CacheKeyFormat = @"{0}-{1}-{2}-{3}-{4}";
    public static string CacheKey(this GetTile request)
    {
      return String.Format(CacheKeyFormat,
        request.LayerName,
        request.zIndex,
        request.xIndex,
        request.yIndex,
        request.StaticResource);
    }    
  }
}