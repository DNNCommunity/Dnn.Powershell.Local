using Dnn.Powershell.Local.Dnn;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Roles
{
    [Cmdlet(VerbsCommon.Add, Nouns.Role)]
    public class AddRole : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        [Parameter(Position = 1)]
        public string Rolename { get; set; }

        [Parameter()]
        public string RoleGroupName { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(Rolename))
                Rolename = Common.Random.GetRandomRoleName();
            WriteVerbose(string.Format("Creating Role {0}", Rolename));

            int RoleID = RoleController.GetRoleIdByName(Context, PortalId, Rolename);
            int RoleGroupID = RoleController.GetOrCreateRoleGroupIdByName(Context, PortalId, RoleGroupName);

            if (RoleID == -1)
            {
                RoleID = RoleController.AddRole(Context, PortalId, RoleGroupID, Rolename);
                WriteVerbose(string.Format("Created role {0}", Rolename));
            }
            else
                WriteVerbose(string.Format("Role {0} exists already", Rolename));

            WriteObject(RoleID);
        }
    }
}
