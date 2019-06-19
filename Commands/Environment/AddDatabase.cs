using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Add, Nouns.Database)]
    public class AddDatabase : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ConnectionString { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public string Login { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            DatabaseController.CreateDatabase(ConnectionString, Name, Login, Path);
        }
    }
}
