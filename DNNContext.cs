using System.Data;
using System.Collections;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Dnn.Powershell.Local
{
    public sealed class DNNContext
    {
        public static DNNContext Instance { get; } = new DNNContext();

        public string ConnectionString { get; set; } = "";
        public string ObjectQualifier { get; set; } = "";
        public string DatabaseOwner { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public Version DNNVersion { get; set; }
        public System.Xml.XmlDocument WebConfig { get; set; } = null;

        public Hashtable Preferences { get; set; }

        private SqlConnection _connection = null;
        public SqlConnection Connection
        {
            get
            {
                if (string.IsNullOrEmpty(ConnectionString))
                    return null;
                if (_connection == null)
                    _connection = new SqlConnection(ConnectionString);
                if ((int)_connection.State != (int)ConnectionState.Open)
                    _connection.Open();
                return _connection;
            }
        }

        public SqlCommand GetCommand()
        {
            var cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandTimeout = 10;
            return cmd;
        }

        public SqlCommand GetCommand(string commandText)
        {
            var cmd = GetCommand();
            cmd.CommandText = ReplaceSqlPlaceholders(commandText);
            return cmd;
        }

        public string ReplaceSqlPlaceholders(string sql)
        {
            sql = sql.Replace("{DBName}", DatabaseName);
            sql = sql.Replace("{databaseOwner}", DatabaseOwner);
            sql = sql.Replace("{objectQualifier}", ObjectQualifier);
            return sql;
        }

        public object ExecuteScalar(string sql)
        {
            if (Connection == null)
                return null;
            SqlCommand cmd = GetCommand(sql);
            return cmd.ExecuteScalar();
        }

        public void RunScript(string sql)
        {
            if (Connection == null)
                return;
            foreach (var segment in Regex.Split(sql, @"GO\r\n"))
            {
                if (segment.Trim() != "")
                {
                    var cmd = GetCommand(segment);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
