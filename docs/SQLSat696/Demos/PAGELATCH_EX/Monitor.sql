/*
Monitor TEMPDB PAGELATCH_EX contention by using the scripts below.

[1] Query to confirm where the resource waits are maximum
*/

select count(*), wait_type, resource_description, sum(wait_duration_ms)
from sys.dm_os_waiting_tasks
group by wait_type, resource_description
having count(*) > 2
order by sum(wait_duration_ms) desc

-- [2] XE session for capturing wait info histograms

CREATE EVENT SESSION XEWaits ON SERVER
ADD EVENT sqlos.wait_info(             
ACTION(package0.callstack)
WHERE (
[wait_type]=(52) and duration > 0) -- please verify the number by querying select map_key from sys.dm_xe_map_values where map_value = 'pagelatch_ex'
)
ADD TARGET package0.histogram  
(SET source_type=1,
source=N'package0.callstack')
WITH (MAX_MEMORY=32768 KB,EVENT_RETENTION_MODE=ALLOW_SINGLE_EVENT_LOSS,
MAX_DISPATCH_LATENCY=5 SECONDS,MAX_EVENT_SIZE=0 KB,MEMORY_PARTITION_MODE=PER_CPU,TRACK_CAUSALITY=OFF,STARTUP_STATE=OFF)
GO 
 
alter event session XEWaits on server state = start
GO

-- once the problem is reproduced, get callstacks
select event_session_address, target_name, execution_count, cast (target_data as XML) as WaitData
from sys.dm_xe_session_targets xst
inner join sys.dm_xe_sessions xs on (xst.event_session_address = xs.address)
where xs.name = 'XEWaits' 

-- stop XE
alter event session XEWaits on server state = STOP
GO
