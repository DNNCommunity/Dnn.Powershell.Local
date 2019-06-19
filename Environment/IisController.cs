using Microsoft.Web.Administration;

namespace Dnn.Powershell.Local.Environment
{
    public class IisController
    {
        public static void CreateSite(string path, string nameOrUrl, string parentSite)
        {
            var iisManager = new ServerManager();
            if (string.IsNullOrEmpty(parentSite))
            {
                var site = iisManager.Sites.Add(nameOrUrl, path, 80);
                site.Bindings.Add(nameOrUrl, "http");
                site.ServerAutoStart = true;
                var newAppPool = iisManager.ApplicationPools.Add(nameOrUrl);
                newAppPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                newAppPool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                site.Applications[0].ApplicationPoolName = nameOrUrl;
            }
            else
            {
                iisManager.Sites[parentSite].Applications.Add(nameOrUrl, path);
            }
            iisManager.CommitChanges();
        }
    }
}
