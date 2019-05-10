use lockhashrepro
go
create or ALTER   procedure [dbo].[interop_1] as begin 	exec dbo.native_1 end
GO
	CREATE OR ALTER   procedure [dbo].[native_1]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_1 to nonadmin;	GRANT EXECUTE ON native_1 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_2] as begin 	exec dbo.native_2 end
GO
	CREATE OR ALTER   procedure [dbo].[native_2]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_2 to nonadmin;	GRANT EXECUTE ON native_2 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_3] as begin 	exec dbo.native_3 end
GO
	CREATE OR ALTER   procedure [dbo].[native_3]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_3 to nonadmin;	GRANT EXECUTE ON native_3 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_4] as begin 	exec dbo.native_4 end
GO
	CREATE OR ALTER   procedure [dbo].[native_4]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_4 to nonadmin;	GRANT EXECUTE ON native_4 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_5] as begin 	exec dbo.native_5 end
GO
	CREATE OR ALTER   procedure [dbo].[native_5]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_5 to nonadmin;	GRANT EXECUTE ON native_5 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_6] as begin 	exec dbo.native_6 end
GO
	CREATE OR ALTER   procedure [dbo].[native_6]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_6 to nonadmin;	GRANT EXECUTE ON native_6 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_7] as begin 	exec dbo.native_7 end
GO
	CREATE OR ALTER   procedure [dbo].[native_7]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_7 to nonadmin;	GRANT EXECUTE ON native_7 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_8] as begin 	exec dbo.native_8 end
GO
	CREATE OR ALTER   procedure [dbo].[native_8]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_8 to nonadmin;	GRANT EXECUTE ON native_8 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_9] as begin 	exec dbo.native_9 end
GO
	CREATE OR ALTER   procedure [dbo].[native_9]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_9 to nonadmin;	GRANT EXECUTE ON native_9 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_10] as begin 	exec dbo.native_10 end
GO
	CREATE OR ALTER   procedure [dbo].[native_10]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_10 to nonadmin;	GRANT EXECUTE ON native_10 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_11] as begin 	exec dbo.native_11 end
GO
	CREATE OR ALTER   procedure [dbo].[native_11]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_11 to nonadmin;	GRANT EXECUTE ON native_11 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_12] as begin 	exec dbo.native_12 end
GO
	CREATE OR ALTER   procedure [dbo].[native_12]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_12 to nonadmin;	GRANT EXECUTE ON native_12 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_13] as begin 	exec dbo.native_13 end
GO
	CREATE OR ALTER   procedure [dbo].[native_13]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_13 to nonadmin;	GRANT EXECUTE ON native_13 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_14] as begin 	exec dbo.native_14 end
GO
	CREATE OR ALTER   procedure [dbo].[native_14]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_14 to nonadmin;	GRANT EXECUTE ON native_14 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_15] as begin 	exec dbo.native_15 end
GO
	CREATE OR ALTER   procedure [dbo].[native_15]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_15 to nonadmin;	GRANT EXECUTE ON native_15 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_16] as begin 	exec dbo.native_16 end
GO
	CREATE OR ALTER   procedure [dbo].[native_16]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_16 to nonadmin;	GRANT EXECUTE ON native_16 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_17] as begin 	exec dbo.native_17 end
GO
	CREATE OR ALTER   procedure [dbo].[native_17]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_17 to nonadmin;	GRANT EXECUTE ON native_17 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_18] as begin 	exec dbo.native_18 end
GO
	CREATE OR ALTER   procedure [dbo].[native_18]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_18 to nonadmin;	GRANT EXECUTE ON native_18 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_19] as begin 	exec dbo.native_19 end
GO
	CREATE OR ALTER   procedure [dbo].[native_19]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_19 to nonadmin;	GRANT EXECUTE ON native_19 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_20] as begin 	exec dbo.native_20 end
GO
	CREATE OR ALTER   procedure [dbo].[native_20]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_20 to nonadmin;	GRANT EXECUTE ON native_20 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_21] as begin 	exec dbo.native_21 end
GO
	CREATE OR ALTER   procedure [dbo].[native_21]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_21 to nonadmin;	GRANT EXECUTE ON native_21 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_22] as begin 	exec dbo.native_22 end
GO
	CREATE OR ALTER   procedure [dbo].[native_22]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_22 to nonadmin;	GRANT EXECUTE ON native_22 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_23] as begin 	exec dbo.native_23 end
GO
	CREATE OR ALTER   procedure [dbo].[native_23]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_23 to nonadmin;	GRANT EXECUTE ON native_23 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_24] as begin 	exec dbo.native_24 end
GO
	CREATE OR ALTER   procedure [dbo].[native_24]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_24 to nonadmin;	GRANT EXECUTE ON native_24 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_25] as begin 	exec dbo.native_25 end
GO
	CREATE OR ALTER   procedure [dbo].[native_25]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_25 to nonadmin;	GRANT EXECUTE ON native_25 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_26] as begin 	exec dbo.native_26 end
GO
	CREATE OR ALTER   procedure [dbo].[native_26]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_26 to nonadmin;	GRANT EXECUTE ON native_26 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_27] as begin 	exec dbo.native_27 end
GO
	CREATE OR ALTER   procedure [dbo].[native_27]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_27 to nonadmin;	GRANT EXECUTE ON native_27 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_28] as begin 	exec dbo.native_28 end
GO
	CREATE OR ALTER   procedure [dbo].[native_28]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_28 to nonadmin;	GRANT EXECUTE ON native_28 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_29] as begin 	exec dbo.native_29 end
GO
	CREATE OR ALTER   procedure [dbo].[native_29]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_29 to nonadmin;	GRANT EXECUTE ON native_29 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_30] as begin 	exec dbo.native_30 end
GO
	CREATE OR ALTER   procedure [dbo].[native_30]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_30 to nonadmin;	GRANT EXECUTE ON native_30 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_31] as begin 	exec dbo.native_31 end
GO
	CREATE OR ALTER   procedure [dbo].[native_31]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_31 to nonadmin;	GRANT EXECUTE ON native_31 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_32] as begin 	exec dbo.native_32 end
GO
	CREATE OR ALTER   procedure [dbo].[native_32]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_32 to nonadmin;	GRANT EXECUTE ON native_32 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_33] as begin 	exec dbo.native_33 end
GO
	CREATE OR ALTER   procedure [dbo].[native_33]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_33 to nonadmin;	GRANT EXECUTE ON native_33 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_34] as begin 	exec dbo.native_34 end
GO
	CREATE OR ALTER   procedure [dbo].[native_34]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_34 to nonadmin;	GRANT EXECUTE ON native_34 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_35] as begin 	exec dbo.native_35 end
GO
	CREATE OR ALTER   procedure [dbo].[native_35]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_35 to nonadmin;	GRANT EXECUTE ON native_35 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_36] as begin 	exec dbo.native_36 end
GO
	CREATE OR ALTER   procedure [dbo].[native_36]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_36 to nonadmin;	GRANT EXECUTE ON native_36 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_37] as begin 	exec dbo.native_37 end
GO
	CREATE OR ALTER   procedure [dbo].[native_37]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_37 to nonadmin;	GRANT EXECUTE ON native_37 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_38] as begin 	exec dbo.native_38 end
GO
	CREATE OR ALTER   procedure [dbo].[native_38]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_38 to nonadmin;	GRANT EXECUTE ON native_38 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_39] as begin 	exec dbo.native_39 end
GO
	CREATE OR ALTER   procedure [dbo].[native_39]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_39 to nonadmin;	GRANT EXECUTE ON native_39 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_40] as begin 	exec dbo.native_40 end
GO
	CREATE OR ALTER   procedure [dbo].[native_40]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_40 to nonadmin;	GRANT EXECUTE ON native_40 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_41] as begin 	exec dbo.native_41 end
GO
	CREATE OR ALTER   procedure [dbo].[native_41]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_41 to nonadmin;	GRANT EXECUTE ON native_41 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_42] as begin 	exec dbo.native_42 end
GO
	CREATE OR ALTER   procedure [dbo].[native_42]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_42 to nonadmin;	GRANT EXECUTE ON native_42 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_43] as begin 	exec dbo.native_43 end
GO
	CREATE OR ALTER   procedure [dbo].[native_43]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_43 to nonadmin;	GRANT EXECUTE ON native_43 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_44] as begin 	exec dbo.native_44 end
GO
	CREATE OR ALTER   procedure [dbo].[native_44]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_44 to nonadmin;	GRANT EXECUTE ON native_44 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_45] as begin 	exec dbo.native_45 end
GO
	CREATE OR ALTER   procedure [dbo].[native_45]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_45 to nonadmin;	GRANT EXECUTE ON native_45 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_46] as begin 	exec dbo.native_46 end
GO
	CREATE OR ALTER   procedure [dbo].[native_46]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_46 to nonadmin;	GRANT EXECUTE ON native_46 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_47] as begin 	exec dbo.native_47 end
GO
	CREATE OR ALTER   procedure [dbo].[native_47]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_47 to nonadmin;	GRANT EXECUTE ON native_47 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_48] as begin 	exec dbo.native_48 end
GO
	CREATE OR ALTER   procedure [dbo].[native_48]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_48 to nonadmin;	GRANT EXECUTE ON native_48 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_49] as begin 	exec dbo.native_49 end
GO
	CREATE OR ALTER   procedure [dbo].[native_49]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_49 to nonadmin;	GRANT EXECUTE ON native_49 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_50] as begin 	exec dbo.native_50 end
GO
	CREATE OR ALTER   procedure [dbo].[native_50]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_50 to nonadmin;	GRANT EXECUTE ON native_50 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_51] as begin 	exec dbo.native_51 end
GO
	CREATE OR ALTER   procedure [dbo].[native_51]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_51 to nonadmin;	GRANT EXECUTE ON native_51 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_52] as begin 	exec dbo.native_52 end
GO
	CREATE OR ALTER   procedure [dbo].[native_52]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_52 to nonadmin;	GRANT EXECUTE ON native_52 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_53] as begin 	exec dbo.native_53 end
GO
	CREATE OR ALTER   procedure [dbo].[native_53]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_53 to nonadmin;	GRANT EXECUTE ON native_53 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_54] as begin 	exec dbo.native_54 end
GO
	CREATE OR ALTER   procedure [dbo].[native_54]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_54 to nonadmin;	GRANT EXECUTE ON native_54 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_55] as begin 	exec dbo.native_55 end
GO
	CREATE OR ALTER   procedure [dbo].[native_55]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_55 to nonadmin;	GRANT EXECUTE ON native_55 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_56] as begin 	exec dbo.native_56 end
GO
	CREATE OR ALTER   procedure [dbo].[native_56]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_56 to nonadmin;	GRANT EXECUTE ON native_56 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_57] as begin 	exec dbo.native_57 end
GO
	CREATE OR ALTER   procedure [dbo].[native_57]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_57 to nonadmin;	GRANT EXECUTE ON native_57 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_58] as begin 	exec dbo.native_58 end
GO
	CREATE OR ALTER   procedure [dbo].[native_58]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_58 to nonadmin;	GRANT EXECUTE ON native_58 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_59] as begin 	exec dbo.native_59 end
GO
	CREATE OR ALTER   procedure [dbo].[native_59]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_59 to nonadmin;	GRANT EXECUTE ON native_59 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_60] as begin 	exec dbo.native_60 end
GO
	CREATE OR ALTER   procedure [dbo].[native_60]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_60 to nonadmin;	GRANT EXECUTE ON native_60 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_61] as begin 	exec dbo.native_61 end
GO
	CREATE OR ALTER   procedure [dbo].[native_61]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_61 to nonadmin;	GRANT EXECUTE ON native_61 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_62] as begin 	exec dbo.native_62 end
GO
	CREATE OR ALTER   procedure [dbo].[native_62]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_62 to nonadmin;	GRANT EXECUTE ON native_62 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_63] as begin 	exec dbo.native_63 end
GO
	CREATE OR ALTER   procedure [dbo].[native_63]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_63 to nonadmin;	GRANT EXECUTE ON native_63 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_64] as begin 	exec dbo.native_64 end
GO
	CREATE OR ALTER   procedure [dbo].[native_64]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_64 to nonadmin;	GRANT EXECUTE ON native_64 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_65] as begin 	exec dbo.native_65 end
GO
	CREATE OR ALTER   procedure [dbo].[native_65]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_65 to nonadmin;	GRANT EXECUTE ON native_65 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_66] as begin 	exec dbo.native_66 end
GO
	CREATE OR ALTER   procedure [dbo].[native_66]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_66 to nonadmin;	GRANT EXECUTE ON native_66 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_67] as begin 	exec dbo.native_67 end
GO
	CREATE OR ALTER   procedure [dbo].[native_67]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_67 to nonadmin;	GRANT EXECUTE ON native_67 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_68] as begin 	exec dbo.native_68 end
GO
	CREATE OR ALTER   procedure [dbo].[native_68]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_68 to nonadmin;	GRANT EXECUTE ON native_68 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_69] as begin 	exec dbo.native_69 end
GO
	CREATE OR ALTER   procedure [dbo].[native_69]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_69 to nonadmin;	GRANT EXECUTE ON native_69 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_70] as begin 	exec dbo.native_70 end
GO
	CREATE OR ALTER   procedure [dbo].[native_70]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_70 to nonadmin;	GRANT EXECUTE ON native_70 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_71] as begin 	exec dbo.native_71 end
GO
	CREATE OR ALTER   procedure [dbo].[native_71]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_71 to nonadmin;	GRANT EXECUTE ON native_71 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_72] as begin 	exec dbo.native_72 end
GO
	CREATE OR ALTER   procedure [dbo].[native_72]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_72 to nonadmin;	GRANT EXECUTE ON native_72 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_73] as begin 	exec dbo.native_73 end
GO
	CREATE OR ALTER   procedure [dbo].[native_73]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_73 to nonadmin;	GRANT EXECUTE ON native_73 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_74] as begin 	exec dbo.native_74 end
GO
	CREATE OR ALTER   procedure [dbo].[native_74]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_74 to nonadmin;	GRANT EXECUTE ON native_74 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_75] as begin 	exec dbo.native_75 end
GO
	CREATE OR ALTER   procedure [dbo].[native_75]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_75 to nonadmin;	GRANT EXECUTE ON native_75 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_76] as begin 	exec dbo.native_76 end
GO
	CREATE OR ALTER   procedure [dbo].[native_76]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_76 to nonadmin;	GRANT EXECUTE ON native_76 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_77] as begin 	exec dbo.native_77 end
GO
	CREATE OR ALTER   procedure [dbo].[native_77]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_77 to nonadmin;	GRANT EXECUTE ON native_77 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_78] as begin 	exec dbo.native_78 end
GO
	CREATE OR ALTER   procedure [dbo].[native_78]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_78 to nonadmin;	GRANT EXECUTE ON native_78 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_79] as begin 	exec dbo.native_79 end
GO
	CREATE OR ALTER   procedure [dbo].[native_79]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_79 to nonadmin;	GRANT EXECUTE ON native_79 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_80] as begin 	exec dbo.native_80 end
GO
	CREATE OR ALTER   procedure [dbo].[native_80]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_80 to nonadmin;	GRANT EXECUTE ON native_80 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_81] as begin 	exec dbo.native_81 end
GO
	CREATE OR ALTER   procedure [dbo].[native_81]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_81 to nonadmin;	GRANT EXECUTE ON native_81 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_82] as begin 	exec dbo.native_82 end
GO
	CREATE OR ALTER   procedure [dbo].[native_82]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_82 to nonadmin;	GRANT EXECUTE ON native_82 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_83] as begin 	exec dbo.native_83 end
GO
	CREATE OR ALTER   procedure [dbo].[native_83]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_83 to nonadmin;	GRANT EXECUTE ON native_83 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_84] as begin 	exec dbo.native_84 end
GO
	CREATE OR ALTER   procedure [dbo].[native_84]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_84 to nonadmin;	GRANT EXECUTE ON native_84 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_85] as begin 	exec dbo.native_85 end
GO
	CREATE OR ALTER   procedure [dbo].[native_85]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_85 to nonadmin;	GRANT EXECUTE ON native_85 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_86] as begin 	exec dbo.native_86 end
GO
	CREATE OR ALTER   procedure [dbo].[native_86]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_86 to nonadmin;	GRANT EXECUTE ON native_86 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_87] as begin 	exec dbo.native_87 end
GO
	CREATE OR ALTER   procedure [dbo].[native_87]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_87 to nonadmin;	GRANT EXECUTE ON native_87 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_88] as begin 	exec dbo.native_88 end
GO
	CREATE OR ALTER   procedure [dbo].[native_88]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_88 to nonadmin;	GRANT EXECUTE ON native_88 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_89] as begin 	exec dbo.native_89 end
GO
	CREATE OR ALTER   procedure [dbo].[native_89]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_89 to nonadmin;	GRANT EXECUTE ON native_89 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_90] as begin 	exec dbo.native_90 end
GO
	CREATE OR ALTER   procedure [dbo].[native_90]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_90 to nonadmin;	GRANT EXECUTE ON native_90 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_91] as begin 	exec dbo.native_91 end
GO
	CREATE OR ALTER   procedure [dbo].[native_91]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_91 to nonadmin;	GRANT EXECUTE ON native_91 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_92] as begin 	exec dbo.native_92 end
GO
	CREATE OR ALTER   procedure [dbo].[native_92]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_92 to nonadmin;	GRANT EXECUTE ON native_92 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_93] as begin 	exec dbo.native_93 end
GO
	CREATE OR ALTER   procedure [dbo].[native_93]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_93 to nonadmin;	GRANT EXECUTE ON native_93 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_94] as begin 	exec dbo.native_94 end
GO
	CREATE OR ALTER   procedure [dbo].[native_94]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_94 to nonadmin;	GRANT EXECUTE ON native_94 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_95] as begin 	exec dbo.native_95 end
GO
	CREATE OR ALTER   procedure [dbo].[native_95]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_95 to nonadmin;	GRANT EXECUTE ON native_95 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_96] as begin 	exec dbo.native_96 end
GO
	CREATE OR ALTER   procedure [dbo].[native_96]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_96 to nonadmin;	GRANT EXECUTE ON native_96 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_97] as begin 	exec dbo.native_97 end
GO
	CREATE OR ALTER   procedure [dbo].[native_97]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_97 to nonadmin;	GRANT EXECUTE ON native_97 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_98] as begin 	exec dbo.native_98 end
GO
	CREATE OR ALTER   procedure [dbo].[native_98]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_98 to nonadmin;	GRANT EXECUTE ON native_98 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_99] as begin 	exec dbo.native_99 end
GO
	CREATE OR ALTER   procedure [dbo].[native_99]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_99 to nonadmin;	GRANT EXECUTE ON native_99 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_100] as begin 	exec dbo.native_100 end
GO
	CREATE OR ALTER   procedure [dbo].[native_100]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_100 to nonadmin;	GRANT EXECUTE ON native_100 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_101] as begin 	exec dbo.native_101 end
GO
	CREATE OR ALTER   procedure [dbo].[native_101]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_101 to nonadmin;	GRANT EXECUTE ON native_101 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_102] as begin 	exec dbo.native_102 end
GO
	CREATE OR ALTER   procedure [dbo].[native_102]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_102 to nonadmin;	GRANT EXECUTE ON native_102 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_103] as begin 	exec dbo.native_103 end
GO
	CREATE OR ALTER   procedure [dbo].[native_103]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_103 to nonadmin;	GRANT EXECUTE ON native_103 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_104] as begin 	exec dbo.native_104 end
GO
	CREATE OR ALTER   procedure [dbo].[native_104]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_104 to nonadmin;	GRANT EXECUTE ON native_104 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_105] as begin 	exec dbo.native_105 end
GO
	CREATE OR ALTER   procedure [dbo].[native_105]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_105 to nonadmin;	GRANT EXECUTE ON native_105 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_106] as begin 	exec dbo.native_106 end
GO
	CREATE OR ALTER   procedure [dbo].[native_106]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_106 to nonadmin;	GRANT EXECUTE ON native_106 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_107] as begin 	exec dbo.native_107 end
GO
	CREATE OR ALTER   procedure [dbo].[native_107]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_107 to nonadmin;	GRANT EXECUTE ON native_107 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_108] as begin 	exec dbo.native_108 end
GO
	CREATE OR ALTER   procedure [dbo].[native_108]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_108 to nonadmin;	GRANT EXECUTE ON native_108 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_109] as begin 	exec dbo.native_109 end
GO
	CREATE OR ALTER   procedure [dbo].[native_109]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_109 to nonadmin;	GRANT EXECUTE ON native_109 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_110] as begin 	exec dbo.native_110 end
GO
	CREATE OR ALTER   procedure [dbo].[native_110]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_110 to nonadmin;	GRANT EXECUTE ON native_110 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_111] as begin 	exec dbo.native_111 end
GO
	CREATE OR ALTER   procedure [dbo].[native_111]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_111 to nonadmin;	GRANT EXECUTE ON native_111 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_112] as begin 	exec dbo.native_112 end
GO
	CREATE OR ALTER   procedure [dbo].[native_112]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_112 to nonadmin;	GRANT EXECUTE ON native_112 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_113] as begin 	exec dbo.native_113 end
GO
	CREATE OR ALTER   procedure [dbo].[native_113]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_113 to nonadmin;	GRANT EXECUTE ON native_113 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_114] as begin 	exec dbo.native_114 end
GO
	CREATE OR ALTER   procedure [dbo].[native_114]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_114 to nonadmin;	GRANT EXECUTE ON native_114 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_115] as begin 	exec dbo.native_115 end
GO
	CREATE OR ALTER   procedure [dbo].[native_115]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_115 to nonadmin;	GRANT EXECUTE ON native_115 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_116] as begin 	exec dbo.native_116 end
GO
	CREATE OR ALTER   procedure [dbo].[native_116]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_116 to nonadmin;	GRANT EXECUTE ON native_116 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_117] as begin 	exec dbo.native_117 end
GO
	CREATE OR ALTER   procedure [dbo].[native_117]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_117 to nonadmin;	GRANT EXECUTE ON native_117 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_118] as begin 	exec dbo.native_118 end
GO
	CREATE OR ALTER   procedure [dbo].[native_118]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_118 to nonadmin;	GRANT EXECUTE ON native_118 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_119] as begin 	exec dbo.native_119 end
GO
	CREATE OR ALTER   procedure [dbo].[native_119]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_119 to nonadmin;	GRANT EXECUTE ON native_119 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_120] as begin 	exec dbo.native_120 end
GO
	CREATE OR ALTER   procedure [dbo].[native_120]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_120 to nonadmin;	GRANT EXECUTE ON native_120 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_121] as begin 	exec dbo.native_121 end
GO
	CREATE OR ALTER   procedure [dbo].[native_121]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_121 to nonadmin;	GRANT EXECUTE ON native_121 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_122] as begin 	exec dbo.native_122 end
GO
	CREATE OR ALTER   procedure [dbo].[native_122]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_122 to nonadmin;	GRANT EXECUTE ON native_122 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_123] as begin 	exec dbo.native_123 end
GO
	CREATE OR ALTER   procedure [dbo].[native_123]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_123 to nonadmin;	GRANT EXECUTE ON native_123 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_124] as begin 	exec dbo.native_124 end
GO
	CREATE OR ALTER   procedure [dbo].[native_124]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_124 to nonadmin;	GRANT EXECUTE ON native_124 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_125] as begin 	exec dbo.native_125 end
GO
	CREATE OR ALTER   procedure [dbo].[native_125]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_125 to nonadmin;	GRANT EXECUTE ON native_125 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_126] as begin 	exec dbo.native_126 end
GO
	CREATE OR ALTER   procedure [dbo].[native_126]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_126 to nonadmin;	GRANT EXECUTE ON native_126 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_127] as begin 	exec dbo.native_127 end
GO
	CREATE OR ALTER   procedure [dbo].[native_127]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_127 to nonadmin;	GRANT EXECUTE ON native_127 to nonadmin; 
GO

create or ALTER   procedure [dbo].[interop_128] as begin 	exec dbo.native_128 end
GO
	CREATE OR ALTER   procedure [dbo].[native_128]
with native_compilation, schemabinding, execute as owner
as  
begin atomic  
with (transaction isolation level=snapshot, language=N'us_english')  
  
  declare @i bigint

  insert dbo.memopt default values
  select @i = scope_identity()

  delete dbo.memopt where id = @i
  
end  
GO
	GRANT EXECUTE ON interop_128 to nonadmin;	GRANT EXECUTE ON native_128 to nonadmin; 
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