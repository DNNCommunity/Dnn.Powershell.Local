using Dnn.Powershell.Local.Dnn;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Roles
{
    [Cmdlet(VerbsCommon.Add, Nouns.Roles)]
    public class AddRoles : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int NrRoles { get; set; }

        [Parameter()]
        public string RoleGroupName { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose("Starting Add Roles Script");

            int RoleGroupID = RoleController.GetOrCreateRoleGroupIdByName(Context, PortalId, RoleGroupName);
            var loopTo = NrRoles;
            for (int i = 1; i <= loopTo; i++)
            {
                string roleName = Common.Random.GetRandomRoleName();
                int RoleID = RoleController.GetRoleIdByName(Context, PortalId, roleName);
                if (RoleID == -1)
                {
                    RoleID = RoleController.AddRole(Context, PortalId, RoleGroupID, roleName);
                    WriteVerbose(string.Format("Created role {0}", roleName));
                }
                else
                    WriteVerbose(string.Format("Role {0} exists already", roleName));
            }
        }
    }
}
