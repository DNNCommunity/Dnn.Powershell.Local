using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Drops a database in SQL server</para>
    /// <para type="description">Runs a DROP script to delete a database from SQL server</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, Nouns.Database)]
    public class RemoveDatabase : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">SA connection string for server</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// <para type="description">Name of database to drop</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseController.DropDatabase(ConnectionString, Name);
        }
    }
}
