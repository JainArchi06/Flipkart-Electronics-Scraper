# Flipkart Electronics Scraper

## Project Overview

A professional C# console application that performs web scraping of Flipkart's electronics category and stores the extracted data in a SQL Server database. This project demonstrates advanced web scraping techniques using Selenium WebDriver, database operations with ADO.NET, and follows enterprise-level coding practices.

## Assignment Requirements Compliance

### Part 1 - Database Design
- **Database**: `ECommerceMasterDB` - Complete
- **Table**: `Product Details` with all required columns - Complete
- **SQL Scripts**: Complete database creation and stored procedures - Complete
- **Data Types**: VARCHAR(100), VARCHAR(MAX), INT with IDENTITY - Complete

### Part 2 - C# Implementation
- **ADO.NET Connection**: Implemented with connection string management - Complete
- **Selenium WebDriver**: Chrome automation with multiple options - Complete
- **Flipkart Navigation**: Electronics category browsing - Complete
- **Data Extraction**: Product details scraping and database storage - Complete
- **Stored Procedures**: All database operations use stored procedures - Complete

## Architecture

### Project Structure
```
FlipkartScraper/
├── Models/
│   └── Product.cs                 # Data model for scraped products
├── Services/
│   ├── DatabaseService.cs        # ADO.NET database operations
│   └── ScrapingService.cs        # Selenium web scraping logic
├── Program.cs                     # Main application entry point
├── appsettings.json              # Configuration settings
├── FlipkartScraper.csproj        # Project dependencies
└── SQL Scripts/
    ├── 01_CreateDatabase.sql     # Database creation script
    ├── 02_CreateTable.sql        # Product table creation
    └── 03_StoredProcedures.sql   # Required stored procedures
```

## Features

### Web Scraping Capabilities
- **Multi-Strategy Navigation**: Direct search and category browsing
- **Robust Element Detection**: Multiple XPath selectors for reliability
- **Chrome Options**: Headless mode, custom user agent, performance optimizations
- **Error Handling**: Comprehensive exception handling and retry mechanisms
- **Rate Limiting**: Configurable delays between requests

### Database Operations
- **Stored Procedures**: All CRUD operations use stored procedures
- **Connection Management**: Proper connection lifecycle management
- **Parameterized Queries**: SQL injection prevention
- **Async Operations**: Non-blocking database operations

### Configuration Management
- **JSON Configuration**: Centralized settings management
- **Connection Strings**: Multiple authentication methods
- **Scraping Settings**: Configurable timeouts and limits

## Technology Stack

- **.NET 8.0**: Latest LTS framework
- **Selenium WebDriver 4.35.0**: Web automation
- **ChromeDriver 140.0.7339.18500**: Chrome browser automation
- **ADO.NET**: Database connectivity
- **SQL Server**: Database management system
- **Microsoft.Extensions.Configuration**: Configuration management

## Dependencies

```xml
<PackageReference Include="Selenium.WebDriver" Version="4.35.0" />
<PackageReference Include="Selenium.WebDriver.ChromeDriver" Version="140.0.7339.18500" />
<PackageReference Include="DotNetSeleniumExtras.WaitHelpers" Version="3.11.0" />
<PackageReference Include="System.Data.SqlClient" Version="4.9.0" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.9" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.9" />
```

## Database Schema

### Product Details Table
| Column Name | Data Type | Constraints |
|-------------|-----------|-------------|
| ProductID | INT | PRIMARY KEY, IDENTITY(1,1) |
| Name | VARCHAR(100) | NOT NULL |
| Price | VARCHAR(100) | NULL |
| Rating | VARCHAR(10) | NULL |
| Description | VARCHAR(MAX) | NULL |
| CreatedDate | DATETIME2 | DEFAULT GETDATE() |
| UpdatedDate | DATETIME2 | DEFAULT GETDATE() |

### Stored Procedures
1. **InsertProductData**: Insert new product with auto-generated ID
2. **GetAllProducts**: Retrieve all products
3. **GetProductById**: Retrieve specific product by ID

## Installation & Setup

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server (2019 or later)
- Chrome browser installed
- Visual Studio 2022 or VS Code

### Database Setup
1. **Create Database**:
   ```sql
   -- Run SQL Scripts in order
   -- 01_CreateDatabase.sql
   -- 02_CreateTable.sql
   -- 03_StoredProcedures.sql
   ```

2. **Configure Database Connection**:
   
   **Option A: Windows Authentication (Recommended)**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ECommerceMasterDB;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```
   
   **Option B: SQL Server Authentication**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ECommerceMasterDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=true;"
     }
   }
   ```

### Application Setup
1. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build Application**:
   ```bash
   dotnet build
   ```

3. **Run Application**:
   ```bash
   dotnet run
   ```

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ECommerceMasterDB;Trusted_Connection=true;TrustServerCertificate=true;",
    "SqlServerAuthentication": "Server=YOUR_SERVER_NAME;Database=ECommerceMasterDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=true;"
  },
  "ScrapingSettings": {
    "MaxProducts": 20,
    "DelayBetweenRequests": 2000,
    "TimeoutSeconds": 30,
    "HeadlessMode": true
  }
}
```

**Important Security Note**: 
- Replace `YOUR_SERVER_NAME`, `YOUR_USERNAME`, and `YOUR_PASSWORD` with your actual SQL Server credentials
- For production environments, use environment variables or Azure Key Vault for credential management

### Chrome Options
- **Headless Mode**: Configurable for debugging
- **User Agent**: Custom browser identification
- **Window Size**: 1920x1080 resolution
- **Performance**: Disabled extensions and plugins
- **Security**: Disabled notifications and geolocation

## Usage

### Basic Execution
```bash
dotnet run
```

### Expected Output
```
=== Flipkart Electronics Scraper ===
Starting scraping process...

Testing database connection...
Database connection successful!

Initializing web scraper...
Chrome driver initialized successfully.
Navigating to Flipkart...
Performed search for 'electronics'
Found 15 product containers using selector: //div[contains(@class,'slAVV4')]
Processing 15 products...
✓ Extracted: Samsung Galaxy S24 - ₹79,999
✓ Extracted: iPhone 15 Pro - ₹1,34,900
...

=== Scraping Complete ===
Total products found: 15
Successfully saved: 15
Failed to save: 0
```

## Key Features Implementation

### 1. Multi-Strategy Web Scraping
- **Direct Search**: Searches for "electronics" in Flipkart search box
- **Category Navigation**: Navigates through electronics category URLs
- **Fallback Mechanisms**: Multiple URL strategies for reliability

### 2. Robust Element Detection
- **Multiple Selectors**: Various XPath patterns for different page layouts
- **Dynamic Content**: Handles dynamic loading and AJAX content
- **Error Recovery**: Continues processing even if individual products fail

### 3. Database Integration
- **Stored Procedures**: All database operations use stored procedures
- **Connection Pooling**: Efficient connection management
- **Transaction Safety**: Proper error handling and rollback

### 4. Professional Error Handling
- **Try-Catch Blocks**: Comprehensive exception handling
- **Logging**: Detailed console output for debugging
- **Graceful Degradation**: Continues operation despite individual failures

## Data Extraction

### Product Information Captured
- **Product Name**: Primary product title
- **Price**: Current selling price
- **Rating**: Customer rating (if available)
- **Description**: Product features and specifications

### Sample Extracted Data
```json
{
  "ProductID": 1,
  "Name": "Samsung Galaxy S24 Ultra 5G",
  "Price": "₹1,24,999",
  "Rating": "4.5",
  "Description": "256GB Storage, 12GB RAM, Titanium Black",
  "CreatedDate": "2024-01-15T10:30:00",
  "UpdatedDate": "2024-01-15T10:30:00"
}
```

## Troubleshooting

### Common Issues

1. **Chrome Driver Issues**:
   - Ensure Chrome browser is installed
   - Check ChromeDriver version compatibility
   - Verify PATH environment variable

2. **Database Connection**:
   - Verify SQL Server is running
   - Check connection string format
   - Ensure database exists and is accessible

3. **Scraping Failures**:
   - Check internet connectivity
   - Verify Flipkart website accessibility
   - Review XPath selectors for page changes

### Debug Mode
To enable debug mode, modify `ScrapingService.cs`:
```csharp
// Comment out headless mode for visual debugging
// chromeOptions.AddArgument("--headless");
```

## Performance Considerations

- **Rate Limiting**: 3-second delays between actions
- **Product Limit**: Maximum 20 products per run
- **Memory Management**: Proper disposal of WebDriver resources
- **Connection Pooling**: Efficient database connection usage

## Security Features

- **SQL Injection Prevention**: Parameterized queries
- **Input Validation**: Data sanitization before database insertion
- **Connection Security**: Trusted connection with certificate validation
- **Error Information**: Limited sensitive information in error messages
- **Credential Protection**: No hardcoded database credentials in source code
- **Configuration Management**: Environment-based configuration support

## Assignment Compliance Summary

| Requirement | Status | Implementation |
|-------------|--------|----------------|
| Database Creation | Complete | ECommerceMasterDB with Product Details table |
| Table Schema | Complete | All required columns with proper data types |
| ADO.NET Connection | Complete | DatabaseService class with connection management |
| Selenium WebDriver | Complete | Chrome automation with multiple options |
| Flipkart Navigation | Complete | Electronics category browsing |
| Data Extraction | Complete | Product details scraping and storage |
| Stored Procedures | Complete | All database operations use stored procedures |
| Error Handling | Complete | Comprehensive exception handling |
| Configuration | Complete | JSON-based configuration management |

## Development

This project demonstrates professional C# development practices including:
- Clean architecture and separation of concerns
- Comprehensive error handling and logging
- Modern async/await patterns
- Professional documentation standards

## License

This project is created for educational purposes as part of an assignment.

## Author

**Assignment Project** - E-Commerce Website Data Scraping
- **Technology**: C# .NET 8.0, Selenium WebDriver, SQL Server
- **Purpose**: Academic assignment demonstrating web scraping and database integration

---

*This README provides comprehensive documentation for the Flipkart Electronics Scraper project, ensuring all assignment requirements are met and properly documented.*