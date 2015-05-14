using System.IO;
using ServiceStack;

namespace TileFixer.ServiceModel
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
}