using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Rename, Nouns.Url)]
    public class RenameUrl : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string OldUrl { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string NewUrl { get; set; }

        protected override void ProcessRecord()
        {
            var script = Sql.SqlScripts.GetScript("RenameUrl");
            script = script.Replace("{OldUrl}", OldUrl);
            script = script.Replace("{NewUrl}", NewUrl);
            Context.ExecuteNonQuery(script);
        }
    }
}
