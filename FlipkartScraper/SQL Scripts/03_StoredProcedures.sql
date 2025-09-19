-- =============================================
-- Stored Procedures Creation Script
-- E-Commerce Website Data Scraping Assignment
-- =============================================

-- Use ECommerceMasterDB database
USE ECommerceMasterDB;
GO

-- =============================================
-- Stored Procedure: InsertProductData
-- Purpose: Insert new product data and return ProductID
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertProductData]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[InsertProductData];
    PRINT 'Existing InsertProductData stored procedure dropped.';
END
GO

CREATE PROCEDURE [dbo].[InsertProductData]
    @Name VARCHAR(100),
    @Price VARCHAR(100) = NULL,
    @Rating VARCHAR(10) = NULL,
    @Description VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Insert product data
        INSERT INTO [dbo].[ProductDetails] 
        (
            [Name], 
            [Price], 
            [Rating], 
            [Description], 
            [CreatedDate], 
            [UpdatedDate]
        )
        VALUES 
        (
            @Name, 
            @Price, 
            @Rating, 
            @Description, 
            GETDATE(), 
            GETDATE()
        );
        
        -- Return the new ProductID
        SELECT SCOPE_IDENTITY() AS NewProductID;
        
        PRINT 'Product inserted successfully.';
    END TRY
    BEGIN CATCH
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        PRINT 'Error inserting product: ' + @ErrorMessage;
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

-- =============================================
-- Stored Procedure: GetAllProducts
-- Purpose: Retrieve all products from ProductDetails table
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllProducts]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[GetAllProducts];
    PRINT 'Existing GetAllProducts stored procedure dropped.';
END
GO

CREATE PROCEDURE [dbo].[GetAllProducts]
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SELECT 
            [ProductID],
            [Name],
            [Price],
            [Rating],
            [Description],
            [CreatedDate],
            [UpdatedDate]
        FROM [dbo].[ProductDetails]
        ORDER BY [CreatedDate] DESC;
        
        PRINT 'All products retrieved successfully.';
    END TRY
    BEGIN CATCH
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        PRINT 'Error retrieving products: ' + @ErrorMessage;
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

-- =============================================
-- Stored Procedure: GetProductById
-- Purpose: Retrieve specific product by ProductID
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProductById]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[GetProductById];
    PRINT 'Existing GetProductById stored procedure dropped.';
END
GO

CREATE PROCEDURE [dbo].[GetProductById]
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SELECT 
            [ProductID],
            [Name],
            [Price],
            [Rating],
            [Description],
            [CreatedDate],
            [UpdatedDate]
        FROM [dbo].[ProductDetails]
        WHERE [ProductID] = @ProductID;
        
        IF @@ROWCOUNT = 0
        BEGIN
            PRINT 'No product found with ProductID: ' + CAST(@ProductID AS VARCHAR(10));
        END
        ELSE
        BEGIN
            PRINT 'Product retrieved successfully.';
        END
    END TRY
    BEGIN CATCH
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        PRINT 'Error retrieving product: ' + @ErrorMessage;
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

-- =============================================
-- Stored Procedure: UpdateProduct
-- Purpose: Update existing product data
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateProduct]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[UpdateProduct];
    PRINT 'Existing UpdateProduct stored procedure dropped.';
END
GO

CREATE PROCEDURE [dbo].[UpdateProduct]
    @ProductID INT,
    @Name VARCHAR(100),
    @Price VARCHAR(100) = NULL,
    @Rating VARCHAR(10) = NULL,
    @Description VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE [dbo].[ProductDetails]
        SET 
            [Name] = @Name,
            [Price] = @Price,
            [Rating] = @Rating,
            [Description] = @Description,
            [UpdatedDate] = GETDATE()
        WHERE [ProductID] = @ProductID;
        
        IF @@ROWCOUNT = 0
        BEGIN
            PRINT 'No product found with ProductID: ' + CAST(@ProductID AS VARCHAR(10));
        END
        ELSE
        BEGIN
            PRINT 'Product updated successfully.';
        END
    END TRY
    BEGIN CATCH
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        PRINT 'Error updating product: ' + @ErrorMessage;
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

-- =============================================
-- Stored Procedure: DeleteProduct
-- Purpose: Delete product by ProductID
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteProduct]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[DeleteProduct];
    PRINT 'Existing DeleteProduct stored procedure dropped.';
END
GO

CREATE PROCEDURE [dbo].[DeleteProduct]
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        DELETE FROM [dbo].[ProductDetails]
        WHERE [ProductID] = @ProductID;
        
        IF @@ROWCOUNT = 0
        BEGIN
            PRINT 'No product found with ProductID: ' + CAST(@ProductID AS VARCHAR(10));
        END
        ELSE
        BEGIN
            PRINT 'Product deleted successfully.';
        END
    END TRY
    BEGIN CATCH
        -- Error handling
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        PRINT 'Error deleting product: ' + @ErrorMessage;
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

-- =============================================
-- Verification: List all created stored procedures
-- =============================================
SELECT 
    ROUTINE_NAME as 'Stored Procedure Name',
    ROUTINE_TYPE as 'Type',
    CREATED as 'Created Date'
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = 'dbo' 
    AND ROUTINE_TYPE = 'PROCEDURE'
    AND ROUTINE_NAME IN ('InsertProductData', 'GetAllProducts', 'GetProductById', 'UpdateProduct', 'DeleteProduct')
ORDER BY ROUTINE_NAME;
GO

PRINT 'All stored procedures created successfully.';
PRINT 'Available procedures: InsertProductData, GetAllProducts, GetProductById, UpdateProduct, DeleteProduct';
GO
