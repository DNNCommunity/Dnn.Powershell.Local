using Dnn.Powershell.Local.Common;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Xml;

namespace Dnn.Powershell.Local.Commands.Environment
{

    /// <summary>
    /// <para type="synopsis">This sets the current environment DNN installation</para>
    /// <para type="description">The environment only has one DNN installation active at a time.
    /// The web.config is loaded and analyzed to get the connection string so future actions
    /// can be run against the database.</para>
    /// </summary>
    [Cmdlet(VerbsOther.Use, Nouns.Dnn)]
    public class UseDnn : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Path to the DNN installation. Note this needs to be 
        /// the root of the DNN installation where the web.config is found.</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public string Sitepath { get; set; }

        protected override void ProcessRecord()
        {
            var webConfigPath = System.IO.Path.Combine(Sitepath, "web.config");
            if (!System.IO.File.Exists(webConfigPath))
            {
                throw new System.Exception(string.Format("Couldn't find web.config at {0}", Sitepath));
            }
            Context.WebConfig = new XmlDocument();
            Context.WebConfig.Load(webConfigPath);

            Context.ConnectionString = Context.WebConfig.SelectSingleNode("/configuration/appSettings/add[@key='SiteSqlServer']") == null ? "" : Context.WebConfig.SelectSingleNode("/configuration/appSettings/add[@key='SiteSqlServer']").Attributes["connectionString"].Get<string>(Context.ConnectionString);
            Context.ConnectionString = Context.WebConfig.SelectSingleNode("/configuration/connectionStrings/add[@name='SiteSqlServer']") == null ? "" : Context.WebConfig.SelectSingleNode("/configuration/connectionStrings/add[@name='SiteSqlServer']").Attributes["connectionString"].Get<string>(Context.ConnectionString);
            Context.DatabaseOwner = Context.WebConfig.SelectSingleNode("/configuration/dotnetnuke/data/providers/add[@name='SqlDataProvider']").Attributes["databaseOwner"].Get<string>(Context.DatabaseOwner);
            Context.DatabaseOwner = Context.DatabaseOwner.ForceEnd(".");
            Context.ObjectQualifier = Context.WebConfig.SelectSingleNode("/configuration/dotnetnuke/data/providers/add[@name='SqlDataProvider']").Attributes["objectQualifier"].Get<string>(Context.ObjectQualifier);
            Context.ObjectQualifier = Context.ObjectQualifier.ForceEnd("_");
            Context.DatabaseName = Regex.Match(Context.ConnectionString, "(?i);Database=([^;]+);(?-i)").Groups[1].Value;
            if (string.IsNullOrEmpty(Context.DatabaseName))
                Context.DatabaseName = Regex.Match(Context.ConnectionString, "(?i);Initial Catalog=([^;]+);(?-i)").Groups[1].Value;

            object r = Context.ExecuteScalarSQL("SELECT MAX(Version) FROM (SELECT REPLICATE('0', CASE WHEN Major>9 THEN 0 ELSE 1 END)+CAST(Major AS VARCHAR(2))+'.'+REPLICATE('0', CASE WHEN Minor>9 THEN 0 ELSE 1 END)+CAST(Minor AS VARCHAR(2))+'.'+REPLICATE('0', CASE WHEN Build>9 THEN 0 ELSE 1 END)+CAST(Build AS VARCHAR(2)) AS Version FROM {databaseOwner}{objectQualifier}Version WHERE ([Name] IS NULL) OR ([Name]='DNNCORP.CE')) t");
            if (r != null)
                Context.DNNVersion = new System.Version(System.Convert.ToString(r));

            WriteContextDetails();
        }
    }
}
