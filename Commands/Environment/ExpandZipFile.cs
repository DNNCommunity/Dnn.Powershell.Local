using Dnn.Powershell.Local.Environment;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Unzips a file to a folder</para>
    /// <para type="description">Takes a zip file and extracts all contents to the specified directory</para>
    /// </summary>
    [Cmdlet(VerbsData.Expand, Nouns.ZipFile)]
    public class ExpandZipFile : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Full name (i.e. including path) of zip file</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string ZipFilePath { get; set; }

        /// <summary>
        /// <para type="description">Directory to extract the contents of the zip file to</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string TargetDir { get; set; }

        protected override void ProcessRecord()
        {
            CompressionController.UnzipFile(ZipFilePath, TargetDir);
        }
    }
}
