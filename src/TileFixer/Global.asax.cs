using System;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.Text;

namespace TileFixer.Spectrum
{
  public class Global : System.Web.HttpApplication
  {
    private RouteAppHost AppHost;

    protected void Application_Start(object sender, EventArgs e)
    {
      LogManager.LogFactory = new NLogFactory();
      // Initialize ServiceStack Host
      AppHost = new RouteAppHost();
      AppHost.Init();
    }

    protected void Session_Start(object sender, EventArgs e)
    {
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      // Code that runs when an unhandled error occurs
      Exception ex = Server.GetLastError();
      if (ex != null && ex.Message.Length > 0)
      {
        this.Log().ErrorFormat("Message: {0}, Dump: {1}, StackTrace: {2}", ex.Message, ex.Dump(), ex.StackTrace);
      }
    }

    protected void Session_End(object sender, EventArgs e)
    {
    }

    protected void Application_End(object sender, EventArgs e)
    {
    }
  }
}