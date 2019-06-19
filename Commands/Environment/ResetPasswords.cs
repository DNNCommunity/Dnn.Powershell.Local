using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Reset, Nouns.Passwords)]
    public class ResetPasswords : DNNCmdLet
    {
        protected override void ProcessRecord()
        {
            var script = Sql.SqlScripts.GetScript("ResetPasswords");
            Context.ExecuteNonQuery(script);
        }
    }
}
