using Dnn.Powershell.Local.Dnn;
using System.Management.Automation;

namespace Dnn.Powershell.Local.Commands.Roles
{
    /// <summary>
    /// <para type="synopsis">This will add a role to a specified portal</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.Role)]
    public class AddRole : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">The Portal ID for the portal to add the role to</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        /// <summary>
        /// <para type="description">The rolename to use</para>
        /// </summary>
        [Parameter(Position = 1)]
        public string Rolename { get; set; }

        /// <summary>
        /// <para type="description">If the role must be added to a specific role group, use this parameter.
        /// If not specified the role will be added to the general portal roles.</para>
        /// </summary>
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
