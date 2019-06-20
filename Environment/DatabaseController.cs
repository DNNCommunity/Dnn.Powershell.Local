using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Dnn.Powershell.Local.Environment
{
    public class DatabaseController
    {
        public static void CreateDatabase(string connectionString, string dbName, string dbLogin, string dbPath)
        {
            var script = Sql.SqlScripts.GetScript("DbCreate");
            script = script.Replace("{DBName}", dbName);
            script = script.Replace("{DBLogin}", dbLogin);
            script = script.Replace("{DBPath}", dbPath);
            RunScript(connectionString, script);
        }

        public static void DropDatabase(string connectionString, string dbName)
        {
            var script = Sql.SqlScripts.GetScript("DbDrop");
            script = script.Replace("{DBName}", dbName);
            RunScript(connectionString, script);
        }

        public static void RestoreDatabase(string connectionString, string bakFile, string dbPath, string dbName, string dbLogin)
        {
            var script = Sql.SqlScripts.GetScript("DbRestore");
            script = script.Replace("{DBName}", dbName);
            script = script.Replace("{DBLogin}", dbLogin);
            script = script.Replace("{DBPath}", dbPath);
            script = script.Replace("{BakFile}", bakFile);
            RunScript(connectionString, script);
        }

        public static void RunScript(string connectionString, string script)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if ((int)connection.State != (int)ConnectionState.Open)
                    connection.Open();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandTimeout = 10;
                foreach (var segment in Regex.Split(script, @"GO\r\n"))
                {
                    if (segment.Trim() != "")
                    {
                        cmd.CommandText = segment;
                        cmd.ExecuteNonQuery();
                    }
                }
                cmd.Dispose();
            }
        }
    }
}
