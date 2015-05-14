using ServiceStack;

namespace TileFixer.ServiceModel
{
  [Route("/getBounds/{zIndex}/{xIndex}/{yIndex}", "GET")]
  public class GetTileBounds : IReturn<TileBoundingBox>
  {
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }
}