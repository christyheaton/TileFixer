using System;

namespace Tile.ServiceModel
{
  public static class TileCompute
  {
    public static BoundingBox MercatorEntireWorld()
    {
      const int projection = 3857;
      return new BoundingBox
      {
        NorthEast = new GeoPoint { Longitude = 180, Latitude = 90, Projection = projection },
        NorthWest = new GeoPoint { Longitude = -180, Latitude = 90, Projection = projection },
        SouthEast = new GeoPoint { Longitude = 180, Latitude = -90, Projection = projection },
        SouthWest = new GeoPoint { Longitude = -180, Latitude = 90, Projection = projection }
      };
    }

    public static BoundingBox GetBounds(double x, double y, double z)
    {
      var bounds = new BoundingBox
      {
        NorthWest = ToLatLong(x, y, z),
        SouthWest = ToLatLong(x, y + 1, z),
        NorthEast = ToLatLong(x + 1, y, z),
        SouthEast = ToLatLong(x + 1, y + 1, z)
      };
      return bounds;
    }

    /// <summary>
    ///   This is the NorthWest Corner of the tile
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private static GeoPoint ToLatLong(double x, double y, double z)
    {
      var newPoint = new GeoPoint();
      var n = Math.PI - ((2.0*Math.PI*y)/Math.Pow(2.0, z));

      newPoint.Longitude = (float) ((x/Math.Pow(2.0, z)*360.0) - 180.0);
      newPoint.Latitude = (float) (180.0/Math.PI*Math.Atan(Math.Sinh(n)));

      return newPoint;
    }
  }
}