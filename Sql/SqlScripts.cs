using Dnn.Powershell.Local.Common;
using System;
using System.Text.RegularExpressions;

namespace Dnn.Powershell.Local.Sql
{
    public static class SqlScripts
    {
        public static string GetScript(string scriptName, Version version)
        {
            switch (scriptName.ToLower())
            {
                case "setrolegroup":
                    {
                        if (version < new Version("05.00.00"))
                            return GetScript("SetRoleGroup");
                        else
                            return GetScript("SetRoleGroup_050000");
                    }
            }
            return GetScript(scriptName);
        }
        public static string GetScript(string scriptName)
        {
            return Regex.Replace(Globals.GetResource(string.Format("Sql.Scripts.{0}.sql", scriptName)), "\r\n", System.Environment.NewLine);
        }
    }
}
