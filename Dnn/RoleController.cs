using Dnn.Powershell.Local.Common;
using System;
using System.Data;

namespace Dnn.Powershell.Local.Dnn
{
    public class RoleController
    {
        public static int GetRoleIdByName(DNNContext context, int portalId, string roleName)
        {
            int RoleID = -1;
            var cmd = context.GetCommand("SELECT RoleID FROM {databaseOwner}{objectQualifier}Roles WHERE PortalId=@PortalID AND Rolename=@Rolename")
                .AddParameter("PortalID", portalId)
                .AddParameter("RoleName", roleName);
            object res = cmd.ExecuteScalar();
            if (res != null)
                RoleID = Convert.ToInt32(res);
            cmd.Dispose();
            return RoleID;
        }

        public static int GetOrCreateRoleGroupIdByName(DNNContext context, int portalId, string roleGroupName)
        {
            int RoleGroupID = -1;
            if (!string.IsNullOrEmpty(roleGroupName))
            {
                var cmd = context.GetCommand(Sql.SqlScripts.GetScript("SetRoleGroup", context.DNNVersion))
                    .AddParameter("PortalID", portalId)
                    .AddParameter("RoleGroupName", roleGroupName);
                RoleGroupID = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
            }
            return RoleGroupID;
        }

        public static int AddRole(DNNContext context, int portalId, int roleGroupID, string roleName)
        {
            int RoleID = -1;
            var cmd = context.GetCommand("{databaseOwner}{objectQualifier}AddRole")
                .AddParameter("PortalID", portalId)
                .AddParameter("RoleGroupId", roleGroupID == -1 ? (object)DBNull.Value : roleGroupID)
                .AddParameter("RoleName", roleName)
                .AddParameter("Description", "")
                .AddParameter("ServiceFee", 0)
                .AddParameter("BillingPeriod", -1)
                .AddParameter("BillingFrequency", "N")
                .AddParameter("TrialFee", 0)
                .AddParameter("TrialPeriod", -1)
                .AddParameter("TrialFrequency", "N")
                .AddParameter("IsPublic", false)
                .AddParameter("AutoAssignment", false)
                .AddParameter("RSVPCode", DBNull.Value)
                .AddParameter("IconFile", DBNull.Value)
                .AddParameter("CreatedByUserID", -1)
                .AddParameter("Status", 1)
                .AddParameter("SecurityMode", 0)
                .AddParameter("IsSystemRole", false);
            cmd.CommandType = CommandType.StoredProcedure;
            RoleID = Convert.ToInt32(cmd.ExecuteScalar());
            return RoleID;
        }
    }
}
