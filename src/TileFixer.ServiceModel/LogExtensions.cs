using System;
using System.Reflection;
using ServiceStack.Logging;

namespace Tile.ServiceModel
{
  public static class LogExtensions
  {
    public static ILog Log(this Type currentType)
    {
      if (currentType == null)
      {
        currentType = (LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).GetType();
      }
      var log = LogManager.GetLogger(currentType.FullName);
      return log;
    }

    public static ILog Log(this object current)
    {
      if (current == null)
      {
        var declared = MethodBase.GetCurrentMethod().DeclaringType ?? typeof(object);
        current = LogManager.GetLogger(declared.FullName);
      }
      var log = LogManager.GetLogger(current.GetType().FullName);
      return log;
    }
  }
}