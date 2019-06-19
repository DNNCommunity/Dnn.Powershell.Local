using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Add, Nouns.IisSite)]
    public class AddIisSite : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Sitepath { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Url { get; set; }

        [Parameter(Position = 2, Mandatory = false)]
        public string ParentSite { get; set; } = "";

        protected override void ProcessRecord()
        {
            IisController.CreateSite(Sitepath, Url, ParentSite);
        }
    }
}
