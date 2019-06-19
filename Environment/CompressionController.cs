using Dnn.Powershell.Local.Common;
using System.IO;
using System.IO.Compression;

namespace Dnn.Powershell.Local.Environment
{
    public class CompressionController
    {
        public static void UnzipFile(string zipFilePath, string targetDir)
        {
            targetDir = targetDir.ForceEnd(@"\");
            using (ZipArchive objZipInputStream = new ZipArchive(File.OpenRead(zipFilePath), ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry objZipEntry in objZipInputStream.Entries)
                {
                    var strFileName = objZipEntry.FullName.Replace("/", @"\");
                    if (strFileName != "")
                    {
                        strFileName = targetDir + strFileName;
                        if (File.Exists(strFileName))
                        {
                            File.SetAttributes(strFileName, FileAttributes.Normal);
                            File.Delete(strFileName);
                        }
                        if (!Directory.Exists(Path.GetDirectoryName(strFileName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(strFileName));
                        objZipEntry.ExtractToFile(strFileName);
                    }
                }
            }
        }

        public static string FindDnnZip(string ZipFolder, string Version, string Type)
        {
            var foundZip = "";
            Version = Version.ToLower();

            if (((Version ?? "") == "latest") | ((Version ?? "") == "nightly"))
            {
                foundZip = "DNN_Platform_Install.zip";
                if (!File.Exists(string.Format(@"{0}\{1}", ZipFolder, foundZip)))
                {
                    return "";
                }
                return "";
            }

            var ver = Version.Split('.');
            if (ver.Length < 1)
            {
                return "";
            }

            Type = Type.ToUpper();

            var shortVersion = "";
            var longVersion = "";
            foreach (var v in ver)
            {
                int iv = int.Parse(v);
                shortVersion += iv.ToString();
                shortVersion += ".";
                if (iv < 100)
                    longVersion += iv.ToString("00");
                else
                    longVersion += iv.ToString();
                longVersion += ".";
            }
            shortVersion = shortVersion.TrimEnd('.');
            longVersion = longVersion.TrimEnd('.');

            var zipEnd = string.Format("_{0}.ZIP", Type);
            var d = new DirectoryInfo(ZipFolder);
            foreach (var f in d.GetFiles())
            {
                var fn = f.Name.ToUpper();
                if (fn.EndsWith(zipEnd))
                {
                    if (fn.StartsWith(string.Format("DNN_PLATFORM_{0}", shortVersion)))
                    {
                        foundZip = f.Name;
                        break;
                    }
                    if (fn.StartsWith(string.Format("DNN_PLATFORM_{0}", longVersion)))
                    {
                        foundZip = f.Name;
                        break;
                    }
                    if (fn.StartsWith(string.Format("DOTNETNUKE_COMMUNITY_{0}", shortVersion)))
                    {
                        foundZip = f.Name;
                        break;
                    }
                    if (fn.StartsWith(string.Format("DOTNETNUKE_COMMUNITY_{0}", longVersion)))
                    {
                        foundZip = f.Name;
                        break;
                    }
                    if (fn.StartsWith(string.Format("DOTNETNUKE_{0}", shortVersion)))
                    {
                        foundZip = f.Name;
                        break;
                    }
                    if (fn.StartsWith(string.Format("DOTNETNUKE_{0}", longVersion)))
                    {
                        foundZip = f.Name;
                        break;
                    }
                }
            }
            return foundZip;
        }
    }
}
