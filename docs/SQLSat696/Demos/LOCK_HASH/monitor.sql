SELECT @@SERVERNAME, @@VERSION
GO

SELECT * 
FROM sys.dm_os_spinlock_stats
ORDER BY backoffs desc
GO

SELECT *
FROM sys.dm_os_wait_stats
ORDER BY signal_wait_time_ms DESC
GO

dbcc sqlperf(spinlockstats, 'clear')
GO

dbcc sqlperf(waitstats, 'clear')
GO

SELECT *
FROM sys.dm_os_nodes
GO

exec xp_readerrorlog
GO

SELECT * 
FROM sys.dm_tcp_listener_states
GO

dbcc tracestatus(-1)
GO

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