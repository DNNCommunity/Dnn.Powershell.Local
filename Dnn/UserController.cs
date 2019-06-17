using Dnn.Powershell.Local.Common;
using System;
using System.Data;

namespace Dnn.Powershell.Local.Dnn
{
    public class UserController
    {
        public static int AddUser(DNNContext context, int portalId, string userName, string password, string email, string firstName, string lastName, string displayName)
        {
            var cmd = context.GetCommand("{databaseOwner}[aspnet_Membership_CreateUser]")
                .AddParameter("ApplicationName", "DotNetNuke")
                .AddParameter("UserName", userName)
                .AddParameter("Password", password)
                .AddParameter("PasswordSalt", Globals.EncodeBase64((new Guid()).ToString()))
                .AddParameter("Email", email)
                .AddParameter("PasswordQuestion", DBNull.Value)
                .AddParameter("PasswordAnswer", DBNull.Value)
                .AddParameter("IsApproved", true)
                .AddParameter("CurrentTimeUtc", DateTime.Now.ToUniversalTime())
                .AddParameter("CreateDate", DateTime.Now.ToUniversalTime())
                .AddParameter("UniqueEmail", false)
                .AddParameter("PasswordFormat", 0)
                .AddOutputParameter("UserId", SqlDbType.UniqueIdentifier);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            int UserId = -1;
            cmd = context.GetCommand("{databaseOwner}{objectQualifier}AddUser")
                .AddParameter("PortalID", portalId)
                .AddParameter("Username", userName)
                .AddParameter("Firstname", firstName)
                .AddParameter("Lastname", lastName)
                .AddParameter("AffiliateId", DBNull.Value)
                .AddParameter("IsSuperUser", false)
                .AddParameter("Email", email)
                .AddParameter("DisplayName", displayName)
                .AddParameter("UpdatePassword", false)
                .AddParameter("Authorised", true);
            cmd.CommandType = CommandType.StoredProcedure;
            if (context.DNNVersion >= new Version("05.00.00"))
                cmd.AddParameter("CreatedByUserID", 0);
            UserId = Convert.ToInt32(cmd.ExecuteScalar());

            cmd = context.GetCommand("INSERT INTO {databaseOwner}[{objectQualifier}UserRoles] ([UserID],[RoleID],[ExpiryDate],[IsTrialUsed],[EffectiveDate]) SELECT @UserID, r.RoleId, NULL, NULL, NULL FROM {databaseOwner}[{objectQualifier}Roles] r WHERE r.AutoAssignment=1 AND r.PortalID=@PortalID")
                .AddParameter("PortalID", portalId)
                .AddParameter("UserID", UserId);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            return UserId;
        }

        public static void AddUserToRole(DNNContext context, int portalId, int userId, string roleName)
        {
            var cmd = context.GetCommand(Sql.SqlScripts.GetScript("Sql.Scripts.AddUserToRole", context.DNNVersion))
                .AddParameter("PortalID", portalId)
                .AddParameter("UserID", userId)
                .AddParameter("Rolename", roleName);
                cmd.ExecuteNonQuery();
        }
    }
}

