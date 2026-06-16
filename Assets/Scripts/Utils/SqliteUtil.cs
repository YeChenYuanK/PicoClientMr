using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

/// <summary>
/// Sqllite util. 需要导入Mono.Data.sqlite.dll
/// </summary>
public class SqliteUtil {

	public static SqliteDataReader CreateTable(string name, string[] col, string[] colType , SqliteConnection dbConnection)
	{

		if (col.Length != colType.Length)
		{
			throw new SqliteException("columns.Length != colType.Length");
		}

		string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];

		for (int i = 1; i < col.Length; ++i)
		{

			query += ", " + col[i] + " " + colType[i];

		}

		query += ")";
		Debug.Log (query);
		return ExecuteQuery(query,dbConnection);

	}

	public static SqliteDataReader InsertInto(string tableName, string[] values, SqliteConnection dbConnection)
	{
		string query = "INSERT INTO " + tableName + " VALUES ('" + values[0] + "'";

		for (int i = 1; i < values.Length; ++i)
		{
			query += ", '" + values[i] + "'";
		}

		query += ")";
		Debug.Log (query);
		return ExecuteQuery(query , dbConnection);

	}

	public static bool TableAlready(string tableName , SqliteConnection dbConnection)
	{
		SqliteCommand dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = "select count(*) from sqlite_master where type='table' and name='" + tableName +"'";
		int count = int.Parse(dbCommand.ExecuteScalar ().ToString());

		Debug.Log (tableName + " Count:" + count );
		if (count > 0) {
			return true;
		} else {
			return false;
		}
	}

	public static SqliteDataReader ExecuteQuery(string sqlQuery , SqliteConnection dbConnection)
	{
		SqliteCommand dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = sqlQuery;
		SqliteDataReader reader = dbCommand.ExecuteReader();
		return reader;

	}

	public static void DropTable(string tableName , SqliteConnection dbConnection)
	{

		SqliteCommand dbCommand = dbConnection.CreateCommand();

		dbCommand.CommandText = "DROP TABLE '" + tableName + "'";
		int result = dbCommand.ExecuteNonQuery ();
		//		SqliteDataReader reader = dbCommand.ExecuteReader();
		Debug.Log("DROP TABLE RESULT:" + result);
	}


	public static string ToSqlType(string dataType)
	{
		string sqlType = "";
		switch(dataType.ToLower())
		{
		case "int":
		case "byte":
        case "bool":
		case "short":
			sqlType = "INT";
			break;
		case "long":
			sqlType = "BIGINT";
			break;
		case "string":
        case "array":
            sqlType = "TEXT";
			break;
		case "float":
			sqlType = "FLOAT";
			break;
		default:
			Debug.LogError ("未定义数据类型");
			break;
		}
		return sqlType;
	}

}
