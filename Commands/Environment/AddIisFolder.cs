using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Add, Nouns.IisFolder)]
    public class AddIisFolder : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Sitepath { get; set; }

        protected override void ProcessRecord()
        {
            DiskController.CreateIisFolder(Sitepath);
        }
    }
}
