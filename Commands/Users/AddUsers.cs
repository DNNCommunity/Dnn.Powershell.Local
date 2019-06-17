using Dnn.Powershell.Local.Common;
using Dnn.Powershell.Local.Dnn;
using System.Management.Automation;
using System;
using System.Text;

namespace Dnn.Powershell.Local.Commands.Users
{
    [Cmdlet(VerbsCommon.Add, Nouns.Users)]
    public class AddUsers : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int NrUsers { get; set; }

        [Parameter()]
        public int MinRoles { get; set; } = 0;

        [Parameter()]
        public int MaxRoles { get; set; } = -1;

        [Parameter()]
        public int RoleGroupID { get; set; } = -2;

        [Parameter()]
        public string RoleGroupName { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose("Starting Bring2mind Add Users Script");

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
                string username = string.Format("{0}{1}{2}", firstname, lastname, Common.Random.RandomNumber(0, 999));
                string password = firstname + lastname;
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