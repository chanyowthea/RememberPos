//using UnityEngine;
//using System;
//using System.Data;
//using System.Collections;
//using MySql.Data.MySqlClient;
//using MySql.Data;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;

//public class SqlAccess
//{
//	public static MySqlConnection dbConnection;
//	//如果只是在本地的话，写localhost就可以。
//	// static string host = "localhost";
//	//如果是局域网，那么写上本机的局域网IP

//	// 192.168.0.100
//	// 39.106.31.37
//	public static string host = "192.168.0.108"; // "169.254.245.78"
//	static string id = "chanyow";
//	static string pwd = "1111";
//	static string database = "rememberPos";

////	public static string host = "39.106.31.37";
////	static string id = "yow2";
////	static string pwd = "1111";
////	static string database = "new";

//	public SqlAccess(string h)
//	{
//		OpenSql();
//	}

//	public SqlAccess()
//	{
//		string ip = Network.player.ipAddress;
//		//string ip = SqlUtil.GetLocalIP(); 
//		Debug.LogError("ip=" + ip);
//		//if (ip != host)
//		//{
//		//	Debug.LogError(string.Format("ip is {0}, while host is {1}", ip, host)); 
//		//	host = ip; 
//		//}

//		host = "192.168.1.4";
//		id = "kbe"; 
//		database = "kbe"; 
//		OpenSql(); 
//	}

//	public static void OpenSql()
//	{
//		try
//		{
//			string connectionString = string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};", host, database, id, pwd, "3306");
//			dbConnection = new MySqlConnection(connectionString);
//			dbConnection.Open(); 
//		}
//		catch (Exception e)
//		{
//			Debug.LogError("服务器连接失败，请重新检查是否打开MySql服务。" + e.Message.ToString());
//			LogUtil.Popup("服务器连接失败，请重新检查是否打开MySql服务。" + e.Message.ToString()); 
//		}
//	}

//	public void CreateTable(string tableName, string[] col, string[] colType, bool isAutoID = true)
//	{
//		Debug.LogError("host=" + host + ", id=" + id + ", database=" + database); 
//		if (col.Length != colType.Length || col.Length == 0 || colType.Length == 0)
//		{
//			Debug.LogError("columns.Length != colType.Length");
//			return; 
//		}

//		string format = "CREATE TABLE IF NOT EXISTS {0} ({1})";
//		string temp = ""; 
//		for (int i = 0; i < col.Length; ++i)
//		{
//			temp += (i == 0 ? "" : ", ") + col[i] + " " + colType[i]; 
//			if (i == 0 && isAutoID)
//			{
//				temp += " NOT NULL AUTO_INCREMENT ";
//			}
//		}
//		if (isAutoID)
//		{
//			temp += string.Format(", PRIMARY KEY ({0})", col[0]);
//		}
//		ExecuteNonQuery(string.Format(format, tableName, temp));
//	}

//	/// <param name="selectkey">限制条件字段</param>
//	/// <param name="selectvalue">限制条件值</param>
//	public void UpdateInto(string tableName, MySqlParameter condition, params MySqlParameter[] args)
//	{
//		// 如果没有信息就插入信息
//		var count = GetCount(tableName, condition); 
//		if (count == 0)
//		{
//			InsertInto(tableName, args); 
//			return; 
//		}

//		string format = "UPDATE {0} SET {1} WHERE {2}"; 

//		// set values 
//		string temp0 = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp0 += args[i].ParameterName.TrimStart('?') + "=" + args[i].ParameterName + ((i != args.Length - 1) ? ", " : "");
//		}

//		// condition
//		string temp1 = condition.ParameterName.TrimStart('?') + "=" + "'" + condition.Value + "'"; 
//		ExecuteNonQuery(string.Format(format, tableName, temp0, temp1), args); 
//	}


//	public void DeleteInfo(string tableName, params MySqlParameter[] args)
//	{
//		if (args == null || args.Length <= 0)
//		{
//			return;
//		}
//		string temp = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp += args[i].ParameterName.TrimStart('?') + "=" + args[i].ParameterName + ((i != args.Length - 1) ? " or " : "");
//		}

//		string format = (args == null || args.Length == 0) ? "DELETE FROM {0}" : "DELETE FROM {0} WHERE {1}"; 
//		ExecuteNonQuery(string.Format(format, tableName, temp), args);
//	}

//	public void Close()
//	{
//		if (dbConnection != null)
//		{
//			dbConnection.Close();
//			dbConnection.Dispose();
//			dbConnection = null;
//		}
//	}

//	public void DeleteTable(string tableName)
//	{
//		ExecuteNonQuery("DROP TABLE " + tableName);
//	}

//	// 插入数据
//	public void InsertInto(string tableName, params MySqlParameter[] args)
//	{
//		//		string s = "SELECT COUNT(*) FROM " + "Talent"; 

//		if (args == null || args.Length <= 0)
//		{
//			return;
//		}
//		string format = "INSERT INTO {0} ({1}) VALUES ({2})";
//		// columns name
//		string temp0 = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp0 += ", " + args[i].ParameterName.TrimStart('?');
//		}
//		temp0 = temp0.TrimStart(',', ' '); 
//		// value references 
//		string temp1 = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp1 += ", " + args[i].ParameterName;
//		}
//		temp1 = temp1.TrimStart(',', ' '); 
//		ExecuteNonQuery(string.Format(format, tableName, temp0, temp1), args); 
//	}

//	/// <summary>
//	/// 根据条件读取数据，并且返回对应条数的数据组，前面的List是数据行数，后面的List是列名对应的数据
//	/// </summary>
//	/// <param name="columnNames">需要读取的字段</param>
//	/// <param name="args">读取信息的限定条件</param>
//	public List<List<object>> ReaderInfo(string tableName, string[] columnNames, params MySqlParameter[] args)
//	{
//		if (columnNames == null || columnNames.Length == 0)
//		{
//			return null; 
//		}

//		// columns name
//		string temp0 = ""; 
//		for (int i = 0; i < columnNames.Length; ++i)
//		{
//			temp0 += ", " + columnNames[i];
//		}
//		temp0 = temp0.TrimStart(',', ' '); 

//		// conditions
//		string temp1 = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp1 += args[i].ParameterName.TrimStart('?') + "=" + args[i].ParameterName + ((i != args.Length - 1) ? " AND " : "");
//		}

//		string format = (args == null || args.Length == 0) ? "SELECT {0} FROM {1}" : "SELECT {0} FROM {1} WHERE {2}"; 
//		return ExecuteReader(string.Format(format, temp0, tableName, temp1), args);
//	}

//	public List<List<object>> ReadAllInfo(string tableName, params MySqlParameter[] args)
//	{
//		// conditions
//		string temp = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp += args[i].ParameterName.TrimStart('?') + "=" + args[i].ParameterName + ((i != args.Length - 1) ? " AND " : "");
//		}

//		string format = (args == null || args.Length == 0) ? "SELECT * FROM {0}" : "SELECT * FROM {0} WHERE {1}"; 
//		return ExecuteReader(string.Format(format, tableName, temp), args);
//	}

//	// 获取表中符合条件的记录数
//	public int GetCount(string tableName, params MySqlParameter[] args)
//	{
//		// conditions
//		string temp = ""; 
//		for (int i = 0; i < args.Length; ++i)
//		{
//			temp += args[i].ParameterName.TrimStart('?') + "=" + args[i].ParameterName + ((i != args.Length - 1) ? " AND " : "");
//		}

//		string format = (args == null || args.Length == 0) ? "SELECT COUNT(*) FROM {0}" : "SELECT COUNT(*) FROM {0} WHERE {1}"; 
//		var list = ExecuteReader(string.Format(format, tableName, temp), args); 

//		list.Print(); 
//		if (list == null || list.Count == 0 || list[0].Count == 0 || (list[0][0]).ToString() == "0")
//		{
//			return 0; 
//		}
//		return list.Count; 
//	}

//	public List<List<object>> ExecuteReader(string sql, params MySqlParameter[] args)
//	{
//		var result = new List<List<object>>(); 
//		using (var command = new MySqlCommand(sql, dbConnection))
//		{
//			foreach (var param in args)
//				command.Parameters.Add(param);
//			using (var reader = command.ExecuteReader())
//			{
//				while (reader.Read())
//				{
//					// 参数个数
//					var buffer = new object[reader.FieldCount];
//					// 读取所有值
//					reader.GetValues(buffer);
//					result.Add(buffer.ToList());
//				}
//			}
//		}
//		Debug.Log(sql); 
//		return result;
//	}

//	public void ExecuteNonQuery(string sql, params MySqlParameter[] args)
//	{
//		Debug.Log(sql); 
//		using (var command = new MySqlCommand(sql, dbConnection))
//		{
//			foreach (var param in args)
//				command.Parameters.Add(param);
//			command.ExecuteNonQuery();
//		}
//	}
//}
