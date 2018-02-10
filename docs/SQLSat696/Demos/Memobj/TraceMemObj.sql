IF EXISTS(SELECT * FROM sys.server_event_sessions WHERE name='TraceMemObj')
    DROP EVENT SESSION [TraceMemObj] ON SERVER
GO

CREATE EVENT SESSION [TraceMemObj] ON SERVER 
ADD EVENT sqlos.page_allocated(
    ACTION(package0.callstack))
,
ADD EVENT sqlos.page_freed(
    ACTION(package0.callstack))
ADD TARGET package0.histogram  
(SET source_type=1,
     source=N'package0.callstack')
WITH (MAX_MEMORY=32768 KB,EVENT_RETENTION_MODE=ALLOW_SINGLE_EVENT_LOSS,
MAX_DISPATCH_LATENCY=5 SECONDS,MAX_EVENT_SIZE=0 KB,MEMORY_PARTITION_MODE=PER_CPU,TRACK_CAUSALITY=OFF,STARTUP_STATE=OFF)
GO

ALTER EVENT SESSION [TraceMemObj] on server state = start
GO

-- wait for problem to repro, and then run the below query. Save the XML data into a file. It can then be visualized using SQLCallStackResolver.
select event_session_address, target_name, execution_count, cast (target_data as XML) as MemObjData
from sys.dm_xe_session_targets xst
      inner join sys.dm_xe_sessions xs on (xst.event_session_address = xs.address)
where xs.name = 'TraceMemObj'
GO

-- Then stop the session
-- ALTER EVENT SESSION [TraceMemObj] on server state = stop

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
