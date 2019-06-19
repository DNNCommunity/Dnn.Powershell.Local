using Dnn.Powershell.Local.Environment;
using System.IO;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Installs DNN to a specified directory</para>
    /// <para type="description">This command finds the right zip file and extracts it
    /// to the specified directory on disk.</para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Install, Nouns.Dnn)]
    public class InstallDnn : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Folder with DNN zip files</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string ZipFolder { get; set; }

        /// <summary>
        /// <para type="description">DNN version to install (can be shorthand like "9.3.2" or long form "09.03.02")</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Version { get; set; }

        /// <summary>
        /// <para type="description">Directory to extract contents of the zip file to</para>
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public string TargetFolder { get; set; }

        /// <summary>
        /// <para type="description">Zip file type (by default "Install") such as "Upgrade" or "Symbols"</para>
        /// </summary>
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
