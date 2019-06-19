using System;
using System.IO;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Environment
{
    [Cmdlet(VerbsCommon.Set, Nouns.WebConfig)]
    public class SetWebconfig : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Sitepath { get; set; }

        [Parameter(Mandatory = false)]
        public string ConnectionString { get; set; }

        [Parameter(Mandatory = false)]
        public string DbOwner { get; set; } = "dbo";

        [Parameter(Mandatory = false)]
        public string ObjectQualifier { get; set; } = "";

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
