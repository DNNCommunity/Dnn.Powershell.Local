using System.Xml;

namespace Dnn.Powershell.Local.Dnn
{
    public class WebConfig : XmlDocument
    {
        private string path { get; set; }

        public WebConfig(string webConfigPath)
        {
            path = webConfigPath;
            Load(webConfigPath);
        }

        public void Save()
        {
            Save(path);
        }

        public void SetConnectionString(string connectionString)
        {
            var node = DocumentElement.SelectSingleNode("connectionStrings");
            if (node != null)
            {
                node.SelectSingleNode("add[@name=\"SiteSqlServer\"]").Attributes["connectionString"].InnerText = connectionString;
            }
        }

        public void SetAppSetting(string settingName, string settingValue)
        {
            var node = DocumentElement.SelectSingleNode("appSettings");
            if (node != null)
            {
                var snode = node.SelectSingleNode(string.Format("add[@key=\"{0}\"]", settingName));
                if (snode != null)
                {
                    snode.Attributes["value"].InnerText = settingValue;
                }
                else
                {
                    // add it
                }
            }
        }

        public void SetProviderProperty(string providerType, string providerName, string propertyName, string propertyValue)
        {
            var dnnNode = DocumentElement.SelectSingleNode("dotnetnuke");
            if (dnnNode != null)
            {
                var provNode = dnnNode.SelectSingleNode(string.Format("{0}/providers/add[@name=\"{1}\"]", providerType, providerName));
                if (provNode != null)
                {
                    provNode.Attributes[propertyName].InnerText = propertyValue;
                }
            }
        }

        public void SetDbParameters(string dbOwner, string objectQualifier)
        {
            SetProviderProperty("data", "SqlDataProvider", "databaseOwner", dbOwner);
            SetProviderProperty("data", "SqlDataProvider", "objectQualifier", objectQualifier);
        }
    }
}
