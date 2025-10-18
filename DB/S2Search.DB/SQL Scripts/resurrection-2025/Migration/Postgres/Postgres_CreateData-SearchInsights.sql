DO $$
DECLARE
    start_date date := '2020-02-01';
    end_date date := '2025-12-31';
    total_days int := (end_date - start_date);

    --random_value_start_number int := 1;
    --random_value_end_number int := 999;

    time_of_day_start_number int := 0;
    time_of_day_end_number int := 23;

    search_index_row record;
    day_idx int;
    date_for_data date;
    current_day_of_month int;
    random_number int;
    current_data record;
    count_for_data int;
    rv_start int;
    rv_end int;
BEGIN
    -- Clear down the tables
    TRUNCATE TABLE search_insights_data;
    TRUNCATE TABLE search_index_request_log;
    
    -- Temporary table for data categories
    CREATE TEMP TABLE tmp_data_categories (
        id serial PRIMARY KEY,
        data_category text,
        data_point text
    ) ON COMMIT DROP;

    -- Insert fixed data categories
    INSERT INTO tmp_data_categories (data_category, data_point)
    VALUES
    ('Search Type','Text & Filter Searches'),
    ('Search Type','Text Searches'),
    ('Search Type','Filter Searches'),
    ('Search Type','No Text or Filter Searches'),

    ('Text & Filter Searches','queryText && field eq ''Value'''),
    ('Text Searches','queryText'),
    ('Filter Searches','field eq ''Value'''),
    ('Results Without Text or Filters','No Text or Filter Searches'),
    ('Zero Results Text & Filter Searches','queryText && field eq ''Value'''),
    ('Zero Results Text Searches','queryText'),
    ('Zero Results Filter Searches','field eq ''Value'''),
    ('Zero Results Without Text Or Filters','No Text or Filter Searches'),

    ('Time of Day (Part)','Morning'),
    ('Time of Day (Part)','Afternoon'),
    ('Time of Day (Part)','Evening'),
    ('Time of Day (Part)','Night'),

    ('Order By','price desc'),
    ('Order By','price asc'),
    ('Order By','monthlyPrice desc'),
    ('Order By','monthlyPrice asc'),
    ('Order By','year desc'),
    ('Order By','year asc'),

    ('Make','Alfa Romeo'),
    ('Make','Aston Martin'),
    ('Make','Audi'),
    ('Make','Bentley'),
    ('Make','BMW'),
    ('Make','Citroen'),
    ('Make','Dodge'),
    ('Make','Ferrari'),
    ('Make','Ford'),
    ('Make','Honda'),
    ('Make','Kia'),
    ('Make','Lexus'),
    ('Make','Nissan'),

    ('Model','2 Series'),
    ('Model','3 Series'),
    ('Model','350Z'),
    ('Model','5 Series'),
    ('Model','599'),
    ('Model','A5'),
    ('Model','A3'),
    ('Model','A6'),
    ('Model','A7'),
    ('Model','B-Max'),
    ('Model','Brera'),
    ('Model','C3'),
    ('Model','C4 Grand Picasso'),
    ('Model','California'),
    ('Model','Capri'),
    ('Model','Ceed'),
    ('Model','Civic'),
    ('Model','Continental'),
    ('Model','CR-V'),
    ('Model','DB9'),
    ('Model','Cube'),
    ('Model','Fiesta'),
    ('Model','Escort'),
    ('Model','Focus'),
    ('Model','Juke'),

    ('Price','501-1001'),
    ('Price','1001-2001'),
    ('Price','2001-3001'),
    ('Price','3001-4001'),
    ('Price','4001-5001'),
    ('Price','5001-6001'),
    ('Price','6001-7001'),
    ('Price','7001-8001'),
    ('Price','8001-9001'),
    ('Price','9001-10001'),
    ('Price','10001-11001'),
    ('Price','11001-12001'),
    ('Price','12001-13001'),

    ('Location','Aberdeen'),
    ('Location','Bedford'),
    ('Location','Boston'),
    ('Location','Bromley'),
    ('Location','Bristol'),
    ('Location','Romford'),
    ('Location','St Helens'),
    ('Location','Stansted'),
    ('Location','Sheffield'),

    ('Year','1972'),
    ('Year','1974'),
    ('Year','1999'),
    ('Year','2001'),
    ('Year','2002'),
    ('Year','2003'),
    ('Year','2004'),
    ('Year','2005'),
    ('Year','2006'),
    ('Year','2007'),
    ('Year','2008'),
    ('Year','2011'),
    ('Year','2012'),
    ('Year','2017'),
    ('Year','2018'),
    ('Year','2019'),
    ('Year','2020'),
    ('Year','2021'),

    ('Mileage','0-1000'),
    ('Mileage','1000-5000'),
    ('Mileage','5000-10000'),
    ('Mileage','10000-15000'),
    ('Mileage','15000-20000'),
    ('Mileage','20000-25000'),

    ('Transmission','Automatic'),
    ('Transmission','Manual'),

    ('Fuel Type','Diesel'),
    ('Fuel Type','Electric'),
    ('Fuel Type','Hybrid'),
    ('Fuel Type','Petrol'),

    ('Body Type','Convertible'),
    ('Body Type','Coupe'),
    ('Body Type','Estate'),
    ('Body Type','Hatchback'),
    ('Body Type','MPV'),
    ('Body Type','Pick Up'),
    ('Body Type','Saloon'),
    ('Body Type','SUV'),

    ('Engine Size','0L'),
    ('Engine Size','1.0L'),
    ('Engine Size','1.1L'),
    ('Engine Size','1.2L'),
    ('Engine Size','1.3L'),
    ('Engine Size','1.4L'),
    ('Engine Size','1.5L'),
    ('Engine Size','1.6L'),
    ('Engine Size','1.7L'),
    ('Engine Size','1.8L'),
    ('Engine Size','1.9L'),
    ('Engine Size','2.0L'),
    ('Engine Size','2.1L'),
    ('Engine Size','2.2L'),
    ('Engine Size','2.3L'),
    ('Engine Size','2.4L'),
    ('Engine Size','2.5L'),
    ('Engine Size','2.6L'),
    ('Engine Size','2.7L'),
    ('Engine Size','2.8L'),
    ('Engine Size','2.9L'),

    ('Doors','2'),
    ('Doors','3'),
    ('Doors','4'),
    ('Doors','5'),
    ('Doors','6'),

    ('Colour','Black'),
    ('Colour','Blue'),
    ('Colour','Bronze'),
    ('Colour','Grey'),
    ('Colour','Orange'),
    ('Colour','Red'),
    ('Colour','Silver'),
    ('Colour','White');

    -- Time of day hours
    FOR day_idx IN time_of_day_start_number..time_of_day_end_number LOOP
        INSERT INTO tmp_data_categories (data_category, data_point)
        VALUES ('Time of Day (Hour)', day_idx::text);
    END LOOP;

    -- Temporary table for search index ids
    CREATE TEMP TABLE tmp_search_index_ids (
        id serial PRIMARY KEY,
        search_index_id uuid
    ) ON COMMIT DROP;

    INSERT INTO tmp_search_index_ids (search_index_id)
    VALUES ('8c663063-4217-4f54-973f-8faec6131b5b');

    -- Iterate search indexes
    FOR search_index_row IN SELECT search_index_id FROM tmp_search_index_ids LOOP
        -- iterate days
        FOR day_idx IN 1..total_days LOOP
            date_for_data := start_date + day_idx;
            current_day_of_month := extract(day from date_for_data)::int;

            random_number := (floor(random()*10) + 1)::int; -- 1..10

            -- determine random value start/end based on day of month
            rv_start := CASE
                WHEN current_day_of_month BETWEEN 1 AND 5 THEN 500
                WHEN current_day_of_month BETWEEN 6 AND 10 THEN 1000
                WHEN current_day_of_month BETWEEN 11 AND 20 THEN 500
                ELSE 100
            END;

            rv_end := CASE
                WHEN current_day_of_month BETWEEN 1 AND 5 THEN random_number * 1000
                WHEN current_day_of_month BETWEEN 6 AND 10 THEN random_number * 1500
                WHEN current_day_of_month BETWEEN 11 AND 20 THEN random_number * 1250
                ELSE 15000
            END;

            -- iterate categories
            FOR current_data IN SELECT data_category, data_point FROM tmp_data_categories LOOP
                count_for_data := GREATEST(0, floor(random() * (rv_end - rv_start + 1))::int + rv_start);

                INSERT INTO search_insights_data
                    (search_index_id, data_category, data_point, count, date)
                VALUES
                    (search_index_row.search_index_id, current_data.data_category, current_data.data_point, count_for_data, date_for_data);
            END LOOP;
        END LOOP;
    END LOOP;

    -- Populate search index request log with daily max
    INSERT INTO search_index_request_log (search_index_id, count, date)
    SELECT
        sid.search_index_id,
        MAX(sid.count) as max_count,
        sid.date
    FROM search_insights_data sid
    JOIN tmp_search_index_ids si ON si.search_index_id = sid.search_index_id
    GROUP BY sid.search_index_id, sid.date;

END $$ LANGUAGE plpgsql;