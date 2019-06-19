using System.IO;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Utilities
{
    [Cmdlet(VerbsData.Save, Nouns.Text)]
    public class SaveText : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Filepath { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public bool Append { get; set; } = true;

        protected override void ProcessRecord()
        {
            using (StreamWriter sw = new StreamWriter(Filepath, Append))
            {
                sw.Write(Text);
                sw.Flush();
            }
        }
    }
}
