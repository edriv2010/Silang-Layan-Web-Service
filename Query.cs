// Query
public class Query
{
	public static string TestConnection
	{
		get
		{
			if (Connection.ServerType == Connection.EServerType.Oracle)
			{
				return "SELECT 1 FROM DUAL";
			}
			return "SELECT 1";
		}
	}

    public static string UserPasswordEncode
    {
        get
        {
            switch (Connection.ServerType)
            {
                case Connection.EServerType.Oracle:
                    return "SELECT to_char(rawtohex(dbms_crypto.hash(utl_raw.cast_to_raw(" + Connection.ParameterSymbol + "userpassword" + "),3))) FROM dual";
                case Connection.EServerType.MySQL:
                    return "SELECT CONVERT(SHA1(" + Connection.ParameterSymbol + "userpassword) USING UTF8)";
                default:
                    return "";
            }
        }
    }

}
