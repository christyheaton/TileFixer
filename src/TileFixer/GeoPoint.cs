using ServiceStack;

namespace TileFixer.Spectrum
{
  public class GeoPoint
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Projection { get; set; }

    public GeoPoint()
    {
      Projection = 3857;
    }

    public override string ToString()
    {
      return this.ToJson();
    }
  }
}