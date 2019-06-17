using Dnn.Powershell.Local.Common;
using Dnn.Powershell.Local.Dnn;
using System.Management.Automation;
using System;
using System.Text;

namespace Dnn.Powershell.Local.Commands.Users
{
    /// <summary>
    /// <para type="synopsis">Adds a number of users to a portal</para>
    /// <para type="description">This uses randomly generated names to add a number of users to a portal.
    /// It will also randomly assign those users roles present in this portal.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.Users)]
    public class AddUsers : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">The portal ID for the portal to add the users to</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        /// <summary>
        /// <para type="description">Number of users to add</para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public int NrUsers { get; set; }

        /// <summary>
        /// <para type="description">Minimum number of roles to assign</para>
        /// </summary>
        [Parameter()]
        public int MinRoles { get; set; } = 0;

        /// <summary>
        /// <para type="description">Maximum number of roles to assign. If not specified
        /// then this will be the number of roles found.</para>
        /// </summary>
        [Parameter()]
        public int MaxRoles { get; set; } = -1;

        /// <summary>
        /// <para type="description">ID of the role group to use for role assignment.
        /// If not specified then roles are chosen from all roles in the portal.</para>
        /// </summary>
        [Parameter()]
        public int RoleGroupID { get; set; } = -2;

        /// <summary>
        /// <para type="description">Role group name to use for role assignment.
        /// Use this parameter in case the role group ID is unknown and this command
        /// will look it up.</para>
        /// </summary>
        [Parameter()]
        public string RoleGroupName { get; set; }

        /// <summary>
        /// <para type="description">The password to use for generated users. If
        /// not specified then the password will be firstname+lastname.</para>
        /// </summary>
        [Parameter()]
        public string Password { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose("Starting Add Users Script");

            if (!string.IsNullOrEmpty(RoleGroupName))
                RoleGroupID = RoleController.GetOrCreateRoleGroupIdByName(Context, PortalId, RoleGroupName);

            var NrRoles = -1;
            if (RoleGroupID == -2)
            {
                using (System.Data.SqlClient.SqlCommand cmd = Context.GetCommand("SELECT COUNT(*) FROM {databaseOwner}[{objectQualifier}Roles] WHERE PortalID=@PortalID AND AutoAssignment=0"))
                {
                    cmd.AddParameter("PortalID", PortalId);
                    NrRoles = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            else
                using (System.Data.SqlClient.SqlCommand cmd = Context.GetCommand("SELECT COUNT(*) FROM {databaseOwner}[{objectQualifier}Roles] WHERE PortalID=@PortalID AND AutoAssignment=0 AND RoleGroupID=@RoleGroupID"))
                {
                    cmd.AddParameter("PortalID", PortalId);
                    if (RoleGroupID == -1)
                        cmd.AddParameter("RoleGroupID", DBNull.Value);
                    else
                        cmd.AddParameter("RoleGroupID", RoleGroupID);
                    NrRoles = Convert.ToInt32(cmd.ExecuteScalar());
                }
            if (MaxRoles == -1)
                MaxRoles = NrRoles;
            if (MinRoles > MaxRoles)
                MinRoles = MaxRoles;
            WriteVerbose(string.Format("Adding between {0} and {1} roles to each user", MinRoles, MaxRoles));

            StringBuilder roleAssignScript = new StringBuilder();

            for (int i = NrUsers; i >= 1; i += -1)
            {
                string firstname = Common.Random.GetRandomFirstName();
                string lastname = Common.Random.GetRandomLastName();
                string displayName = string.Format("{0} {1}", firstname, lastname);
                string email = string.Format("{0}@{1}.name", firstname, lastname);
                string username = string.Format("{0}{1}{2}", firstname.Substring(0, 1), lastname, Common.Random.RandomNumber(0, 999));
                string password = string.IsNullOrEmpty(Password) ? firstname + lastname : Password;
                int UserId = UserController.AddUser(Context, PortalId, username, password, email, firstname, lastname, displayName);
                WriteVerbose(string.Format("{0} Created User {1}", i, displayName));

                roleAssignScript.Append("INSERT INTO {databaseOwner}[{objectQualifier}UserRoles] ([UserID],[RoleID],[ExpiryDate],[IsTrialUsed],[EffectiveDate]) SELECT TOP " + Common.Random.RandomNumber(MinRoles, MaxRoles).ToString() + " " + UserId.ToString() + ", r.RoleId, NULL, NULL, NULL FROM {databaseOwner}[{objectQualifier}Roles] r WHERE PortalID=@PortalID AND AutoAssignment=0 ");
                if (RoleGroupID != -2)
                    roleAssignScript.Append("AND RoleGroupID=@RoleGroupID ");
                roleAssignScript.AppendLine("ORDER BY NEWID();");

                if ((i % 200) == 0)
                {
                    AssignRoles(roleAssignScript);
                    roleAssignScript = new StringBuilder();
                }
            }

            AssignRoles(roleAssignScript);
        }

        private void AssignRoles(StringBuilder roleAssignScript)
        {
            WriteVerbose("Assigning Roles");
            var cmd = Context.GetCommand(roleAssignScript.ToString())
                .AddParameter("PortalID", PortalId);
            if (RoleGroupID != -2)
            {
                if (RoleGroupID == -1)
                    cmd = cmd.AddParameter("RoleGroupID", DBNull.Value);
                else
                    cmd = cmd.AddParameter("RoleGroupID", RoleGroupID);
            }
            cmd.ExecuteNonQuery();
        }
    }
}