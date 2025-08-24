use CustomerResourceStore
go
/* #####################################################
	Use this script to generate a list of notifications
   ##################################################### */ 

declare @searchIndexId uniqueidentifier = 'Insert Search Index Id Here'
declare @MaxLoops int = 1 --How many notifications to generate

declare @LoopCount int = 0
while (@LoopCount < @MaxLoops)
begin
	declare @recipients varchar(500) = 'test' + CAST(@LoopCount as varchar(10)) + '@test.test'
	declare @event varchar(100) = 'Feed ' + case when case when RAND() > 0.5 then 1 else 0 end = 1 then 'Success' else 'Failure' end
	declare @transmitType varchar(100) = 'System'
	declare @endDate datetime = GETUTCDATE()
	declare @startDate datetime = DATEADD(day, -30, @endDate)

	DECLARE @Seconds INT = DATEDIFF(SECOND, @startDate, @endDate)
	DECLARE @Random INT = ROUND(((@Seconds-1) * RAND()), 0)

	declare @RandomDate datetime = DATEADD(SECOND, @Random, @startDate)

	INSERT INTO dbo.Notifications
	(
	SearchIndexId,
	Recipients,
	[Event],
	TransmitType,
	CreatedDate
	)
	SELECT
	@searchIndexId,
	@recipients,
	@event,
	@transmitType,
	@RandomDate

	set @LoopCount = @LoopCount + 1
end