using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Resets all DNN passwords</para>
    /// <para type="description">Runs a script that will reset all passwords to "password" for an
    /// installation. This is useful when you have a backup installation which is a copy
    /// of a live installation and you wish to access other user accounts.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Reset, Nouns.Passwords)]
    public class ResetPasswords : DNNCmdLet
    {
        protected override void ProcessRecord()
        {
            var script = Sql.SqlScripts.GetScript("ResetPasswords");
            Context.RunScript(script);
        }
    }
}
