using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Clear, Nouns.Smtp)]
    public class ClearSmtp : DNNCmdLet
    {
        protected override void ProcessRecord()
        {
            var script = Sql.SqlScripts.GetScript("ClearSmtp");
            Context.ExecuteNonQuery(script);
        }
    }
}
