using Dnn.Powershell.Local.Environment;
using System.IO;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsLifecycle.Install, Nouns.Dnn)]
    public class InstallDnn : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ZipFolder { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Version { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public string TargetFolder { get; set; }

        [Parameter(Mandatory = false)]
        public string Type { get; set; } = "Install";

        protected override void ProcessRecord()
        {

            var zip = CompressionController.FindDnnZip(ZipFolder, Version, Type);
            if (string.IsNullOrEmpty(zip))
            {
                throw new System.Exception(string.Format("Cannot find DNN install zip for version {0} in folder {1}", Version, ZipFolder));
            }
            DiskController.CreateIisFolder(TargetFolder);
            CompressionController.UnzipFile(Path.Combine(ZipFolder, zip), TargetFolder);
        }
    }
}
