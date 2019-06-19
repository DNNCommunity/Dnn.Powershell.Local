using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Adds a database</para>
    /// <para type="description">Adds a database to SQL server</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.Database)]
    public class AddDatabase : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">SA connection string</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// <para type="description">Name of database to create</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">SQL login to use as db_owner</para>
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public string Login { get; set; }

        /// <summary>
        /// <para type="description">Path to save the database files</para>
        /// </summary>
        [Parameter(Position = 3, Mandatory = true)]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseController.CreateDatabase(ConnectionString, Name, Login, Path);
        }
    }
}
