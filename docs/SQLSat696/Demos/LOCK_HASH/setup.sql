CREATE LOGIN [nonadmin] WITH PASSWORD=N'somepassword', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

create database lockhashrepro
go

use lockhashrepro
go

ALTER DATABASE lockhashrepro ADD FILEGROUP imoltp CONTAINS MEMORY_OPTIMIZED_DATA  
go

ALTER DATABASE lockhashrepro ADD FILE (name='imoltp_mod1', filename='c:\sqldb\lockhash_imoltp_sql2017') TO FILEGROUP imoltp
go  

create table memopt(
id bigint identity (1, 1) not null primary key nonclustered hash with (bucket_count = 1000)
)
with (memory_optimized = on, durability = schema_only)
go

create or alter procedure dbo.native
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
go  

create or alter procedure interop
as
begin
	exec dbo.native
end
go

create user nonadmin from login [nonadmin]
alter role db_datareader add member [nonadmin]
alter role db_datawriter add member [nonadmin]
GO

grant execute on interop to nonadmin
go

/*
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE. 

This sample code is not supported under any Microsoft standard support program or service.  
The entire risk arising out of the use or performance of the sample scripts and documentation remains with you.  
In no event shall Microsoft, its authors, or anyone else involved in the creation, production, or delivery of the scripts 
be liable for any damages whatsoever (including, without limitation, damages for loss of business profits, 
business interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability 
to use the sample scripts or documentation, even if Microsoft has been advised of the possibility of such damages. 
*/
