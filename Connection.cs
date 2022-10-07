// Connection
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

public class Connection
{
	public enum EServerType
	{
		Oracle,
		MySQL,
		MsSQL,
		ISRC,
		EDeposit
	}

	public static string ConnectionStringMySQL = null;

	public static string ConnectionStringOracle = null;

	public static string ConnectionStringMsSQL = null;

	public static string ConnectionStringISRC = null;

	public static string ConnectionStringEDeposit = null;

	public static string ParameterSymbolMySQL = "?";

	public static string ParameterSymbolOracle = ":";

	public static string ParameterSymbolMsSQL = "@";

	public static string ParameterSymbolISRC = "?";

	public static string ParameterSymbolEDeposit = "?";

	public static EServerType ServerType;

	public static string ParameterSymbol;

	public static ArrayList GetServerType()
	{
		ArrayList Result = new ArrayList();
		string[] names = Enum.GetNames(typeof(EServerType));
		foreach (string volume in names)
		{
			Result.Add(volume);
		}
		return Result;
	}

	public static void LoadDropDownServerType(DropDownList ddl)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("Nama");
		string[] names = Enum.GetNames(typeof(EServerType));
		foreach (string ServerType in names)
		{
			DataRow row = dt.NewRow();
			row[0] = ServerType;
			row[1] = ServerType;
			dt.Rows.Add(row);
		}
		ddl.DataValueField = "Id";
		ddl.DataTextField = "Nama";
		ddl.DataSource = dt;
		ddl.DataBind();
	}

	public static void SetConnectionString(EServerType ServerType, string ConnString)
	{
		Connection.ServerType = ServerType;
		switch (ServerType)
		{
		case EServerType.MySQL:
			ParameterSymbol = ParameterSymbolMySQL;
			ConnectionStringMySQL = ConnString;
			break;
		case EServerType.Oracle:
			ParameterSymbol = ParameterSymbolOracle;
			ConnectionStringOracle = ConnString;
			break;
		case EServerType.MsSQL:
			ParameterSymbol = ParameterSymbolMsSQL;
			ConnectionStringMsSQL = ConnString;
			break;
		case EServerType.ISRC:
			ParameterSymbol = ParameterSymbolISRC;
			ConnectionStringISRC = ConnString;
			break;
		case EServerType.EDeposit:
			ParameterSymbol = ParameterSymbolEDeposit;
			ConnectionStringEDeposit = ConnString;
			break;
		}
	}

	public static void SetConnection()
	{
		string Driver = ConfigurationManager.AppSettings["Driver"];
		string strServerType = "";
		switch (Driver.ToUpper())
		{
		case "ORACLE":
			ServerType = EServerType.Oracle;
			strServerType = "Oracle";
			break;
		case "MYSQL":
			ServerType = EServerType.MySQL;
			strServerType = "MySQL";
			break;
		case "MsSQL":
			ServerType = EServerType.MsSQL;
			strServerType = "MsSQL";
			break;
		case "ISRC":
			ServerType = EServerType.ISRC;
			strServerType = "ISRC";
			break;
		case "EDeposit":
			ServerType = EServerType.EDeposit;
			strServerType = "EDeposit";
			break;
		}
		string ConnString = ConfigurationManager.ConnectionStrings["ConnectionString" + strServerType].ConnectionString;
		CCryptography.Rijndael.Key = Convert.FromBase64String(MyApplication.SavedKeyApplicationSetting);
		CCryptography.Rijndael.IV = Convert.FromBase64String(MyApplication.SavedIVApplicationSetting);
		ConnString = CCryptography.Rijndael.Decrypt(ConnString, MyApplication.SavedKeyApplicationSetting, MyApplication.SavedIVApplicationSetting);
		SetConnectionString(ServerType, ConnString);
	}

	public static void SetConnectionMySQL()
	{
		if (string.IsNullOrEmpty(ConnectionStringMySQL))
		{
			string ConnString = ConfigurationManager.ConnectionStrings["ConnectionStringMySQL"].ConnectionString;
			CCryptography.Rijndael.Key = Convert.FromBase64String(MyApplication.SavedKeyApplicationSetting);
			CCryptography.Rijndael.IV = Convert.FromBase64String(MyApplication.SavedIVApplicationSetting);
			ConnString = CCryptography.Rijndael.Decrypt(ConnString, MyApplication.SavedKeyApplicationSetting, MyApplication.SavedIVApplicationSetting);
			SetConnectionString(EServerType.MySQL, ConnString);
		}
		else
		{
			ServerType = EServerType.MySQL;
			ParameterSymbol = ParameterSymbolMySQL;
		}
	}

	public static void SetConnectionISRC()
	{
		if (string.IsNullOrEmpty(ConnectionStringISRC))
		{
			string ConnString = ConfigurationManager.ConnectionStrings["ConnectionStringISRC"].ConnectionString;
			CCryptography.Rijndael.Key = Convert.FromBase64String(MyApplication.SavedKeyApplicationSetting);
			CCryptography.Rijndael.IV = Convert.FromBase64String(MyApplication.SavedIVApplicationSetting);
			ConnString = CCryptography.Rijndael.Decrypt(ConnString, MyApplication.SavedKeyApplicationSetting, MyApplication.SavedIVApplicationSetting);
			SetConnectionString(EServerType.ISRC, ConnString);
		}
		else
		{
			ServerType = EServerType.ISRC;
			ParameterSymbol = ParameterSymbolISRC;
		}
	}

	public static void SetConnectionEDeposit()
	{
		if (string.IsNullOrEmpty(ConnectionStringEDeposit))
		{
			string ConnString = ConfigurationManager.ConnectionStrings["ConnectionStringEDeposit"].ConnectionString;
			CCryptography.Rijndael.Key = Convert.FromBase64String(MyApplication.SavedKeyApplicationSetting);
			CCryptography.Rijndael.IV = Convert.FromBase64String(MyApplication.SavedIVApplicationSetting);
			ConnString = CCryptography.Rijndael.Decrypt(ConnString, MyApplication.SavedKeyApplicationSetting, MyApplication.SavedIVApplicationSetting);
			SetConnectionString(EServerType.EDeposit, ConnString);
		}
		else
		{
			ServerType = EServerType.EDeposit;
			ParameterSymbol = ParameterSymbolEDeposit;
		}
	}
}
