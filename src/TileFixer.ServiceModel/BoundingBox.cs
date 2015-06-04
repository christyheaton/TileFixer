using ServiceStack;

namespace Tile.ServiceModel
{
  public class BoundingBox
  {
    public GeoPoint NorthWest { get; set; }
    public GeoPoint NorthEast { get; set; }
    public GeoPoint SouthWest { get; set; }
    public GeoPoint SouthEast { get; set; }

    public override string ToString()
    {
      return this.ToJson();
    }
  }
}