using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using TileFixer.ServiceInterface;
using TileFixer.ServiceModel;

namespace TileFixer.Tests
{
  [TestFixture]
  public class UnitTests
  {
    private readonly ServiceStackHost appHost;

    public UnitTests()
    {
      appHost = new BasicAppHost(typeof (TileLayer).Assembly)
      {
        ConfigureContainer = container =>
        {
          //Add your IoC dependencies here
        }
      }
        .Init();
    }

    [TestFixtureTearDown]
    public void TestFixtureTearDown()
    {
      appHost.Dispose();
    }

    [Test]
    public void TestGetBounds()
    {
      const string testBounds =
        "{\"NorthWest\":{\"Latitude\":32.768798828125,\"Longitude\":-97.3828125,\"Projection\":3857},\"NorthEast\":{\"Latitude\":32.768798828125,\"Longitude\":-97.3388671875,\"Projection\":3857},\"SouthWest\":{\"Latitude\":32.7318420410156,\"Longitude\":-97.3828125,\"Projection\":3857},\"SouthEast\":{\"Latitude\":32.7318420410156,\"Longitude\":-97.3388671875,\"Projection\":3857}}";
      var service = appHost.Container.Resolve<TileLayer>();
      var boundsRequests = new GetTileBounds {zIndex = 13, xIndex = 1880, yIndex = 3306};
      var response = (TileBoundingBox) service.Get(boundsRequests);
      Assert.That(response.ToJson(), Is.EqualTo(testBounds));
    }
  }
}