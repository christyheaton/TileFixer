using ServiceStack.Logging;

namespace TileFixer.ServiceModel
{
  public static class LogExtensions
  {
    public static ILog Log(this object current)
    {
      if (current == null)
      {
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      }
      return (current != null) ? LogManager.GetLogger(current.GetType()) : null;
    }
  }
}