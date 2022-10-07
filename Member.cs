// Member
using System;
using System.Data;

public class Member
{
	public static string GetMaxMemberNo(DateTime CreateDate)
	{
		string Result = "";
		return Command.ExecScalar("SELECT MAX(MemberNo) FROM members where MemberNo LIKE '" + String.Format("{0:yyMMdd}",CreateDate) + "%'", "0");
	}

	public static string RandomChar(string NickDef, int MaxVal)
	{
		string chars = "1,2,3,4,5,6,7,8,9,0";
		string NickCook = "";
		string[] arrStr = chars.Split(",".ToCharArray());
		string strDraw = string.Empty;
		Random r = new Random();
		for (int i = 1; i <= MaxVal; i++)
		{
			strDraw += arrStr[r.Next(0, arrStr.Length - 1)];
		}
		return NickDef.TrimEnd() + "_" + strDraw;
	}

	public static string NickGenerator(string FullName, int LengthPostfix)
	{
		int lengtToTrim = 6;
		if (FullName.Length < 6)
		{
			lengtToTrim = FullName.Length;
		}
		string NickDef = FullName.Trim().Substring(0, lengtToTrim);
		string NickGenerated = "";
		string NickCook = "";
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("NickDef", NickDef + "%'");
		string SQL = "SELECT NickName FROM membersonline WHERE NickName LIKE " + Connection.ParameterSymbol + "NickDef";
		DataTable dt = Command.ExecDataAdapter(SQL, tar);
		if (dt.Rows.Count == 0)
		{
			NickGenerated = NickDef;
		}
		else
		{
			NickCook = RandomChar(NickDef, LengthPostfix);
			for (int i = 0; i <= dt.Rows.Count; i++)
			{
				if (NickCook != dt.Rows[i]["NickName"].ToString())
				{
					NickGenerated = NickCook;
					break;
				}
				NickCook = RandomChar(NickCook.Substring(0, lengtToTrim).ToString(), LengthPostfix);
			}
			NickGenerated = NickCook;
		}
		return NickGenerated.ToLower();
	}

	public static string getIdBranch(string Code)
	{
		return Command.ExecScalar("SELECT ID FROM branchs WHERE UPPER(CODE) ='" + Code.Trim().ToUpper() + "'", "0");
	}
}
