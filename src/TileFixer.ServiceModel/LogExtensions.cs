using System.Reflection;
using ServiceStack.Logging;

namespace Tile.ServiceModel
{
  public static class LogExtensions
  {
    public static ILog Log(this object current)
    {
      if (current == null)
      {
        LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      }
      return (current != null) ? LogManager.GetLogger(current.GetType()) : null;
    }
  }
}