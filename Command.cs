// Command
using System;
using System.Data;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

public class Command
{
	public enum InsertOrUpdate
	{
		Insert,
		Update
	}

	public class ExecInsertOrUpdateWithTrans
	{
		private OracleConnection connOracle;

		private OracleCommand cmdOracle;

		private OracleTransaction transOracle = null;

		private MySqlConnection connMySQL;

		private MySqlCommand cmdMySQL;

		private MySqlTransaction transMySQL = null;

		public void BeginTransaction()
		{
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				try
				{
					connOracle = new OracleConnection(Connection.ConnectionStringOracle);
					cmdOracle = new OracleCommand();
					connOracle.Open();
					cmdOracle = connOracle.CreateCommand();
					transOracle = connOracle.BeginTransaction();
					cmdOracle.Connection = connOracle;
					cmdOracle.Transaction = transOracle;
					break;
				}
				catch (OracleException ex2)
				{
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex4)
				{
					Util.RaiseMessage(ex4.Message + Environment.NewLine);
					break;
				}
				finally
				{
					if (cmdOracle != null)
					{
						cmdOracle.Dispose();
					}
					if (connOracle != null)
					{
						connOracle.Close();
					}
					if (connOracle != null)
					{
						connOracle.Dispose();
					}
				}
			case Connection.EServerType.MySQL:
				try
				{
					connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
					cmdMySQL = new MySqlCommand();
					connMySQL.Open();
					cmdMySQL = connMySQL.CreateCommand();
					transMySQL = connMySQL.BeginTransaction();
					cmdMySQL.Connection = connMySQL;
					cmdMySQL.Transaction = transMySQL;
					break;
				}
				catch (MySqlException ex3)
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
					Util.RaiseMessage(ex3.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex4)
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
					Util.RaiseMessage(ex4.Message + Environment.NewLine);
					break;
				}
				finally
				{
				}
			}
		}

		public void CommitTransaction()
		{
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				try
				{
					if (transOracle != null && transOracle.Connection != null)
					{
						transOracle.Commit();
					}
					break;
				}
				catch (OracleException ex2)
				{
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex4)
				{
					Util.RaiseMessage(ex4.Message + Environment.NewLine);
					break;
				}
				finally
				{
					if (cmdOracle != null)
					{
						cmdOracle.Dispose();
					}
					if (connOracle != null)
					{
						connOracle.Close();
					}
					if (connOracle != null)
					{
						connOracle.Dispose();
					}
				}
			case Connection.EServerType.MySQL:
				try
				{
					if (transMySQL != null && transMySQL.Connection != null)
					{
						transMySQL.Commit();
					}
					break;
				}
				catch (MySqlException ex3)
				{
					Util.RaiseMessage(ex3.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex4)
				{
					Util.RaiseMessage(ex4.Message + Environment.NewLine);
					break;
				}
				finally
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
				}
			}
		}

		public static object GenerateSQLInsertCommand(object CmdObj, string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
		{
			object Cmd = null;
			string strFieldName = null;
			string strFieldData = null;
			string strUpdateData = null;
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
			{
				OracleCommand CmdOracle = (OracleCommand)CmdObj;
				CmdOracle.Parameters.Clear();
				switch (InsertOrUpdate)
				{
				case InsertOrUpdate.Insert:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						strFieldName = strFieldName + FieldValues.Item1(i) + ",";
						strFieldData = strFieldData + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						CmdOracle.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						strFieldName = strFieldName.Remove(strFieldName.Length - 1, 1);
						strFieldData = strFieldData.Remove(strFieldData.Length - 1, 1);
					}
					CmdOracle.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
					break;
				}
				case InsertOrUpdate.Update:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						string text = strUpdateData;
						strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						CmdOracle.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
					}
					CmdOracle.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
					break;
				}
				}
				Cmd = CmdOracle;
				break;
			}
			case Connection.EServerType.MySQL:
			{
				MySqlCommand CmdMySQL = (MySqlCommand)CmdObj;
				CmdMySQL.Parameters.Clear();
				switch (InsertOrUpdate)
				{
				case InsertOrUpdate.Insert:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						strFieldName = strFieldName + FieldValues.Item1(i) + ",";
						strFieldData = strFieldData + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						CmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						strFieldName = strFieldName.Remove(strFieldName.Length - 1, 1);
						strFieldData = strFieldData.Remove(strFieldData.Length - 1, 1);
					}
					CmdMySQL.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
					break;
				}
				case InsertOrUpdate.Update:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						string text = strUpdateData;
						strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						CmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
					}
					CmdMySQL.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
					break;
				}
				}
				Cmd = CmdMySQL;
				break;
			}
			default:
				Util.RaiseMessage("Server Type doesn't recognized!");
				break;
			}
			return Cmd;
		}

		public bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
		{
			bool Result = false;
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				try
				{
					cmdOracle = (OracleCommand)GenerateSQLInsertCommand(cmdOracle, TableName, FieldValues, InsertOrUpdate, SQLWhere);
					cmdOracle.ExecuteNonQuery();
					Result = true;
				}
				catch (OracleException ex4)
				{
					if (transOracle != null && transOracle.Connection != null)
					{
						transOracle.Rollback();
					}
					Util.RaiseMessage(ex4.Message + Environment.NewLine);
				}
				catch (Exception ex3)
				{
					if (transOracle != null && transOracle.Connection != null)
					{
						transOracle.Rollback();
					}
					Util.RaiseMessage(ex3.Message + Environment.NewLine);
				}
				finally
				{
					if (cmdOracle != null)
					{
						cmdOracle.Dispose();
					}
					if (connOracle != null)
					{
						connOracle.Close();
					}
					if (connOracle != null)
					{
						connOracle.Dispose();
					}
				}
				break;
			case Connection.EServerType.MySQL:
				try
				{
					cmdMySQL = (MySqlCommand)GenerateSQLInsertCommand(cmdMySQL, TableName, FieldValues, InsertOrUpdate, SQLWhere);
					cmdMySQL.ExecuteNonQuery();
					Result = true;
				}
				catch (MySqlException ex2)
				{
					try
					{
						if (transMySQL != null && transMySQL.Connection != null)
						{
							transMySQL.Rollback();
						}
					}
					finally
					{
						Util.RaiseMessage(ex2.Message + Environment.NewLine);
					}
				}
				catch (Exception ex3)
				{
					try
					{
						if (transMySQL != null && transMySQL.Connection != null)
						{
							transMySQL.Rollback();
						}
					}
					finally
					{
						Util.RaiseMessage(ex3.Message + Environment.NewLine);
					}
				}
				finally
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
				}
				break;
			}
			return Result;
		}
	}

	public static bool TestConnection()
	{
		bool Result = true;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection connOracle = null;
			OracleCommand cmdOracle = null;
			try
			{
				connOracle = new OracleConnection(Connection.ConnectionStringOracle);
				cmdOracle = new OracleCommand();
				cmdOracle.Connection = connOracle;
				cmdOracle.CommandText = Query.TestConnection;
				connOracle.Open();
				cmdOracle.ExecuteScalar();
			}
			catch (OracleException ex2)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex2.Message);
				Result = false;
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex7.Message);
				Result = false;
			}
			finally
			{
				if (connOracle != null) connOracle.Close();
                if (connOracle != null) connOracle.Dispose();
                if (cmdOracle != null) cmdOracle.Dispose();
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection connMySQL = null;
			MySqlCommand cmdMySQL = null;
			try
			{
				connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
				cmdMySQL = new MySqlCommand();
				cmdMySQL.Connection = connMySQL;
				cmdMySQL.CommandText = Query.TestConnection;
				connMySQL.Open();
				cmdMySQL.ExecuteScalar();
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex8.Message);
				Result = false;
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex7.Message);
				Result = false;
			}
			finally
			{
                if (cmdMySQL != null)
				{
					cmdMySQL.Dispose();
				}
				if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}
			}
			break;
		}
		case Connection.EServerType.ISRC:
		{
			MySqlConnection connISRC = null;
			MySqlCommand cmdISRC = null;
			try
			{
				connISRC = new MySqlConnection(Connection.ConnectionStringISRC);
				cmdISRC = new MySqlCommand();
				cmdISRC.Connection = connISRC;
				cmdISRC.CommandText = Query.TestConnection;
				connISRC.Open();
				cmdISRC.ExecuteScalar();
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex8.Message);
				Result = false;
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex7.Message);
				Result = false;
			}
			finally
			{

   				if (connISRC != null)
				{
					connISRC.Close();
				}

                if (connISRC != null)
				{
					connISRC.Dispose();
				}
				if (cmdISRC != null)
				{
					cmdISRC.Dispose();
				}

			}
			break;
		}
		case Connection.EServerType.EDeposit:
		{
			MySqlConnection connEDeposit = null;
			MySqlCommand cmdEDeposit = null;
			try
			{
				connEDeposit = new MySqlConnection(Connection.ConnectionStringEDeposit);
				cmdEDeposit = new MySqlCommand();
				cmdEDeposit.Connection = connEDeposit;
				cmdEDeposit.CommandText = Query.TestConnection;
				connEDeposit.Open();
				cmdEDeposit.ExecuteScalar();
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex8.Message);
				Result = false;
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex7.Message);
				Result = false;
			}
			finally
			{
                
				if (connEDeposit != null)
				{
					connEDeposit.Close();
				}
				if (connEDeposit != null)
				{
					connEDeposit.Dispose();
				}
                if (cmdEDeposit != null)
				{
					cmdEDeposit.Dispose();
				}
			}
			break;
		}
		}
		return Result;
	}

	public static string ExecScalar(string SQL)
	{
		return ExecScalar(null, SQL, "");
	}

	public static string ExecScalar(string SQL, string NullValue)
	{
		return ExecScalar(null, SQL, NullValue);
	}

	public static string ExecScalar(TwoArrayList Parameter, string SQL)
	{
		return ExecScalar(Parameter, SQL, "");
	}

	public static string ExecScalar(TwoArrayList Parameter, string SQL, string NullValue)
	{
		string Result = null;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection connOracle = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand cmdOracle = new OracleCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdOracle.Parameters.Add(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdOracle.BindByName = true;
				cmdOracle.CommandText = SQL;
				cmdOracle.Connection = connOracle;
				connOracle.Open();
				object temp = cmdOracle.ExecuteScalar();
				Result = ((temp != null && !(temp.ToString() == "")) ? temp.ToString() : NullValue);
				temp = null;
			}
			catch (OracleException ex2)
			{
				Result = NullValue;
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdOracle.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Result = NullValue;
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdOracle.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (cmdOracle != null)
				{
					cmdOracle.Dispose();
				}
				if (connOracle != null)
				{
					connOracle.Close();
				}
				if (connOracle != null)
				{
					connOracle.Dispose();
				}
				
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand cmdMySQL = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdMySQL.CommandText = SQL;
				cmdMySQL.Connection = connMySQL;
				connMySQL.Open();
				object temp = cmdMySQL.ExecuteScalar();
				Result = ((temp != null && !(temp.ToString() == "")) ? temp.ToString() : NullValue);
				temp = null;
			}
			catch (MySqlException ex8)
			{
				Result = NullValue;
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Result = NullValue;
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
			}
			finally
			{
               if (cmdMySQL != null)
				{
					cmdMySQL.Dispose();
				}

                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}
			}
			break;
		}
		case Connection.EServerType.ISRC:
		{
			MySqlConnection connISRC = new MySqlConnection(Connection.ConnectionStringISRC);
			MySqlCommand cmdISRC = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdISRC.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdISRC.CommandText = SQL;
				cmdISRC.Connection = connISRC;
				connISRC.Open();
				object temp = cmdISRC.ExecuteScalar();
				Result = ((temp != null && !(temp.ToString() == "")) ? temp.ToString() : NullValue);
				temp = null;
			}
			catch (MySqlException ex8)
			{
				Result = NullValue;
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdISRC.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Result = NullValue;
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdISRC.CommandText : "") + Environment.NewLine);
			}
			finally
			{
               if (cmdISRC != null)
				{
					cmdISRC.Dispose();
				}

                if (connISRC != null)
				{
					connISRC.Close();
				}
				if (connISRC != null)
				{
					connISRC.Dispose();
				}

			}
			break;
		}
		case Connection.EServerType.EDeposit:
		{
			MySqlConnection connEDeposit = new MySqlConnection(Connection.ConnectionStringEDeposit);
			MySqlCommand cmdEDeposit = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdEDeposit.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdEDeposit.CommandText = SQL;
				cmdEDeposit.Connection = connEDeposit;
				connEDeposit.Open();
				object temp = cmdEDeposit.ExecuteScalar();
				Result = ((temp != null && !(temp.ToString() == "")) ? temp.ToString() : NullValue);
				temp = null;
			}
			catch (MySqlException ex8)
			{
				Result = NullValue;
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdEDeposit.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Result = NullValue;
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdEDeposit.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (cmdEDeposit != null)
				{
					cmdEDeposit.Dispose();
				}

                if (connEDeposit != null)
				{
					connEDeposit.Close();
				}
				if (connEDeposit != null)
				{
					connEDeposit.Dispose();
				}
			}
			break;
		}
		}
		return Result;
	}

	public static DataTable ExecDataAdapter(string SQL)
	{
		return ExecDataAdapter(SQL, null);
	}

	public static DataTable ExecDataAdapter(string SQL, TwoArrayList Parameter)
	{
		DataTable Result = new DataTable();
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection connOracle = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand cmdOracle = new OracleCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdOracle.Parameters.Add(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdOracle.BindByName = true;
				cmdOracle.CommandText = SQL;
				cmdOracle.Connection = connOracle;
				connOracle.Open();
				OracleDataAdapter da = new OracleDataAdapter(cmdOracle);
				da.Fill(Result);
				da.Dispose();
			}
			catch (OracleException ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdOracle.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdOracle.CommandText : "") + Environment.NewLine);
			}
			finally
			{

                if (cmdOracle != null)
				{
					cmdOracle.Dispose();
				}

                if (connOracle != null)
				{
					connOracle.Close();
				}
				if (connOracle != null)
				{
					connOracle.Dispose();
				}

			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand cmdMySQL = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdMySQL.CommandText = SQL;
				cmdMySQL.Connection = connMySQL;
				connMySQL.Open();
				MySqlDataAdapter da2 = new MySqlDataAdapter(cmdMySQL);
				da2.Fill(Result);
				da2.Dispose();
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (cmdMySQL != null)
				{
                    cmdMySQL.Dispose();
				}

                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}

			}
			break;
		}
		case Connection.EServerType.ISRC:
		{
			MySqlConnection connISRC = new MySqlConnection(Connection.ConnectionStringISRC);
			MySqlCommand cmdISRC = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdISRC.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdISRC.CommandText = SQL;
				cmdISRC.Connection = connISRC;
				connISRC.Open();
				MySqlDataAdapter da2 = new MySqlDataAdapter(cmdISRC);
				da2.Fill(Result);
				da2.Dispose();
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdISRC.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdISRC.CommandText : "") + Environment.NewLine);
			}
			finally
			{
				if (cmdISRC != null)
				{
					cmdISRC.Dispose();
				}

                if (connISRC != null)
				{
					connISRC.Close();
				}
				if (connISRC != null)
				{
					connISRC.Dispose();
				}
			}
			break;
		}
		case Connection.EServerType.EDeposit:
		{
			MySqlConnection connEDeposit = new MySqlConnection(Connection.ConnectionStringEDeposit);
			MySqlCommand cmdEDeposit = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdEDeposit.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdEDeposit.CommandText = SQL;
				cmdEDeposit.Connection = connEDeposit;
				connEDeposit.Open();
				MySqlDataAdapter da2 = new MySqlDataAdapter(cmdEDeposit);
				da2.Fill(Result);
				da2.Dispose();
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdEDeposit.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdEDeposit.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (cmdEDeposit != null)
				{
					cmdEDeposit.Dispose();
				}

                if (connEDeposit != null)
				{
					connEDeposit.Close();
				}
				if (connEDeposit != null)
				{
					connEDeposit.Dispose();
				}
			}
			break;
		}
		}
		return Result;
	}

	public static bool ExecNonQuery(string SQL)
	{
		return ExecNonQuery(SQL, null);
	}

	public static bool ExecNonQuery(string SQL, TwoArrayList Parameter)
	{
		bool Result = false;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection connOracle = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand cmdOracle = new OracleCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdOracle.Parameters.Add(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
					cmdOracle.BindByName = true;
				}
				cmdOracle.BindByName = true;
				cmdOracle.CommandText = SQL;
				cmdOracle.Connection = connOracle;
				connOracle.Open();
				cmdOracle.ExecuteNonQuery();
				Result = true;
			}
			catch (OracleException ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdOracle.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdOracle.CommandText : "") + Environment.NewLine);
			}
			finally
			{
               if (cmdOracle != null)
				{
					cmdOracle.Dispose();
				}

                if (connOracle != null)
				{
					connOracle.Close();
				}
				if (connOracle != null)
				{
					connOracle.Dispose();
				}

			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand cmdMySQL = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdMySQL.CommandText = SQL;
				cmdMySQL.Connection = connMySQL;
				connMySQL.Open();
				cmdMySQL.ExecuteNonQuery();
				Result = true;
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}
			}
			break;
		}
		case Connection.EServerType.ISRC:
		{
			MySqlConnection connISRC = new MySqlConnection(Connection.ConnectionStringISRC);
			MySqlCommand cmdISRC = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdISRC.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdISRC.CommandText = SQL;
				cmdISRC.Connection = connISRC;
				connISRC.Open();
				cmdISRC.ExecuteNonQuery();
				Result = true;
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdISRC.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdISRC.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (cmdISRC != null)
				{
					cmdISRC.Dispose();
				}

                if (connISRC != null)
				{
					connISRC.Close();
				}
				if (connISRC != null)
				{
					connISRC.Dispose();
				}
			}
			break;
		}
		case Connection.EServerType.EDeposit:
		{
			MySqlConnection connEDeposit = new MySqlConnection(Connection.ConnectionStringEDeposit);
			MySqlCommand cmdEDeposit = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						cmdEDeposit.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				cmdEDeposit.CommandText = SQL;
				cmdEDeposit.Connection = connEDeposit;
				connEDeposit.Open();
				cmdEDeposit.ExecuteNonQuery();
				Result = true;
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdEDeposit.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdEDeposit.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (cmdEDeposit != null)
				{
					cmdEDeposit.Dispose();
				}

                if (connEDeposit != null)
				{
					connEDeposit.Close();
				}
				if (connEDeposit != null)
				{
					connEDeposit.Dispose();
				}

				
			}
			break;
		}
		}
		return Result;
	}

	public static bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		return ExecInsertOrUpdate(TableName, FieldValues, InsertOrUpdate, SQLWhere, null);
	}

	public static bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere, TwoArrayList ParameterWhere)
	{
		bool Result = false;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection connOracle = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand cmdOracle = new OracleCommand();
			try
			{
				cmdOracle = (OracleCommand)GenerateSQLInsertCommand(TableName, FieldValues, InsertOrUpdate, SQLWhere);
				if (ParameterWhere != null)
				{
					for (int i = 0; i <= ParameterWhere.Count() - 1; i++)
					{
						cmdOracle.Parameters.Add(Connection.ParameterSymbol + ParameterWhere.Item1(i), ParameterWhere.Item2(i));
					}
				}
				connOracle.Open();
				cmdOracle.Connection = connOracle;
				cmdOracle.ExecuteNonQuery();
				Result = true;
			}
			catch (OracleException ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + ". For table : " + TableName);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + ". For table : " + TableName);
			}
			finally
			{
                if (cmdOracle != null)
				{
					cmdOracle.Dispose();
				}

                if (connOracle != null)
				{
					connOracle.Close();
				}
				if (connOracle != null)
				{
					connOracle.Dispose();
				}

				
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand cmdMySQL = new MySqlCommand();
			try
			{
				cmdMySQL = (MySqlCommand)GenerateSQLInsertCommand(TableName, FieldValues, InsertOrUpdate, SQLWhere);
				connMySQL.Open();
				cmdMySQL.Connection = connMySQL;
				cmdMySQL.ExecuteNonQuery();
				Result = true;
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + ". For table : " + TableName);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + ". For table : " + TableName);
			}
			finally
			{
               if (cmdMySQL!= null)
				{
					cmdMySQL.Dispose();
				}

                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}


							}
			break;
		}
		case Connection.EServerType.ISRC:
		{
			MySqlConnection connISRC = new MySqlConnection(Connection.ConnectionStringISRC);
			MySqlCommand cmdISRC = new MySqlCommand();
			try
			{
				cmdISRC = (MySqlCommand)GenerateSQLInsertCommand(TableName, FieldValues, InsertOrUpdate, SQLWhere);
				connISRC.Open();
				cmdISRC.Connection = connISRC;
				cmdISRC.ExecuteNonQuery();
				Result = true;
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + ". For table : " + TableName);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + ". For table : " + TableName);
			}
			finally
			{
                if (cmdISRC!= null)
				{
					cmdISRC.Dispose();
				}

                if (connISRC != null)
				{
					connISRC.Close();
				}
				if (connISRC != null)
				{
					connISRC.Dispose();
				}


			}
			break;
		}
		case Connection.EServerType.EDeposit:
		{
			MySqlConnection connEDeposit = new MySqlConnection(Connection.ConnectionStringEDeposit);
			MySqlCommand cmdEDeposit = new MySqlCommand();
			try
			{
				cmdEDeposit = (MySqlCommand)GenerateSQLInsertCommand(TableName, FieldValues, InsertOrUpdate, SQLWhere);
				connEDeposit.Open();
				cmdEDeposit.Connection = connEDeposit;
				cmdEDeposit.ExecuteNonQuery();
				Result = true;
			}
			catch (MySqlException ex8)
			{
				Util.RaiseMessage(ex8.Message + Environment.NewLine + ". For table : " + TableName);
			}
			catch (Exception ex7)
			{
				Util.RaiseMessage(ex7.Message + Environment.NewLine + ". For table : " + TableName);
			}
			finally
			{
                if (cmdEDeposit!= null)
				{
					cmdEDeposit.Dispose();
				}

                if (connEDeposit != null)
				{
					connEDeposit.Close();
				}
				if (connEDeposit != null)
				{
					connEDeposit.Dispose();
				}
				
			}
			break;
		}
		}
		return Result;
	}

	public static object GenerateSQLInsertCommand(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		object Cmd = null;
		string strFieldName = null;
		string strFieldData = null;
		string strUpdateData = null;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleCommand CmdOracle = new OracleCommand();
			switch (InsertOrUpdate)
			{
			case InsertOrUpdate.Insert:
			{
				for (int i = 0; i <= FieldValues.Count() - 1; i++)
				{
					strFieldName = strFieldName + FieldValues.Item1(i) + ",";
					strFieldData = strFieldData + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdOracle.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				strFieldName = strFieldName.TrimEnd(char.Parse(","));
				strFieldData = strFieldData.TrimEnd(char.Parse(","));
				CmdOracle.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
				break;
			}
			case InsertOrUpdate.Update:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					if (FieldValues.Item2(i).ToString().Contains("regexp_replace("))
					{
						string text = strUpdateData;
						strUpdateData = text + FieldValues.Item1(i) + " = " + FieldValues.Item2(i).ToString() + ",";
					}
					else
					{
						string text = strUpdateData;
						strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						CmdOracle.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
				}
				if (i > 0)
				{
					strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
				}
				CmdOracle.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
				break;
			}
			}
			Cmd = CmdOracle;
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlCommand CmdMySQL = new MySqlCommand();
			switch (InsertOrUpdate)
			{
			case InsertOrUpdate.Insert:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					strFieldName = strFieldName + FieldValues.Item1(i) + ",";
					strFieldData = strFieldData + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					strFieldName = strFieldName.Remove(strFieldName.Length - 1, 1);
					strFieldData = strFieldData.Remove(strFieldData.Length - 1, 1);
				}
				CmdMySQL.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
				break;
			}
			case InsertOrUpdate.Update:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					string text = strUpdateData;
					strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
				}
				CmdMySQL.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
				break;
			}
			}
			Cmd = CmdMySQL;
			break;
		}
		case Connection.EServerType.ISRC:
		{
			MySqlCommand CmdISRC = new MySqlCommand();
			switch (InsertOrUpdate)
			{
			case InsertOrUpdate.Insert:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					strFieldName = strFieldName + FieldValues.Item1(i) + ",";
					strFieldData = strFieldData + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdISRC.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					strFieldName = strFieldName.Remove(strFieldName.Length - 1, 1);
					strFieldData = strFieldData.Remove(strFieldData.Length - 1, 1);
				}
				CmdISRC.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
				break;
			}
			case InsertOrUpdate.Update:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					string text = strUpdateData;
					strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdISRC.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
				}
				CmdISRC.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
				break;
			}
			}
			Cmd = CmdISRC;
			break;
		}
		case Connection.EServerType.EDeposit:
		{
			MySqlCommand CmdEDeposit = new MySqlCommand();
			switch (InsertOrUpdate)
			{
			case InsertOrUpdate.Insert:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					strFieldName = strFieldName + FieldValues.Item1(i) + ",";
					strFieldData = strFieldData + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdEDeposit.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					strFieldName = strFieldName.Remove(strFieldName.Length - 1, 1);
					strFieldData = strFieldData.Remove(strFieldData.Length - 1, 1);
				}
				CmdEDeposit.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
				break;
			}
			case InsertOrUpdate.Update:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					string text = strUpdateData;
					strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					CmdEDeposit.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
				}
				CmdEDeposit.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
				break;
			}
			}
			Cmd = CmdEDeposit;
			break;
		}
		default:
			Util.RaiseMessage("Server Type doesn't recognized!");
			break;
		}
		return Cmd;
	}

	public static bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate)
	{
		return ExecInsertOrUpdate(TableName, FieldValues, InsertOrUpdate, "");
	}

	public static string ExecScalarMySQL(string SQL, string NullValue)
	{
		return ExecScalarMySQL(null, SQL, NullValue);
	}

	public static string ExecScalarMySQL(TwoArrayList Parameter, string SQL, string NullValue)
	{
		string Result = null;
		MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand cmdMySQL = new MySqlCommand();
		try
		{
			if (Parameter != null)
			{
				for (int i = 0; i <= Parameter.Count() - 1; i++)
				{
					cmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbolMySQL + Parameter.Item1(i), Parameter.Item2(i));
				}
			}
			cmdMySQL.CommandText = SQL;
			cmdMySQL.Connection = connMySQL;
			connMySQL.Open();
			object temp = cmdMySQL.ExecuteScalar();
			Result = ((temp != null && !(temp.ToString() == "")) ? temp.ToString() : NullValue);
			temp = null;
		}
		catch (MySqlException ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
		}
		finally
		{
             if (cmdMySQL!= null)
				{
					cmdMySQL.Dispose();
				}

                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}
		}
		return Result;
	}

	public static DataTable ExecDataAdapterMySQL(string SQL, TwoArrayList Parameter = null)
	{
		DataTable Result = new DataTable();
		MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand cmdMySQL = new MySqlCommand();
		try
		{
			if (Parameter != null)
			{
				for (int i = 0; i <= Parameter.Count() - 1; i++)
				{
					cmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
				}
			}
			cmdMySQL.CommandText = SQL;
			cmdMySQL.Connection = connMySQL;
			connMySQL.Open();
			MySqlDataAdapter da = new MySqlDataAdapter(cmdMySQL);
			da.Fill(Result);
			da.Dispose();
		}
		catch (MySqlException ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
		}
		finally
		{

            if (cmdMySQL != null)
            {
                cmdMySQL.Dispose();
            }

            if (connMySQL != null)
            {
                connMySQL.Close();
            }
            if (connMySQL != null)
            {
                connMySQL.Dispose();
            }

		}
		return Result;
	}

	public static bool ExecNonQueryMySQL(string SQL)
	{
		bool Result = false;
		MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand cmdMySQL = new MySqlCommand();
		try
		{
			cmdMySQL.CommandText = SQL;
			cmdMySQL.Connection = connMySQL;
			connMySQL.Open();
			cmdMySQL.ExecuteNonQuery();
			Result = true;
		}
		catch (MySqlException ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? cmdMySQL.CommandText : "") + Environment.NewLine);
		}
		finally
		{
                if (cmdMySQL!= null)
				{
					cmdMySQL.Dispose();
				}

                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}
		}
		return Result;
	}

	public static bool ExecInsertOrUpdateMySQL(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate)
	{
		return ExecInsertOrUpdateMySQL(TableName, FieldValues, InsertOrUpdate, "");
	}

	public static object GenerateSQLInsertCommandMySQL(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		object Cmd = null;
		string strFieldName = null;
		string strFieldData = null;
		string strUpdateData = null;
		MySqlCommand CmdMySQL = new MySqlCommand();
		switch (InsertOrUpdate)
		{
		case InsertOrUpdate.Insert:
		{
			int i;
			for (i = 0; i <= FieldValues.Count() - 1; i++)
			{
				strFieldName = strFieldName + FieldValues.Item1(i) + ",";
				strFieldData = strFieldData + Connection.ParameterSymbolMySQL + FieldValues.Item1(i) + ",";
				CmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbolMySQL + FieldValues.Item1(i), FieldValues.Item2(i));
			}
			if (i > 0)
			{
				strFieldName = strFieldName.Remove(strFieldName.Length - 1, 1);
				strFieldData = strFieldData.Remove(strFieldData.Length - 1, 1);
			}
			CmdMySQL.CommandText = "INSERT INTO " + TableName + "(" + strFieldName + ") VALUES (" + strFieldData + ")";
			break;
		}
		case InsertOrUpdate.Update:
		{
			int i;
			for (i = 0; i <= FieldValues.Count() - 1; i++)
			{
				string text = strUpdateData;
				strUpdateData = text + FieldValues.Item1(i) + " = " + Connection.ParameterSymbolMySQL + FieldValues.Item1(i) + ",";
				CmdMySQL.Parameters.AddWithValue(Connection.ParameterSymbolMySQL + FieldValues.Item1(i), FieldValues.Item2(i));
			}
			if (i > 0)
			{
				strUpdateData = strUpdateData.Remove(strUpdateData.Length - 1, 1);
			}
			CmdMySQL.CommandText = "UPDATE " + TableName + " SET " + strUpdateData + SQLWhere;
			break;
		}
		}
		return CmdMySQL;
	}

	public static bool ExecInsertOrUpdateMySQL(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		bool Result = false;
		MySqlConnection connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand cmdMySQL = new MySqlCommand();
		try
		{
			cmdMySQL = (MySqlCommand)GenerateSQLInsertCommandMySQL(TableName, FieldValues, InsertOrUpdate, SQLWhere);
			connMySQL.Open();
			cmdMySQL.Connection = connMySQL;
			cmdMySQL.ExecuteNonQuery();
			Result = true;
		}
		catch (MySqlException ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine);
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine);
		}
		finally
		{
               if (cmdMySQL != null)
				{
					cmdMySQL.Dispose();
				}

                if (connMySQL != null)
				{
					connMySQL.Close();
				}
				if (connMySQL != null)
				{
					connMySQL.Dispose();
				}

		}
		return Result;
	}
}
