using System;
using System.IO;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    /// <summary>
    /// <para type="synopsis">Change the web.config of a DNN site</para>
    /// <para type="description">This will allow you to set various parameters on
    /// the web.config file of a DNN installation.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, Nouns.WebConfig)]
    public class SetWebconfig : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Path to DNN installation. This must contain the web.config.</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Sitepath { get; set; }

        /// <summary>
        /// <para type="description">If specified the connection string will be replaced with this</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// <para type="description">Set dbOwner (default "dbo")</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string DbOwner { get; set; } = "dbo";

        /// <summary>
        /// <para type="description">Set the objectQualifier (default empty)</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string ObjectQualifier { get; set; } = "";

        /// <summary>
        /// <para type="description">Set UseInstallWizard. If true DNN will use the install wizard
        /// to install. If false then the site will install as a script without user intervention.</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool UseInstallWizard { get; set; } = true;

        protected override void ProcessRecord()
        {
            var configFile = Path.Combine(Sitepath, "web.config");
            if (!File.Exists(configFile))
            {
                throw new Exception("Couldn't find a web.config");
            }
            var config = new Dnn.WebConfig(configFile);
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                config.SetConnectionString(ConnectionString);
            }
            config.SetDbParameters(DbOwner, ObjectQualifier);
            config.SetAppSetting("UseInstallWizard", UseInstallWizard.ToString().ToLower());
            config.Save();
        }
    }
}
