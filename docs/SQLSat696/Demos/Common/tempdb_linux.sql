use [tempdb]

GO
use [model]

GO
USE [master]
GO
ALTER DATABASE [tempdb] MODIFY FILE ( NAME = N'tempdev', SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev2', FILENAME = N'/var/opt/mssql/data/tempdb2.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev3', FILENAME = N'/var/opt/mssql/data/tempdb3.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev4', FILENAME = N'/var/opt/mssql/data/tempdb4.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev5', FILENAME = N'/var/opt/mssql/data/tempdb5.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev6', FILENAME = N'/var/opt/mssql/data/tempdb6.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev7', FILENAME = N'/var/opt/mssql/data/tempdb7.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev8', FILENAME = N'/var/opt/mssql/data/tempdb8.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev9', FILENAME = N'/var/opt/mssql/data/tempdb9.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev10', FILENAME = N'/var/opt/mssql/data/tempdb10.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev11', FILENAME = N'/var/opt/mssql/data/tempdb11.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev12', FILENAME = N'/var/opt/mssql/data/tempdb12.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev13', FILENAME = N'/var/opt/mssql/data/tempdb13.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev14', FILENAME = N'/var/opt/mssql/data/tempdb14.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev15', FILENAME = N'/var/opt/mssql/data/tempdb15.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
ALTER DATABASE [tempdb] ADD FILE ( NAME = N'tempdev16', FILENAME = N'/var/opt/mssql/data/tempdb16.ndf' , SIZE = 512000KB , FILEGROWTH = 512000KB )
GO
