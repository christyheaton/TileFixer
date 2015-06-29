using System.IO;
using ServiceStack;

namespace Tile.ServiceModel
{
  [Route("/spectrumMetaTile/{zIndex}/{xIndex}/{yIndex}", "GET")]
  public class SpectrumMetaTile : IReturn<Stream>
  {
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }
}