// Util
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Win32;

public class Util
{
	public static string EncryptSHA1(string plain)
	{
		return FormsAuthentication.HashPasswordForStoringInConfigFile(plain, "SHA1");
	}

	public static string GetPageTitle(Page Page)
	{
		string Result = "";
		string PageName = Path.GetFileName(Page.Request.Path);
		string ModuleID = Command.ExecScalar("SELECT ID FROM modules WHERE URL='" + PageName + "'", "");
		ArrayList ModuleNames = new ArrayList();
		while (ModuleID != "")
		{
			string ModuleName = Command.ExecScalar("SELECT Name FROM modules WHERE ID='" + ModuleID + "'", "");
			ModuleNames.Add(ModuleName);
			ModuleID = Command.ExecScalar("SELECT ParentID FROM modules WHERE ID='" + ModuleID + "'", "");
		}
		for (int i = ModuleNames.Count - 1; i >= 0; i--)
		{
			string ModuleName = ModuleNames[i].ToString();
			Result = ((i <= 0) ? (Result + ModuleName) : (Result + ModuleName + " ► "));
		}
		return Result;
	}

	public static void RaiseMessage(string MessageContent)
	{
		MyApplication.CurrentErrorMessage = MessageContent;
		if (HttpContext.Current != null && HttpContext.Current.Session != null)
		{
			HttpContext.Current.Session.Add(MySession.CurrentErrorMessage, MessageContent);
		}
		CreateLog(MessageContent);
		if (MyApplication.IsDebug)
		{
			ShowAlertMessage(MessageContent);
		}
		MyApplication.CurrentErrorMessage = MessageContent.Replace("\n", "\\n").Replace("\r", "\\r");
	}

	public static void CreateLog(string LogEntry)
	{
		try
		{
			string PageName = "";
			try
			{
				if (HttpContext.Current != null)
				{
					Page page = HttpContext.Current.Handler as Page;
					PageName = Path.GetFileName(page.Request.Path);
				}
			}
			catch
			{
			}
			if (!Directory.Exists(MyApplication.LogPath))
			{
				Directory.CreateDirectory(MyApplication.LogPath);
			}
			string BasePath = MyApplication.LogPath;
			string FilePath = BasePath + "\\" + String.Format("{0:yyyy-MM-dd}",DateTime.Now) + "." + MyApplication.LogExt;
			LogEntry = String.Format("{0:yyyy-MM-dd HH:mm:ss}",DateTime.Now) + " : " + PageName + "," + LogEntry + Environment.NewLine;
			File.AppendAllText(FilePath, LogEntry);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string HexConverter(Color c)
	{
		return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
	}

	public static string GetRandomHexColor()
	{
		Random rd = new Random();
		return String.Format("{0:X6}",rd.Next(16777216));
	}

	public static string RemoveInvalidCharacter(string Str)
	{
		return Regex.Replace(Str, "[^\\w\\- ]", "");
	}

	public static bool Ping(string HostName)
	{
		Ping ping = new Ping();
		PingReply pingreply = ping.Send(HostName);
		if (pingreply.Status == IPStatus.Success)
		{
			return true;
		}
		return false;
	}

	public static string GetServerIPAddress()
	{
		List<IPAddress> ipList = new List<IPAddress>();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface ni in allNetworkInterfaces)
		{
			foreach (UnicastIPAddressInformation ua in ni.GetIPProperties().UnicastAddresses)
			{
				if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
				{
					return ua.Address.ToString();
				}
			}
		}
		return "";
	}

	public static string GetIPAddress()
	{
		IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
		IPAddress[] addressList = host.AddressList;
		foreach (IPAddress ip in addressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				return ip.ToString();
			}
		}
		return "";
	}

	public static string GetUserHost(Page p)
	{
		return string.IsNullOrEmpty(p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? p.Request.ServerVariables["REMOTE_ADDR"] : p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
	}

	public static string GetUserHostMasterPage(MasterPage p)
	{
		return string.IsNullOrEmpty(p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? p.Request.ServerVariables["REMOTE_ADDR"] : p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
	}

	public static bool IsUrl()
	{
		string str1 = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
		string str2 = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
		return str1 != null && str1.IndexOf(str2) == 7;
	}

	public static void ShowAlertMessage(string error, string title = "Peringatan!", string extended_script = "", string icon = "fa fa-warning")
	{

         if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
		if (page != null )
		{
			error = error.Replace("'", "`");
			error = error.Replace("\\", "\\\\");
			error = error.Replace("\r", " ");
			error = error.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "$.alert({title: '" + title + "'," + ((!string.IsNullOrEmpty(icon)) ? ("icon: '" + icon + "',") : "") + "content: '" + error + "',buttons: {    okay: {        text: 'OK',        btnClass: 'btn-green'    }}});" + extended_script, addScriptTags: true);
		}
	}

	public static void ShowAlertMessage(string error, string location)
	{
        if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
        if (page != null)
     	{
			error = error.Replace("'", "`");
			error = error.Replace("\\", "\\\\");
			error = error.Replace("\r", " ");
			error = error.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');window.location='" + location + "';", addScriptTags: true);
		}
	}

	public static void ShowAlertError()
	{
        string error = "Gagal menyimpan data, hubungi team pengembang sistem.";
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
        if (page != null)
        {

			error = error.Replace("'", "`");
			error = error.Replace("\\", "\\\\");
			error = error.Replace("\r", " ");
			error = error.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", addScriptTags: true);
		}
	}

	public static void ShowMsgAndGoToPage(string msg, string url)
	{
        if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
        if (page != null)
        {
			msg = msg.Replace("'", "`");
			msg = msg.Replace("\\", "\\\\");
			msg = msg.Replace("\r", " ");
			msg = msg.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + msg + "');window.location ='" + url + "';", addScriptTags: true);
		}
	}

	public static void GoToPage(string url)
	{
        if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
        if (page != null)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "gotoPage", "window.location ='" + url + "';", addScriptTags: true);
		}
	}

	public static void ClearControls(ControlCollection ctrls)
	{
		foreach (Control ctrl in ctrls)
		{
			if (ctrl is TextBox)
			{
				((TextBox)ctrl).Text = string.Empty;
			}
			ClearControls(ctrl.Controls);
		}
	}

	public static string GetPostRequest(string Key)
	{
		string str = "";
		if (HttpContext.Current.Request[Key] != null)
		{
			str = HttpContext.Current.Request[Key];
		}
		return str.Trim();
	}

	public static string GetRequest(string Key)
	{
		string str = "";
		if (HttpContext.Current.Request[Key] != null)
		{
			str = HttpContext.Current.Request[Key];
		}
		return str.Trim().ToLower();
	}

	public static bool IsExist(string[] List, string CheckString)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i] == CheckString)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsStartsWithExist(string[] List, string CheckString)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].StartsWith(CheckString))
			{
				return true;
			}
		}
		return false;
	}

	public static int GetIndex(string[] List, string CheckString)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i] == CheckString)
			{
				return i;
			}
		}
		return -1;
	}

	public static int GetIndex(DataTable dt, string ColumnName, string CheckString)
	{
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			if (dt.Rows[i][ColumnName].ToString() == CheckString)
			{
				return i;
			}
		}
		return -1;
	}

	public static ArrayList GetStartsWithIndex(string[] List, string CheckString)
	{
		ArrayList Result = new ArrayList();
		if (List == null)
		{
			return Result;
		}
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].StartsWith(CheckString))
			{
				Result.Add(i);
			}
		}
		return Result;
	}

	public static ArrayList GetIndexs(string[] List, string CheckString)
	{
		ArrayList Result = new ArrayList();
		if (List == null)
		{
			return Result;
		}
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i] == CheckString)
			{
				Result.Add(i);
			}
		}
		return Result;
	}

	public static ArrayList GetIndexsStartWith(string[] List, string CheckString)
	{
		ArrayList Result = new ArrayList();
		if (List == null)
		{
			return Result;
		}
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].StartsWith(CheckString))
			{
				Result.Add(i);
			}
		}
		return Result;
	}

	public static ArrayList GetIndexs(string[] List, ArrayList ListCheckString)
	{
		ArrayList Result = new ArrayList();
		if (List == null)
		{
			return Result;
		}
		for (int i = 0; i < List.Length; i++)
		{
			foreach (string CheckString in ListCheckString)
			{
				if (List[i] == CheckString)
				{
					Result.Add(i);
				}
			}
		}
		return Result;
	}

	public static string UpperFirst(string s)
	{
		return Regex.Replace(s, "\\b[a-z]\\w+", delegate(Match match)
		{
			string text = match.ToString();
			return char.ToUpper(text[0]) + text.Substring(1);
		});
	}

	public static string CollapseSpaces(string value)
	{
		return Regex.Replace(value, "\\s+", " ");
	}

	public static void SelectText(TextBox txt)
	{
		if (ScriptManager.GetCurrent(txt.Page) != null && ScriptManager.GetCurrent(txt.Page).IsInAsyncPostBack)
		{
			ScriptManager.RegisterStartupScript(txt.Page, txt.Page.GetType(), "SetFocusInUpdatePanel-" + txt.ClientID, String.Format("ctrlToSelect='{0}';",txt.ClientID), addScriptTags: true);
		}
		else
		{
			txt.Page.ClientScript.RegisterStartupScript(txt.Page.GetType(), "Select-" + txt.ClientID, String.Format("document.getElementById('{0}').select();",txt.ClientID), addScriptTags: true);
		}
	}

	public static string GetRoman(int number)
	{
		if (number <= 0)
		{
			return "";
		}
		int[] Nums = new int[13]
		{
			1, 4, 5, 9, 10, 40, 50, 90, 100, 400,
			500, 900, 1000
		};
		string[] RomanNums = new string[13]
		{
			"I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD",
			"D", "M", "M"
		};
		string sRtn = "";
		int j = number;
		for (int i = Nums.Length - 1; i >= 0; i--)
		{
			while (j >= Nums[i])
			{
				j -= Nums[i];
				sRtn += RomanNums[i];
			}
		}
		return sRtn;
	}

	public static bool ConvertToBoolean(object Value)
	{
		if (Value == null || string.IsNullOrEmpty(Value.ToString()))
		{
			return false;
		}
		if (Value.ToString().ToLower() == "false" || Value.ToString() == "0")
		{
			return false;
		}
		return true;
	}

	
    public static int MonthIndexEn(string MonthName)
    {
        switch (MonthName)
        {
            case "Jan":
                return 1;
            case "Feb":
                return 2;
            case "Mar":
                return 3;
            case "Apr":
                return 4;
            case "May":
                return 5;
            case "Jun":
                return 6;
            case "Jul":
                return 7;
            case "Aug":
                return 8;
            case "Sep":
                return 9;
            case "Oct":
                return 10;
            case "Nov":
                return 11;
            case "Dec":
                return 12;
            default:
                return 0;
        }



	}

	
	public static int MonthIndexCombo(string MonthName)
	{
        switch (MonthName)
        {
            case "Jan":
                return 1;
            case "Feb":
                return 2;
            case "Mar":
                return 3;
            case "Apr":
                return 4;
            case "Mei":
                return 5;
            case "May":
                return 5;
            case "Jun":
                return 6;
            case "Jul":
                return 7;
            case "Agust":
                return 8;
            case "Aug":
                return 8;
            case "Sep":
                return 9;
            case "Okt":
                return 10;
            case "Nop":
                return 11;
            case "Des":
                return 12;
            case "Oct":
                return 10;
            case "Nov":
                return 11;
            case "Dec":
                return 12;
            default:
                return 0;
        }
	}

	public static void CreateDirectories(DirectoryInfo instance)
	{
		if (instance.Parent != null)
		{
			CreateDirectories(instance.Parent);
		}
		if (!instance.Exists)
		{
			instance.Create();
		}
	}

	public static bool IsValidEmail(string EmailAddress)
	{
		try
		{
			MailAddress i = new MailAddress(EmailAddress);
			return true;
		}
		catch (FormatException)
		{
			return false;
		}
	}

	public static bool IsValidPhoneNumber(string PhoneNumber)
	{
		if (!Regex.IsMatch(PhoneNumber, "^\\+?(\\d[\\d-. ]+)?(\\([\\d-. ]+\\))?[\\d-. ]+\\d$"))
		{
			return false;
		}
		return true;
	}

	public static void DeleteFolder(string FolderName)
	{
		try
		{
			DirectoryInfo dir = new DirectoryInfo(FolderName);
			dir.Delete(recursive: true);
		}
		catch
		{
			ShowAlertMessage("Cannot delete directory : " + FolderName);
		}
	}

	private static ImageCodecInfo GetEncoderInfo(string mimeType)
	{
		ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
		for (int i = 0; i < encoders.Length; i++)
		{
			if (encoders[i].MimeType == mimeType)
			{
				return encoders[i];
			}
		}
		return null;
	}

	public static void ResizeImage(string fileName, string outputFileName, int NewWidth, int NewHeight, int newResolution, string newCodec, int qualityLevel, bool IsStretch)
	{
		System.Drawing.Image original = System.Drawing.Image.FromFile(fileName);
		float aspect = (float)original.Height / (float)original.Width;
		int newHeight = (int)((float)NewWidth * aspect);
		if (IsStretch)
		{
			newHeight = NewHeight;
		}
		int newWidth = NewWidth;
		if (NewWidth == 0)
		{
			newWidth = (int)((float)NewHeight * ((float)original.Width / (float)original.Height));
			newHeight = NewHeight;
		}
		Bitmap temp = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
		temp.MakeTransparent();
		temp.SetResolution(newResolution, newResolution);
		Graphics newImage = Graphics.FromImage(temp);
		newImage.DrawImage(original, 0, 0, newWidth, newHeight);
		temp.Save(outputFileName);
		original.Dispose();
		temp.Dispose();
		newImage.Dispose();
	}

	public static string EscapeURL(string Input)
	{
		return HttpUtility.JavaScriptStringEncode(Input).ToString().Replace("\"", "&quot;")
			.Replace("+", "%2B");
	}

	public static string GetComputer_LanIP()
	{
		string strHostName = Dns.GetHostName();
		IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
		IPAddress[] addressList = ipEntry.AddressList;
		foreach (IPAddress ipAddress in addressList)
		{
			if (ipAddress.AddressFamily.ToString() == "InterNetwork")
			{
				return ipAddress.ToString();
			}
		}
		return "-";
	}

	public static string RemoveAccent(string txt)
	{
		byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
		return Encoding.ASCII.GetString(bytes);
	}

	public static string Slugify(string phrase)
	{
		string str = RemoveAccent(phrase).ToLower();
		str = Regex.Replace(str, "[^a-z0-9\\s-]", "");
		str = Regex.Replace(str, "\\s+", " ").Trim();
		return Regex.Replace(str, "\\s", "-");
	}

	public static string BytesToString(long byteCount)
	{
		string[] suf = new string[7] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
		if (byteCount == 0)
		{
			return "0" + suf[0];
		}
		long bytes = Math.Abs(byteCount);
		int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024.0)));
		double num = Math.Round((double)bytes / Math.Pow(1024.0, place), 1);
		return (double)Math.Sign(byteCount) * num + " " + suf[place];
	}

	public static string BytesToStringKB(long byteCount)
	{
		return String.Format("{0:N0} KB",(byteCount/ 1024));
	}

	public static string TrimEnd(string inputText, string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			while (!string.IsNullOrEmpty(inputText) && inputText.EndsWith(value, StringComparison.CurrentCultureIgnoreCase))
			{
				inputText = inputText.Substring(0, inputText.Length - value.Length);
			}
		}
		return inputText;
	}

	public static string ReplaceAt(string str, int index, int length, string replace)
	{
		return str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);
	}

	public static List<string> ToListID(DataTable dt)
	{
		List<string> result = new List<string>();
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			result.Add(dt.Rows[i][0].ToString());
		}
		return result;
	}

	public static string DigitGrouping(string Input)
	{
		if (string.IsNullOrEmpty(Input))
		{
			return "0";
		}
		return decimal.Parse(Input).ToString("#,##0", new CultureInfo("id-ID"));
	}

	public static string GetMimeType(string fileName)
	{
		string mimeType = "application/unknown";
		string ext = Path.GetExtension(fileName).ToLower();
		RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
		if (regKey != null && regKey.GetValue("Content Type") != null)
		{
			mimeType = regKey.GetValue("Content Type").ToString();
		}
		return mimeType;
	}

	public static string TruncateWord(string WordTeks, int WordCount)
	{
		string Result = "";
		string[] s = WordTeks.Split(char.Parse(" "));
		if (s.Length > WordCount)
		{
			string s2 = "";
			for (int i = 0; i < WordCount; i++)
			{
				s2 = s2 + s[i] + " ";
			}
			Result += s2.TrimEnd();
			if (s.Length > WordCount)
			{
				Result += "...";
			}
		}
		else
		{
			Result = WordTeks;
		}
		return Result;
	}

	public static void SetEncryptedCookie(string name, string value)
	{
		byte[] plaintextValueBytes = Encoding.UTF8.GetBytes(value);
		HttpContext.Current.Response.Cookies[name].Value = MachineKey.Encode(plaintextValueBytes, MachineKeyProtection.All);
		HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddYears(1);
	}

	public static string GetEncryptedCookie(string name)
	{
		try
		{
			string plaintext = HttpContext.Current.Request.Cookies[name].Value;
			byte[] decryptedBytes = MachineKey.Decode(plaintext, MachineKeyProtection.All);
			return Encoding.UTF8.GetString(decryptedBytes);
		}
		catch
		{
			return null;
		}
	}

	public static string NmPerpus()
	{
		Connection.SetConnection();
		string SQL = "SELECT Value FROM settingparameters WHERE Name='NamaPerpustakaan'";
		DataTable dt = Command.ExecDataAdapter(SQL, null);
		return dt.Rows[0]["Value"].ToString();
	}

	public static string UrlEncode(string encode)
	{
		if (encode == null)
		{
			return null;
		}
		string encoded = "";
		foreach (char c in encode)
		{
			int val = c;
			switch (val)
			{
			default:
				if ((val < 65 || val > 90) && (val < 97 || val > 122))
				{
					break;
				}
				goto case 32;
			case 32:
			case 45:
			case 48:
			case 49:
			case 50:
			case 51:
			case 52:
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
				encoded += c;
				continue;
			}
			encoded = encoded + "%" + val.ToString("X");
		}
		return encoded.Replace("%25", "-25").Replace("%2A", "-2A").Replace("%26", "-26")
			.Replace("%3A", "-3A");
	}

	public static string UrlDecode(string decode)
	{
		if (decode == null)
		{
			return null;
		}
		decode = decode.Replace("-25", "%25").Replace("-2A", "%2A").Replace("-26", "%26")
			.Replace("-3A", "%3A");
		return HttpUtility.UrlDecode(decode);
	}
}
