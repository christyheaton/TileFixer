using ServiceStack;

namespace TileFixer.ServiceModel
{
  public class GeoPoint
  {
    public GeoPoint()
    {
      Projection = 3857;
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Projection { get; set; }

    public override string ToString()
    {
      return this.ToJson();
    }
  }
}