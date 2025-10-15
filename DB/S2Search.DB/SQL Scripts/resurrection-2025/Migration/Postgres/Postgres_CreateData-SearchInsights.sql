USE [CustomerResourceStore]
GO
/* ########################################################
	Use this script to generate some sample Search Insights
   ######################################################## */ 

declare @startDate date = '2020-02-01'
declare @endDate date = '2022-02-08'

declare @totalDays int = DATEDIFF(day, @startDate, @endDate)

declare @randomValueStartNumber int = 1
declare @randomValueEndNumber int = 999

declare @dataCategories table 
(
	Id int identity(1,1),
	DataCategory varchar(50),
	DataPoint varchar(1000)
)

INSERT INTO @dataCategories
( DataCategory, DataPoint)
VALUES
--Fixed Data Categories
( 'Search Type', 'Text & Filter Searches' ),
( 'Search Type', 'Text Searches' ),
( 'Search Type', 'Filter Searches' ),
( 'Search Type', 'No Text or Filter Searches' ),

( 'Text & Filter Searches', 'queryText && field eq ''Value''' ),
( 'Text Searches', 'queryText' ),
( 'Filter Searches', 'field eq ''Value''' ),
( 'Results Without Text or Filters', 'No Text or Filter Searches' ),
( 'Zero Results Text & Filter Searches', 'queryText && field eq ''Value''' ),
( 'Zero Results Text Searches', 'queryText' ),
( 'Zero Results Filter Searches', 'field eq ''Value''' ),
( 'Zero Results Without Text Or Filters', 'No Text or Filter Searches' ),

( 'Time of Day (Part)', 'Morning' ),
( 'Time of Day (Part)', 'Afternoon' ),
( 'Time of Day (Part)', 'Evening' ),
( 'Time of Day (Part)', 'Night' ),

( 'Order By', 'price desc' ),
( 'Order By', 'price asc' ),
( 'Order By', 'monthlyPrice desc' ),
( 'Order By', 'monthlyPrice asc' ),
( 'Order By', 'year desc' ),
( 'Order By', 'year asc' ),

--Facets
( 'Make', 'Alfa Romeo' ),
( 'Make', 'Aston Martin' ),
( 'Make', 'Audi' ),
( 'Make', 'Bentley' ),
( 'Make', 'BMW' ),
( 'Make', 'Citroen' ),
( 'Make', 'Dodge' ),
( 'Make', 'Ferrari' ),
( 'Make', 'Ford' ),
( 'Make', 'Honda' ),
( 'Make', 'Kia' ),
( 'Make', 'Lexus' ),
( 'Make', 'Nissan' ),

( 'Model', '2 Series' ),
( 'Model', '3 Series' ),
( 'Model', '350Z' ),
( 'Model', '5 Series' ),
( 'Model', '599' ),
( 'Model', 'A5' ),
( 'Model', 'A3' ),
( 'Model', 'A6' ),
( 'Model', 'A7' ),
( 'Model', 'B-Max' ),
( 'Model', 'Brera' ),
( 'Model', 'C3' ),
( 'Model', 'C4 Grand Picasso' ),
( 'Model', 'California' ),
( 'Model', 'Capri' ),
( 'Model', 'Ceed' ),
( 'Model', 'Civic' ),
( 'Model', 'Continental' ),
( 'Model', 'CR-V' ),
( 'Model', 'DB9' ),
( 'Model', 'Cube' ),
( 'Model', 'Civic' ),
( 'Model', 'Fiesta' ),
( 'Model', 'Escort' ),
( 'Model', 'Focus' ),
( 'Model', 'Juke' ),

( 'Price', '501-1001' ),
( 'Price', '1001-2001' ),
( 'Price', '2001-3001' ),
( 'Price', '3001-4001' ),
( 'Price', '4001-5001' ),
( 'Price', '5001-6001' ),
( 'Price', '6001-7001' ),
( 'Price', '7001-8001' ),
( 'Price', '8001-9001' ),
( 'Price', '9001-10001' ),
( 'Price', '10001-11001' ),
( 'Price', '11001-12001' ),
( 'Price', '12001-13001' ),

( 'Location', 'Aberdeen' ),
( 'Location', 'Bedford' ),
( 'Location', 'Boston' ),
( 'Location', 'Bromley' ),
( 'Location', 'Bristol' ),
( 'Location', 'Romford' ),
( 'Location', 'St Helens' ),
( 'Location', 'Stansted' ),
( 'Location', 'Sheffield' ),

( 'Year', '1972' ),
( 'Year', '1974' ),
( 'Year', '1999' ),
( 'Year', '2001' ),
( 'Year', '2002' ),
( 'Year', '2003' ),
( 'Year', '2004' ),
( 'Year', '2005' ),
( 'Year', '2006' ),
( 'Year', '2007' ),
( 'Year', '2008' ),
( 'Year', '2011' ),
( 'Year', '2012' ),
( 'Year', '2017' ),
( 'Year', '2018' ),
( 'Year', '2019' ),
( 'Year', '2020' ),
( 'Year', '2021' ),

( 'Mileage', '0-1000' ),
( 'Mileage', '1000-5000' ),
( 'Mileage', '5000-10000' ),
( 'Mileage', '10000-15000' ),
( 'Mileage', '15000-20000' ),
( 'Mileage', '20000-25000' ),

( 'Transmission', 'Automatic' ),
( 'Transmission', 'Manual' ),

( 'Fuel Type', 'Diesel' ),
( 'Fuel Type', 'Electric' ),
( 'Fuel Type', 'Hybrid' ),
( 'Fuel Type', 'Petrol' ),

( 'Body Type', 'Convertible' ),
( 'Body Type', 'Coupe' ),
( 'Body Type', 'Estate' ),
( 'Body Type', 'Hatchback' ),
( 'Body Type', 'MPV' ),
( 'Body Type', 'Pick Up' ),
( 'Body Type', 'Saloon' ),
( 'Body Type', 'SUV' ),

( 'Engine Size', '0L' ),
( 'Engine Size', '1.0L' ),
( 'Engine Size', '1.1L' ),
( 'Engine Size', '1.2L' ),
( 'Engine Size', '1.3L' ),
( 'Engine Size', '1.4L' ),
( 'Engine Size', '1.5L' ),
( 'Engine Size', '1.6L' ),
( 'Engine Size', '1.7L' ),
( 'Engine Size', '1.8L' ),
( 'Engine Size', '1.9L' ),
( 'Engine Size', '2.0L' ),
( 'Engine Size', '2.1L' ),
( 'Engine Size', '2.2L' ),
( 'Engine Size', '2.3L' ),
( 'Engine Size', '2.4L' ),
( 'Engine Size', '2.5L' ),
( 'Engine Size', '2.6L' ),
( 'Engine Size', '2.7L' ),
( 'Engine Size', '2.8L' ),
( 'Engine Size', '2.9L' ),

( 'Doors', '2' ),
( 'Doors', '3' ),
( 'Doors', '4' ),
( 'Doors', '5' ),
( 'Doors', '6' ),

( 'Colour', 'Black' ),
( 'Colour', 'Blue' ),
( 'Colour', 'Bronze' ),
( 'Colour', 'Grey' ),
( 'Colour', 'Orange' ),
( 'Colour', 'Red' ),
( 'Colour', 'Silver' ),
( 'Colour', 'White' )

declare @timeOfDayStartNumber int = 0
declare @timeOfDayEndNumber int = 23

declare @timeOfDayHourCategories varchar(20) = 'Time of Day (Hour)'

WHILE @timeOfDayStartNumber <= @timeOfDayEndNumber
BEGIN
	INSERT INTO @dataCategories
	( DataCategory, DataPoint)
	VALUES
	( @timeOfDayHourCategories, CAST(@timeOfDayStartNumber as varchar(10)) )

	set @timeOfDayStartNumber = @timeOfDayStartNumber + 1
END

declare @searchIndexIds table
(
	Id int identity(1,1),
	SearchIndexId uniqueidentifier
)

insert into @searchIndexIds
( SearchIndexId )
VALUES
( '8c663063-4217-4f54-973f-8faec6131b5b' )

declare @currentSearchIndexId int = 1
declare @lastSearchIndexId int = (SELECT MAX(Id) FROM @searchIndexIds)

WHILE @currentSearchIndexId <= @lastSearchIndexId
BEGIN
	declare @searchIndexId uniqueidentifier = (SELECT SearchIndexId FROM @searchIndexIds WHERE Id = @currentSearchIndexId)
	declare @currentDay int = 1;

	set @randomValueStartNumber = 999
	set @randomValueEndNumber = 10001

	while @currentDay <= @totalDays
	begin
		declare @dateForData date = DATEADD(day, @currentDay, @startDate)
		declare @currentDayOfMonth int = DATENAME(DAY, @dateForData)
		
		declare @randomNumber int = ROUND(RAND()* (10-1)+1, 0)

		set @randomValueStartNumber =	CASE WHEN @currentDayOfMonth BETWEEN 0 AND 5 THEN 500
											 WHEN @currentDayOfMonth BETWEEN 5 AND 10 THEN 1000
											 WHEN @currentDayOfMonth BETWEEN 10 AND 20 THEN 500
											 ELSE 100
											 END

		set @randomValueEndNumber =		CASE WHEN @currentDayOfMonth BETWEEN 0 AND 5 THEN @randomNumber * 1000
											 WHEN @currentDayOfMonth BETWEEN 5 AND 10 THEN @randomNumber * 1500
											 WHEN @currentDayOfMonth BETWEEN 10 AND 20 THEN @randomNumber * 1250
											 ELSE 15000
											 END

		declare @currentDataCategoryId int = 1;
		declare @lastDataCategoryId int = (SELECT MAX(Id) FROM @dataCategories);

		while @currentDataCategoryId <= @lastDataCategoryId
		begin
			declare @countForData int = (FLOOR(RAND()*(@randomValueEndNumber-@randomValueStartNumber)));

			INSERT INTO insights.SearchInsightsData
			( SearchIndexId, DataCategory, DataPoint, [Count], [Date] )
			SELECT 
			@searchIndexId,
			DataCategory,
			DataPoint,
			@countForData,
			@dateForData
			FROM @dataCategories
			WHERE Id = @currentDataCategoryId

			set @currentDataCategoryId = @currentDataCategoryId + 1;
		end

		set @currentDay = @currentDay + 1
	end

	set @currentSearchIndexId = @currentSearchIndexId + 1
END

insert into insights.SearchIndexRequestLog
(SearchIndexId, [Count], [Date])
SELECT 
sid.SearchIndexId,
MAX(sid.[Count]) as MaxCount,
sid.[Date]
FROM [CustomerResourceStore].[insights].[SearchInsightsData] sid
INNER JOIN @searchIndexIds si on si.SearchIndexId = sid.SearchIndexId
GROUP BY sid.SearchIndexId, sid.[Date]