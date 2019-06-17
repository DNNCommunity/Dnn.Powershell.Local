IF NOT EXISTS(SELECT * FROM {databaseOwner}[{objectQualifier}RoleGroups] WHERE PortalID=@PortalID AND RoleGroupName=@RoleGroupName)
 EXECUTE {databaseOwner}[{objectQualifier}AddRoleGroup] @PortalID, @RoleGroupName, 'Roles added by script', 0;
SELECT RoleGroupID FROM {databaseOwner}[{objectQualifier}RoleGroups] WHERE PortalID=@PortalID AND RoleGroupName=@RoleGroupName;
