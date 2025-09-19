-- =============================================
-- Table Creation Script
-- E-Commerce Website Data Scraping Assignment
-- =============================================

-- Use ECommerceMasterDB database
USE ECommerceMasterDB;
GO

-- Drop table if exists (for clean setup)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductDetails]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[ProductDetails];
    PRINT 'Existing ProductDetails table dropped.';
END
GO

-- Create ProductDetails table as per assignment requirements
CREATE TABLE [dbo].[ProductDetails] (
    [ProductID] INT IDENTITY(1,1) NOT NULL,
    [Name] VARCHAR(100) NOT NULL,
    [Price] VARCHAR(100) NULL,
    [Rating] VARCHAR(10) NULL,
    [Description] VARCHAR(MAX) NULL,
    [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    
    -- Primary Key Constraint
    CONSTRAINT [PK_ProductDetails] PRIMARY KEY CLUSTERED ([ProductID] ASC)
);
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX [IX_ProductDetails_Name] 
ON [dbo].[ProductDetails] ([Name] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_ProductDetails_CreatedDate] 
ON [dbo].[ProductDetails] ([CreatedDate] ASC);
GO

-- Verify table creation
SELECT 
    TABLE_NAME as 'Table Name',
    COLUMN_NAME as 'Column Name',
    DATA_TYPE as 'Data Type',
    CHARACTER_MAXIMUM_LENGTH as 'Max Length',
    IS_NULLABLE as 'Nullable',
    COLUMN_DEFAULT as 'Default Value'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'ProductDetails'
ORDER BY ORDINAL_POSITION;
GO

PRINT 'ProductDetails table created successfully with all required columns.';
PRINT 'Table includes: ProductID (PK, IDENTITY), Name, Price, Rating, Description, CreatedDate, UpdatedDate';
GO
