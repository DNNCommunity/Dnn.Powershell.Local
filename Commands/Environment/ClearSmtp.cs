using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Clear SMTP settings in DNN</para>
    /// <para type="description">This command runs a script to clear all SMTP settings in DNN. This is useful
    /// after copying a live site to local and avoid any emails from being sent while testing.</para>
    /// </summary>
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
