/*
SETUP
-----

0. Install SQL 2016 with SP1. Make sure you have sufficient number of TEMPDB data files, ideally 1 per logical CPU to avoid allocation bottlenecks. In my repro I had 8 data files for 8 logical CPUs.

1. Install RML tools from https://www.microsoft.com/en-us/download/details.aspx?id=4511. You may need the SQL 2012 native client as well ()

2. Run the script shown below to create the procedure, user defined table type necessary for the repro
*/

USE tempdb
GO

CREATE TYPE [dbo].[keyholder] AS TABLE(
	[mycounter] [int] IDENTITY(1,1) NOT NULL,
	[somekey] [int] NOT NULL,
	 PRIMARY KEY NONCLUSTERED 
(
	[mycounter] ASC
)
)
go

create proc reproissue
(@p1 dbo.keyholder readonly)
as
begin
set nocount on; 

create table #t1 ([Guid] uniqueidentifier NOT NULL PRIMARY KEY); 
insert #t1 values (newid())
select * into #t2 from @p1; 

end
GO

/*
3. Repro the issue by running OSTRESS as given below. Adjust the values for n (256 is on a 8-logical CPU system; on a 32 CPU machine you can use -n1024.

ostress -S. -dTEMPDB -Q"set nocount on; DECLARE @p1 dbo.keyholder; insert @p1 (somekey) values (1); exec reproissue @p1" -n256 -r1000000

*/