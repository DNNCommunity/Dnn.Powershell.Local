using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsData.Restore, Nouns.Database)]
    public class RestoreDatabase : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public string Login { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Position = 4, Mandatory = true)]
        public string BakFile { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseController.RestoreDatabase(ConnectionString, BakFile, Path, Name, Login);
        }
    }
}
