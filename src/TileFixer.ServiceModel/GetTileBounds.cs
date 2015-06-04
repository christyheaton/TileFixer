using ServiceStack;

namespace Tile.ServiceModel
{
  [Route("/getBounds/{zIndex}/{xIndex}/{yIndex}", "GET")]
  public class GetTileBounds : IReturn<BoundingBox>
  {
    public int zIndex { get; set; }
    public int xIndex { get; set; }
    public int yIndex { get; set; }
  }
}