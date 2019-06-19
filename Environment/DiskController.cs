using System.IO;
using System.Security.AccessControl;

namespace Dnn.Powershell.Local.Environment
{
    public class DiskController
    {
        public static void CreateIisFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                DirectorySecurity security = info.GetAccessControl();
                security.AddAccessRule(new FileSystemAccessRule("NT AUTHORITY\\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                security.AddAccessRule(new FileSystemAccessRule("NT AUTHORITY\\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                info.SetAccessControl(security);
            }
        }
    }
}
