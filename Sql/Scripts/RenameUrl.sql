UPDATE {databaseOwner}{objectQualifier}PortalAlias
SET HTTPAlias=REPLACE(HTTPAlias, '{OldUrl}', '{NewUrl}')
GO

UPDATE {databaseOwner}{objectQualifier}Tabs
SET Url = REPLACE(Url,'{OldUrl}', '{NewUrl}')
WHERE NOT Url IS NULL
GO
