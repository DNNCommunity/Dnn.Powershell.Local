USE master

DECLARE @DatabaseName nvarchar(50)
SET @DatabaseName = N'{DBName}'

DECLARE @SQL varchar(max)
SET @SQL = ''

SELECT @SQL = @SQL + 'Kill ' + Convert(varchar, SPId) + ';'
FROM MASTER..SysProcesses
WHERE DBId = DB_ID(@DatabaseName) AND SPId <> @@SPId

EXEC(@SQL)
GO

DECLARE @Table TABLE (
    LogicalName varchar(128),
    [PhysicalName] varchar(128), 
    [Type] varchar, 
    [FileGroupName] varchar(128), 
    [Size] varchar(128),
    [MaxSize] varchar(128), 
    [FileId]varchar(128), 
    [CreateLSN]varchar(128), 
    [DropLSN]varchar(128), 
    [UniqueId]varchar(128), 
    [ReadOnlyLSN]varchar(128), 
    [ReadWriteLSN]varchar(128),
    [BackupSizeInBytes]varchar(128), 
    [SourceBlockSize]varchar(128), 
    [FileGroupId]varchar(128), 
    [LogGroupGUID]varchar(128), 
    [DifferentialBaseLSN]varchar(128), 
    [DifferentialBaseGUID]varchar(128), 
    [IsReadOnly]varchar(128), 
    [IsPresent]varchar(128), 
    [TDEThumbprint]varchar(128),
    [SnapshotUrl]varchar(128)
)
DECLARE @Path varchar(1000)='{BakFile}'
DECLARE @LogicalNameData varchar(128),@LogicalNameLog varchar(128)
INSERT INTO @table
EXEC('
RESTORE FILELISTONLY 
   FROM DISK=''' +@Path+ '''
   ');
SET @LogicalNameData=(SELECT LogicalName FROM @Table WHERE Type='D');
SET @LogicalNameLog=(SELECT LogicalName FROM @Table WHERE Type='L');

RESTORE DATABASE [www_formamed_dev] 
 FROM  DISK = N'{BakFile}'
 WITH  FILE = 1,  
 MOVE @LogicalNameData TO N'{DBPath}\{DBName}.mdf',  
 MOVE @LogicalNameLog TO N'{DBPath}\{DBName}_log.ldf',  
 NOUNLOAD,  REPLACE,  STATS = 10

GO

IF NOT EXISTS(SELECT * FROM sys.sysusers su join sys.syslogins sl on sl.sid = su.sid where sl.name = '{DBLogin}')
CREATE USER [{DBLogin} User] FOR LOGIN [{DBLogin}]
GO
DECLARE @uname NVARCHAR(100) = (SELECT TOP 1 su.name
FROM sys.sysusers su
join sys.syslogins sl on sl.sid = su.sid
where sl.name = '{DBLogin}');
EXEC sp_addrolemember N'db_owner', @uname;
GO
EXEC dbo.sp_changedbowner @loginame = N'sa', @map = false
GO

