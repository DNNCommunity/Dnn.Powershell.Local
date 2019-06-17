using Dnn.Powershell.Local.Dnn;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Roles
{
    /// <summary>
    /// <para type="synopsis">Add a set of roles to a portal</para>
    /// <para type="description">This will use randomly generated names for the roles</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.Roles)]
    public class AddRoles : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Portal ID for the portal to add the roles to</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        /// <summary>
        /// <para type="description">The number of roles to add</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public int NrRoles { get; set; }

        /// <summary>
        /// <para type="description">If specified the roles will be added to this role group.
        /// If not then they will be added as generic portal roles.</para>
        /// </summary>
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
