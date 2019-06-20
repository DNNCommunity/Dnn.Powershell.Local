using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Dnn.Powershell.Local.Environment
{
    public class HostsFile
    {
        private List<string> CommentLines { get; set; } = new List<string>();
        private SortedDictionary<string, HostEntry> Entries { get; set; } = new SortedDictionary<string, HostEntry>();

        public HostsFile()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            var contents = Common.Globals.ReadFile(path);
            using (var reader = new StringReader(contents))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().StartsWith("#"))
                    {
                        CommentLines.Add(line);
                    }
                    else
                    {
                        var m = Regex.Match(line, @"\s*(\d+\.\d+\.\d+\.\d+)\s+([^\s]+)(\s+(#.*))?");
                        if (m.Success)
                        {
                            Entries[m.Groups[2].Value.ToLower()] = new HostEntry()
                            {
                                IpAddress = m.Groups[1].Value,
                                HostName = m.Groups[2].Value.ToLower(),
                                Comment = m.Groups[4].Success ? m.Groups[4].Value : ""
                            };
                        }
                    }
                }
            }
        }

        public void Save()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
            var maxLength = 0;
            foreach (var e in Entries.Values)
            {
                if (maxLength < e.HostName.Length) maxLength = e.HostName.Length;
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var c in CommentLines)
                {
                    sw.WriteLine(c);
                }
                sw.WriteLine();
                foreach (var e in Entries.Keys)
                {
                    var entry = Entries[e];
                    var spaces = new string(' ', 20 - entry.IpAddress.Length);
                    if (string.IsNullOrEmpty(entry.Comment))
                    {
                        sw.WriteLine(string.Format("{0}{1}{2}", entry.IpAddress, spaces, entry.HostName));
                    }
                    else
                    {
                        var commentSpaces = new string(' ', maxLength + 4 - entry.IpAddress.Length);
                        sw.WriteLine(string.Format("{0}{1}{2}{3}{4}", entry.IpAddress, spaces, entry.HostName, commentSpaces, entry.Comment));
                    }
                }
                sw.Flush();
            }
        }

        public void AddHost(string hostName, string ipAddress, string comment)
        {
            Entries[hostName.ToLower()] = new HostEntry()
            {
                IpAddress = ipAddress,
                HostName = hostName.ToLower(),
                Comment = string.IsNullOrEmpty(comment) ? "" : "# " + comment
            };
        }

        public void RemoveHost(string hostName)
        {
            hostName = hostName.ToLower();
            if (Entries.ContainsKey(hostName))
            {
                Entries.Remove(hostName);
            }
        }

        private class HostEntry
        {
            public string IpAddress { get; set; }
            public string HostName { get; set; }
            public string Comment { get; set; }
        }

    }
}
