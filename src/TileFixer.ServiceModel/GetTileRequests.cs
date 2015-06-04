using System.Collections.Generic;
using ServiceStack;

namespace Tile.ServiceModel
{
  [Route("/getTileRequests/{Latitude}/{Longitude}", "GET")]
  public class GetTileRequests : IReturn<List<GetTileBounds>>
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }
}