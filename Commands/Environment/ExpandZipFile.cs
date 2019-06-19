using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsData.Expand, Nouns.ZipFile)]
    public class ExpandZipFile : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ZipFilePath { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string TargetDir { get; set; }

        protected override void ProcessRecord()
        {
            CompressionController.UnzipFile(ZipFilePath, TargetDir);
        }
    }
}
