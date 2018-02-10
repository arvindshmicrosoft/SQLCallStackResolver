IF EXISTS(SELECT * FROM sys.server_event_sessions WHERE name='XESpins')
    DROP EVENT SESSION XESpins ON SERVER
GO

select * 
from sys.dm_xe_map_values
where map_value = 'LOCK_HASH'
	 
CREATE EVENT SESSION XESpins ON SERVER 
ADD EVENT sqlos.spinlock_backoff
    (
    ACTION (package0.callstack)
    WHERE type = 151	-- MAKE SURE YOU CHANGE THIS NUMBER!
    ) 
ADD TARGET package0.histogram
    (
    SET source_type = 1,
        source = N'package0.callstack'
    )
WITH  (
        MAX_MEMORY = 32768 KB,
        EVENT_RETENTION_MODE = ALLOW_SINGLE_EVENT_LOSS,
        MAX_DISPATCH_LATENCY = 5 SECONDS,
        MAX_EVENT_SIZE = 0 KB,
        MEMORY_PARTITION_MODE = PER_CPU,
        TRACK_CAUSALITY = OFF,
        STARTUP_STATE = OFF
      );
GO

alter event session XESpins on server state = start
GO

select event_session_address, target_name, execution_count, cast (target_data as XML) as SpinlockData
from sys.dm_xe_session_targets xst
      inner join sys.dm_xe_sessions xs on (xst.event_session_address = xs.address)
where xs.name = 'XESpins'
GO 

-- alter event session XESpins on server state = stop
