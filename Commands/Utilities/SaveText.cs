using System.IO;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Utilities
{
    /// <summary>
    /// <para type="synopsis">Save test to a file</para>
    /// <para type="description">This will save a string to a file, optionally overwriting it</para>
    /// </summary>
    [Cmdlet(VerbsData.Save, Nouns.Text)]
    public class SaveText : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Path to file to write to</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Filepath { get; set; }

        /// <summary>
        /// <para type="description">Text to add to the file</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Text { get; set; }

        /// <summary>
        /// <para type="description">Whether to append the text to the end of the file or not</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool Append { get; set; } = true;

        protected override void ProcessRecord()
        {
            if (!Directory.Exists(Path.GetDirectoryName(Filepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filepath));
            }
            using (StreamWriter sw = new StreamWriter(Filepath, Append))
            {
                sw.Write(Text);
                sw.Flush();
            }
        }
    }
}
