// Silang_Layan_Web_Service.Global
using System;
using System.Web;

public class Global : HttpApplication
{
	protected void Application_Start(object sender, EventArgs e)
	{
		try
		{
			MyApplication.InitApplication();
		}
		catch
		{
			base.Server.ClearError();
			base.Response.Clear();
		}
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
	}

	protected void Session_End(object sender, EventArgs e)
	{
	}

	protected void Application_End(object sender, EventArgs e)
	{
	}
}
