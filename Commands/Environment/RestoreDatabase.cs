using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Restores a SQL database</para>
    /// <para type="description">This command will restore a database from a backup (.bak) file.</para>
    /// </summary>
    [Cmdlet(VerbsData.Restore, Nouns.Database)]
    public class RestoreDatabase : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">SA connection string for the SQL server</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// <para type="description">Name of the database to restore to</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">User login to use as db_owner for the database. This login must exists
        /// on the target SQL server.</para>
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public string Login { get; set; }

        /// <summary>
        /// <para type="description">Path to save the db files to</para>
        /// </summary>
        [Parameter(Position = 3, Mandatory = true)]
        public string Path { get; set; }

        /// <summary>
        /// <para type="description">Full path to the backup file</para>
        /// </summary>
        [Parameter(Position = 4, Mandatory = true)]
        public string BakFile { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseController.RestoreDatabase(ConnectionString, BakFile, Path, Name, Login);
        }
    }
}
