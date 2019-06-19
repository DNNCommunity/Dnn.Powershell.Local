using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Rename the url DNN uses to access a site</para>
    /// <para type="description">This command will run a script to set the http alias correctly in
    /// DNN's database so a copied site can be accessed through the new url. Useful when copying
    /// live sites to local for debugging.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Rename, Nouns.Url)]
    public class RenameUrl : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Url used by the old site. Note this can be a part of the url as well.</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string OldUrl { get; set; }

        /// <summary>
        /// <para type="description">What to replace the OldUrl with</para>
        /// </summary>
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
