using System.IO;
using ServiceStack;

namespace TileFixer.ServiceModel
{
  [Route("/getMetaTile/{zIndex}/{xIndex}/{yIndex}", "GET")]
  public class GetMetaTile : IReturn<Stream>
  {
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }
}