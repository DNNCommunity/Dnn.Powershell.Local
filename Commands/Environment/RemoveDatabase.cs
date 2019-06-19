using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Remove, Nouns.Database)]
    public class RemoveDatabase : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseController.DropDatabase(ConnectionString, Name);
        }
    }
}
