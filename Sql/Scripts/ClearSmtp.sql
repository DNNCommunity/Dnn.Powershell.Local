UPDATE {databaseOwner}{objectQualifier}HostSettings
SET SettingValue='0'
WHERE SettingName='SMTPAuthentication'
GO

UPDATE {databaseOwner}{objectQualifier}HostSettings
SET SettingValue='N'
WHERE SettingName='SMTPEnableSSL'
GO

UPDATE {databaseOwner}{objectQualifier}HostSettings
SET SettingValue=''
WHERE SettingName='SMTPPassword'
GO

UPDATE {databaseOwner}{objectQualifier}HostSettings
SET SettingValue='0.0.0.0'
WHERE SettingName='SMTPServer'
GO

UPDATE {databaseOwner}{objectQualifier}HostSettings
SET SettingValue=''
WHERE SettingName='SMTPUsername'
GO

