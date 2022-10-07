// MyApplication
using System.Configuration;
using System.Web;
using System.Web.UI;

public class MyApplication
{
	public static string ApplicationBriefName = "Silang Layan Web Service";

	public static string ApplicationLongName = "Silang Layan Web Service";

	public static string ApplicationWebServiceVersion = "1.0";

	public static string ClientName = "Perpusnas RI";

	public static string VendorName = "Perpusnas RI";

	public static string PublishYear = "2021";

	public static string LogPath = "C:\\SILANG_LAYAN_WEB_SERVICE_LOG\\";

	public static string LogExt = "log";

	public static string MainPage = "Default.aspx";

	public static int MaxItemPerPage = 10;

	public static int MaxPageNumberDisplayed = 10;

	public static bool IsDebug = false;

	public static string CurrentErrorMessage = "";

	public static string SavedKeyApplicationSetting = "Gpd20EP7dAFY2MPB7btkneE/nJfAk4gzmLvne7PJwmM=";

	public static string SavedIVApplicationSetting = "N5wGPcqyOFU2Vxpk2TIcyw==";

	public static string LogoPath = "";

	public static string BreakString = "<br/>";

	public static string SuperAdminID = "0";

	public static bool IsCaseSencitive = true;

	public static string SNSavedKeyApplicationSetting = "TpSQw+pRO1lEUbH2wqm1oLkuP7qC2m2YXJw/1bDizjw=";

	public static string SNSavedIVApplicationSetting = "dZaiXvOyn804h6rSgCtdpA==";

	public static void InitApplication()
	{
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
        if (page != null)
		///if (HttpContext.Current.Handler is Page handler)
		{
			page.Session.Clear();
		}
		Connection.SetConnection();
		GetIsCaseSencitiveValue();
	}

	public static void GetIsCaseSencitiveValue()
	{
		try
		{
			string s = ConfigurationManager.AppSettings["IsCaseSencitive"].ToString();
			if (s == "1")
			{
				IsCaseSencitive = true;
			}
			else
			{
				IsCaseSencitive = false;
			}
		}
		catch
		{
			Util.ShowAlertMessage("Cannot find IsCaseSencitive key on appSettings of web.config!");
		}
	}

	public static string GetActivationServerURL()
	{
		return ConfigurationManager.AppSettings["ActivationServerURL"].ToString();
	}
}
