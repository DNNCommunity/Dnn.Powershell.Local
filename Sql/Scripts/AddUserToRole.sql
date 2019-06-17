DECLARE @UserRoleId int
DECLARE @RoleId int

SELECT @UserRoleId = null
SELECT @RoleId = null

SELECT @UserRoleId = ur.UserRoleId
	FROM {databaseOwner}{objectQualifier}UserRoles ur
	INNER JOIN {databaseOwner}{objectQualifier}Roles r ON r.RoleID=ur.RoleID
	WHERE ur.UserId = @UserID
  AND r.RoleName = @RoleName
	 AND r.PortalID = @PortalID

SELECT @RoleId = r.RoleId
	FROM {databaseOwner}{objectQualifier}Roles r
	WHERE r.RoleName = @RoleName
	 AND r.PortalID = @PortalID
 
IF @UserRoleId IS NOT NULL
	BEGIN
		UPDATE {databaseOwner}{objectQualifier}UserRoles
			SET 
				Status = 1,
				IsOwner = 0,
				ExpiryDate = NULL,
				EffectiveDate = NULL,
				IsTrialUsed = 1,
				LastModifiedByUserID = 1,
				LastModifiedOnDate = getdate()
			WHERE  UserRoleId = @UserRoleId
		SELECT @UserRoleId
	END
ELSE
	BEGIN
		INSERT INTO {databaseOwner}{objectQualifier}UserRoles (
			UserId,
			RoleId,
			Status,
			IsOwner,
			EffectiveDate,
			ExpiryDate,
			IsTrialUsed,
			CreatedByUserID,
			CreatedOnDate,
			LastModifiedByUserID,
			LastModifiedOnDate
		  )
		VALUES (
			@UserID,
			@RoleId,
			1,
			0,
			NULL,
			NULL,
			1,
			1,
			getdate(),
			1,
			getdate()
		  )

 END
 