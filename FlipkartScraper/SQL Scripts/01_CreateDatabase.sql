-- =============================================
-- Database Creation Script
-- E-Commerce Website Data Scraping Assignment
-- =============================================

-- Create ECommerceMasterDB database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ECommerceMasterDB')
BEGIN
    CREATE DATABASE ECommerceMasterDB;
    PRINT 'Database ECommerceMasterDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database ECommerceMasterDB already exists.';
END

-- Use the created database
USE ECommerceMasterDB;
GO

-- Verify database creation
SELECT 
    name as 'Database Name',
    database_id as 'Database ID',
    create_date as 'Created Date'
FROM sys.databases 
WHERE name = 'ECommerceMasterDB';
GO
