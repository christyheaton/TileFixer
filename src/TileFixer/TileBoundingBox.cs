using System;
using ServiceStack.Text;

namespace TileFixer.Spectrum
{
  public class TileBoundingBox
  {
    public GeoPoint NorthWest { get; set; }
    public GeoPoint NorthEast { get; set; }
    public GeoPoint SouthWest { get; set; }
    public GeoPoint SouthEast { get; set; }

    public static TileBoundingBox GetTileBounds(double x, double y, double z)
    {
      var bounds = new TileBoundingBox
      {
        NorthWest = TileToLatLong(x, y, z),
        SouthWest = TileToLatLong(x, y + 1, z),
        NorthEast = TileToLatLong(x + 1, y, z),
        SouthEast = TileToLatLong(x + 1, y + 1, z)
      };
      return bounds;
    }

    /// <summary>
    /// This is the NorthWest Corner of th tile
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private static GeoPoint TileToLatLong(double x, double y, double z)
    {
      var newPoint = new GeoPoint();
      double n = Math.PI - ((2.0 * Math.PI * y) / Math.Pow(2.0, z));

      newPoint.Longitude = (float)((x / Math.Pow(2.0, z) * 360.0) - 180.0);
      newPoint.Latitude = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

      return newPoint;
    }

    public override string ToString()
    {
      return this.ToJson();
    }
  }
}