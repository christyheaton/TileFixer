using System.IO;
using ServiceStack;

namespace Tile.ServiceModel
{
  [Route("/metaTile/{zIndex}/{xIndex}/{yIndex}", "GET")]
  public class MetaTile : IReturn<Stream>
  {
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }
}