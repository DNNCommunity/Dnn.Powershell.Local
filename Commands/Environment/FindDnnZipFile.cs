using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Find, Nouns.DnnZipFile)]
    public class FindDnnZipFile : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ZipFolder { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Version { get; set; }

        [Parameter(Position = 2, Mandatory = false)]
        public string Type { get; set; } = "Install";

        protected override void ProcessRecord()
        {
            var zip = CompressionController.FindDnnZip(ZipFolder, Version, Type);
            WriteObject(zip);
        }
    }
}
