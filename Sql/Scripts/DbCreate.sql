﻿CREATE DATABASE [{DBName}] ON  PRIMARY
( NAME = N'{DBName}', FILENAME = N'{DBPath}\{DBName}.mdf')
 LOG ON 
( NAME = N'{DBName}_log', FILENAME = N'{DBPath}\{DBName}_log.ldf')
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'{DBName}', @new_cmptlevel=100
GO
ALTER DATABASE [{DBName}] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [{DBName}] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [{DBName}] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [{DBName}] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [{DBName}] SET ARITHABORT OFF 
GO
ALTER DATABASE [{DBName}] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [{DBName}] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [{DBName}] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [{DBName}] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [{DBName}] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [{DBName}] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [{DBName}] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [{DBName}] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [{DBName}] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [{DBName}] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [{DBName}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [{DBName}] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [{DBName}] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [{DBName}] SET  READ_WRITE 
GO
ALTER DATABASE [{DBName}] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [{DBName}] SET  MULTI_USER 
GO
ALTER DATABASE [{DBName}] SET PAGE_VERIFY CHECKSUM  
GO
USE [{DBName}]
GO
IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [{DBName}] MODIFY FILEGROUP [PRIMARY] DEFAULT
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

