CREATE EVENT SESSION [OOM] ON SERVER 
ADD EVENT sqlserver.error_reported(
    ACTION(package0.callstack)
    WHERE (error_number = 701 ))
ADD TARGET package0.ring_buffer
WITH (STARTUP_STATE=OFF)
GO

alter event session [OOM] on server state = start
GO

SELECT event_session_address,
       target_name,
       execution_count,
       CAST (target_data AS XML) AS CallStack
FROM   sys.dm_xe_session_targets AS xst
       INNER JOIN
       sys.dm_xe_sessions AS xs
       ON (xst.event_session_address = xs.address)
WHERE  xs.name = 'OOM';
GO

-- alter event session [OOM] on server state = stop

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
