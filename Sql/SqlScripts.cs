using Dnn.Powershell.Local.Common;
using System;

namespace Dnn.Powershell.Local.Sql
{
    public static class SqlScripts
    {
        public static string GetScript(string scriptName, Version version)
        {
            switch (scriptName.ToLower())
            {
                case "addusertorole":
                    {
                        return Globals.GetResource("AddUserToRole.sql");
                    }

                case "setrolegroup":
                    {
                        if (version < new Version("05.00.00"))
                            return Globals.GetResource("SetRoleGroup.sql");
                        else
                            return Globals.GetResource("SetRoleGroup_050000.sql");
                    }
            }
            return "";
        }
    }
}
