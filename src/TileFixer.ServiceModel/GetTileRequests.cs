using System.Collections.Generic;
using ServiceStack;

namespace TileFixer.ServiceModel
{
  [Route("/getTileRequests/{Latitude}/{Longitude}", "GET")]
  public class GetTileRequests : IReturn<List<GetTileBounds>>
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }
}