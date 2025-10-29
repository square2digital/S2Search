# S2Search Database Project

Enterprise-grade database solution for S2Search platform with multi-database support for both SQL Azure and PostgreSQL. This project provides a comprehensive data layer supporting search operations, feed management, customer configuration, and analytics.

## 🚀 Overview

The S2Search database is the foundational data layer that powers the entire search platform, providing:

- **Multi-Database Support** - Native compatibility with both SQL Azure and PostgreSQL
- **Microservice Integration** - Dedicated schemas for each service component
- **Feed Management** - Complete data pipeline for feed processing and indexing
- **Search Analytics** - Comprehensive search insights and business intelligence
- **Customer Management** - Multi-tenant customer configuration and theming
- **Security Framework** - Role-based access control with service-specific users

## 📊 Database Architecture

### Supported Database Platforms

#### Microsoft SQL Azure

- **Version**: Azure SQL Database v12+
- **Collation**: SQL_Latin1_General_CP1_CI_AS
- **Features**: Full Azure SQL Database feature set
- **Scalability**: Automatic scaling and performance tuning
- **Backup**: Built-in automated backups and point-in-time recovery

#### PostgreSQL

- **Version**: PostgreSQL 12+
- **Extensions**: Full-text search, UUID, JSONB support
- **Features**: Advanced indexing and query optimization
- **Scalability**: Horizontal scaling with read replicas
- **Backup**: Continuous archiving and point-in-time recovery

## 📁 Project Structure

```
S2Search.DB/
├── S2Search.DB.sln                    # 📋 Visual Studio solution file
├── S2Search.DB/                       # 🗄️ Main database project
│   ├── S2Search.DB.Azure.sqlproj      # SQL Server Database Project
│   ├── Schemas.sql                    # Schema definitions
│   ├── logins.sql                     # Login and authentication setup
│   ├── dbo/                          # Default schema objects
│   │   └── Tables/                   # Core business tables
│   │       ├── Customers.sql         # Customer management
│   │       ├── Feeds.sql             # Feed configuration
│   │       ├── SearchIndex.sql       # Search index management
│   │       ├── SearchInsightsData.sql # Analytics data
│   │       ├── Themes.sql            # UI theming configuration
│   │       └── *.sql                 # Additional core tables
│   ├── FeedServicesFunc/             # Feed processing schema
│   │   ├── Stored Procedures/        # Feed-related procedures
│   │   └── User Defined Types/       # Custom data types
│   ├── SearchInsightsFunc/           # Analytics schema
│   │   └── Stored Procedures/        # Analytics procedures
│   ├── SFTPGoServicesFunc/           # SFTP service schema
│   │   └── Stored Procedures/        # File transfer procedures
│   ├── Configuration/                # Configuration schema
│   │   └── Stored Procedures/        # Configuration management
│   ├── Admin/                        # Administrative schema
│   │   └── Stored Procedures/        # Admin operations
│   ├── Security/                     # Database security
│   │   ├── FeedServicesFunc.sql      # Feed service user
│   │   ├── SearchInsightsFunc.sql    # Analytics user
│   │   ├── SFTPGoServicesFunc.sql    # SFTP service user
│   │   └── *.sql                     # Additional service users
│   └── SQL Scripts/                  # Utility and migration scripts
│       ├── GetAllCustomerData.sql    # Data extraction scripts
│       ├── Legacy/                   # Legacy migration scripts
│       └── resurrection-2025/        # Platform modernization scripts
```

## 🏗️ Schema Architecture

### Core Schemas

#### `dbo` - Core Business Objects

- **Purpose**: Primary business entities and core functionality
- **Tables**: Customers, Feeds, SearchIndex, Themes, SearchInsightsData
- **Access**: Shared across all application services

#### `FeedServicesFunc` - Feed Processing

- **Purpose**: Data feed ingestion, processing, and validation
- **Integration**: Azure Functions for feed processing
- **Operations**: Feed scheduling, document management, indexing
- **Security**: Dedicated service user with schema-specific permissions

#### `SearchInsightsFunc` - Analytics & Reporting

- **Purpose**: Search analytics, user behavior tracking, business intelligence
- **Integration**: Azure Functions for real-time analytics processing
- **Operations**: Search tracking, performance metrics, usage reports
- **Security**: Analytics-specific user with read/write access to insights data

#### `SFTPGoServicesFunc` - File Transfer Services

- **Purpose**: Secure file transfer operations and SFTP management
- **Integration**: SFTPGo service integration
- **Operations**: File management, user provisioning, access control
- **Security**: SFTP service user with file operation permissions

#### `Configuration` - System Configuration

- **Purpose**: Customer-specific configuration and theme management
- **Operations**: Theme retrieval, search configuration, customer settings
- **Security**: Configuration-specific access controls

#### `Admin` - Administrative Operations

- **Purpose**: Platform administration and management operations
- **Operations**: Customer management, system monitoring, maintenance
- **Security**: Administrative access controls and audit trails

## 🗃️ Core Tables

### Customer Management

#### `Customers`

```sql
CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    BusinessName VARCHAR(100),
    CustomerEndpoint VARCHAR(100),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME
);
```

- **Purpose**: Multi-tenant customer management
- **Features**: Unique customer endpoints, business information
- **Integration**: All services reference customer configuration

#### `Themes`

```sql
CREATE TABLE Themes (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    PrimaryColour VARCHAR(50),
    SecondaryColour VARCHAR(50),
    LogoUrl VARCHAR(500),
    CreatedDate DATETIME DEFAULT GETUTCDATE()
);
```

- **Purpose**: Customer-specific UI theming and branding
- **Features**: Color schemes, logo management, brand customization

### Feed Management

#### `Feeds`

```sql
CREATE TABLE Feeds (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FeedType VARCHAR(20) NOT NULL,
    FeedScheduleCron VARCHAR(255) NOT NULL,
    SearchIndexId UNIQUEIDENTIFIER NOT NULL,
    DataFormat VARCHAR(50) NOT NULL,
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    SupersededDate DATETIME,
    IsLatest BIT DEFAULT 1
);
```

- **Purpose**: Feed configuration and scheduling
- **Features**: CRON scheduling, format management, versioning
- **Integration**: Azure Functions feed processing pipeline

#### `FeedCurrentDocuments`

```sql
CREATE TABLE FeedCurrentDocuments (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SearchIndexId UNIQUEIDENTIFIER NOT NULL,
    DocumentId VARCHAR(100) NOT NULL,
    DocumentJson NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME
);
```

- **Purpose**: Current feed document storage and tracking
- **Features**: JSON document storage, change tracking, indexing status

### Search Infrastructure

#### `SearchIndex`

```sql
CREATE TABLE SearchIndex (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    IndexName VARCHAR(100) NOT NULL,
    ServiceEndpoint VARCHAR(500),
    ApiKey VARCHAR(500),
    CreatedDate DATETIME DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1
);
```

- **Purpose**: Search service configuration and credentials
- **Features**: Multi-search-engine support, credential management
- **Security**: Encrypted API keys and endpoint management

#### `SearchInsightsData`

```sql
CREATE TABLE SearchInsightsData (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SearchIndexId UNIQUEIDENTIFIER NOT NULL,
    DataCategory VARCHAR(50) NOT NULL,
    DataPoint VARCHAR(1000) NOT NULL,
    Count INT DEFAULT 0,
    Date DATE DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME DEFAULT GETUTCDATE()
);
```

- **Purpose**: Search analytics and business intelligence data
- **Features**: Category-based metrics, temporal tracking, aggregated counts
- **Integration**: Real-time analytics processing via Azure Functions

## 🔐 Security Model

### Service-Based Authentication

#### Database Users and Schemas

```sql
-- Feed Processing Service
CREATE LOGIN [FeedServicesFunc] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [FeedServicesFunc] FOR LOGIN [FeedServicesFunc]
    WITH DEFAULT_SCHEMA = [FeedServicesFunc];

-- Analytics Service
CREATE LOGIN [SearchInsightsFunc] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [SearchInsightsFunc] FOR LOGIN [SearchInsightsFunc]
    WITH DEFAULT_SCHEMA = [SearchInsightsFunc];

-- SFTP Service
CREATE LOGIN [SFTPGoServicesFunc] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [SFTPGoServicesFunc] FOR LOGIN [SFTPGoServicesFunc]
    WITH DEFAULT_SCHEMA = [SFTPGoServicesFunc];

-- Configuration Service
CREATE LOGIN [Configuration] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [Configuration] FOR LOGIN [Configuration]
    WITH DEFAULT_SCHEMA = [Configuration];

-- Administrative Access
CREATE LOGIN [Admin] WITH PASSWORD = 'SecurePassword123!';
CREATE USER [Admin] FOR LOGIN [Admin]
    WITH DEFAULT_SCHEMA = [Admin];
```

### Permission Matrix

| Schema               | FeedServices | SearchInsights | SFTPGo | Configuration | Admin |
| -------------------- | ------------ | -------------- | ------ | ------------- | ----- |
| `dbo` (Read)         | ✅           | ✅             | ✅     | ✅            | ✅    |
| `dbo` (Write)        | ✅           | ✅             | ❌     | ✅            | ✅    |
| `FeedServicesFunc`   | ✅           | ❌             | ❌     | ❌            | ✅    |
| `SearchInsightsFunc` | ❌           | ✅             | ❌     | ❌            | ✅    |
| `SFTPGoServicesFunc` | ❌           | ❌             | ✅     | ❌            | ✅    |
| `Configuration`      | ❌           | ❌             | ❌     | ✅            | ✅    |
| `Admin`              | ❌           | ❌             | ❌     | ❌            | ✅    |

## 🛠️ Database Setup

### Prerequisites

#### For SQL Azure

- **Azure Subscription** with SQL Database service enabled
- **SQL Server Management Studio** (SSMS) or Azure Data Studio
- **Visual Studio** with SQL Server Data Tools (SSDT)
- **.NET Framework 4.7.2+** for database project

#### For PostgreSQL

- **PostgreSQL 12+** server instance
- **pgAdmin** or preferred PostgreSQL management tool
- **psql** command-line tool
- **PostgreSQL Extensions**: uuid-ossp, pg_trgm, btree_gin

### SQL Azure Deployment

#### 1. Create Azure SQL Database

```bash
# Azure CLI commands
az sql server create \
    --name s2search-sql-server \
    --resource-group s2search-rg \
    --location eastus \
    --admin-user s2admin \
    --admin-password 'SecurePassword123!'

az sql db create \
    --resource-group s2search-rg \
    --server s2search-sql-server \
    --name S2Search \
    --service-objective S2 \
    --backup-storage-redundancy Local
```

#### 2. Deploy Database Project

```bash
# Using SqlPackage.exe
SqlPackage.exe /Action:Publish \
    /SourceFile:"S2Search.DB.Azure.dacpac" \
    /TargetConnectionString:"Server=tcp:s2search-sql-server.database.windows.net,1433;Initial Catalog=S2Search;Persist Security Info=False;User ID=s2admin;Password=SecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

#### 3. Configure Firewall Rules

```bash
# Allow Azure services
az sql server firewall-rule create \
    --resource-group s2search-rg \
    --server s2search-sql-server \
    --name AllowAzureServices \
    --start-ip-address 0.0.0.0 \
    --end-ip-address 0.0.0.0

# Allow specific IP ranges
az sql server firewall-rule create \
    --resource-group s2search-rg \
    --server s2search-sql-server \
    --name AllowClientIP \
    --start-ip-address 192.168.1.0 \
    --end-ip-address 192.168.1.255
```

### PostgreSQL Deployment

#### 1. Create Database and Extensions

```sql
-- Connect as superuser
CREATE DATABASE s2search_db;

-- Connect to the new database
\c s2search_db;

-- Create required extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";
CREATE EXTENSION IF NOT EXISTS "btree_gin";
CREATE EXTENSION IF NOT EXISTS "pg_stat_statements";
```

#### 2. Create Schemas

```sql
-- Create schemas (PostgreSQL equivalent)
CREATE SCHEMA IF NOT EXISTS feedservicesfunc;
CREATE SCHEMA IF NOT EXISTS searchinsightsfunc;
CREATE SCHEMA IF NOT EXISTS sftpgoservicesfunc;
CREATE SCHEMA IF NOT EXISTS configuration;
CREATE SCHEMA IF NOT EXISTS admin;
CREATE SCHEMA IF NOT EXISTS insights;
```

#### 3. Create Service Users

```sql
-- Create service users
CREATE USER feedservicesfunc WITH PASSWORD 'SecurePassword123!';
CREATE USER searchinsightsfunc WITH PASSWORD 'SecurePassword123!';
CREATE USER sftpgoservicesfunc WITH PASSWORD 'SecurePassword123!';
CREATE USER configuration WITH PASSWORD 'SecurePassword123!';
CREATE USER admin WITH PASSWORD 'SecurePassword123!';

-- Grant schema permissions
GRANT USAGE ON SCHEMA feedservicesfunc TO feedservicesfunc;
GRANT ALL PRIVILEGES ON SCHEMA feedservicesfunc TO feedservicesfunc;

GRANT USAGE ON SCHEMA searchinsightsfunc TO searchinsightsfunc;
GRANT ALL PRIVILEGES ON SCHEMA searchinsightsfunc TO searchinsightsfunc;

-- Additional grants for shared schemas
GRANT USAGE ON SCHEMA public TO feedservicesfunc, searchinsightsfunc, sftpgoservicesfunc, configuration;
```

## 📊 Stored Procedures

### Feed Management Procedures

#### `FeedServicesFunc.GetCurrentFeedDocuments`

```sql
CREATE PROCEDURE [FeedServicesFunc].[GetCurrentFeedDocuments]
    @SearchIndexId UNIQUEIDENTIFIER,
    @PageNumber INT = 0,
    @PageSize INT = 1000
AS
BEGIN
    SELECT
        Id,
        DocumentId,
        DocumentJson,
        CreatedDate,
        ModifiedDate
    FROM FeedCurrentDocuments
    WHERE SearchIndexId = @SearchIndexId
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
```

#### `FeedServicesFunc.GetSearchIndexCredentials`

```sql
CREATE PROCEDURE [FeedServicesFunc].[GetSearchIndexCredentials]
    @SearchIndexId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        si.IndexName,
        si.ServiceEndpoint,
        si.ApiKey,
        si.IsActive
    FROM SearchIndex si
    WHERE si.Id = @SearchIndexId AND si.IsActive = 1;
END
```

### Configuration Procedures

#### `Configuration.GetThemeByCustomerEndpoint`

```sql
CREATE PROCEDURE [Configuration].[GetThemeByCustomerEndpoint]
    @CustomerEndpoint VARCHAR(100)
AS
BEGIN
    SELECT
        t.Id,
        t.PrimaryColour,
        t.SecondaryColour,
        t.LogoUrl,
        t.CreatedDate
    FROM Themes t
    INNER JOIN Customers c ON t.CustomerId = c.Id
    WHERE c.CustomerEndpoint = @CustomerEndpoint;
END
```

### Analytics Procedures

#### `SearchInsightsFunc.RecordSearchInsight`

```sql
CREATE PROCEDURE [SearchInsightsFunc].[RecordSearchInsight]
    @SearchIndexId UNIQUEIDENTIFIER,
    @DataCategory VARCHAR(50),
    @DataPoint VARCHAR(1000),
    @Count INT = 1
AS
BEGIN
    MERGE SearchInsightsData AS target
    USING (SELECT @SearchIndexId AS SearchIndexId,
                  @DataCategory AS DataCategory,
                  @DataPoint AS DataPoint,
                  CAST(GETUTCDATE() AS DATE) AS Date) AS source
    ON (target.SearchIndexId = source.SearchIndexId
        AND target.DataCategory = source.DataCategory
        AND target.DataPoint = source.DataPoint
        AND target.Date = source.Date)
    WHEN MATCHED THEN
        UPDATE SET Count = target.Count + @Count,
                   ModifiedDate = GETUTCDATE()
    WHEN NOT MATCHED THEN
        INSERT (SearchIndexId, DataCategory, DataPoint, Count, Date, ModifiedDate)
        VALUES (source.SearchIndexId, source.DataCategory, source.DataPoint, @Count, source.Date, GETUTCDATE());
END
```

## 🔄 Migration and Version Management

### Database Versioning Strategy

1. **Schema Versioning** - Track schema changes with version numbers
2. **Migration Scripts** - Incremental update scripts for version upgrades
3. **Rollback Scripts** - Safe rollback procedures for each migration
4. **Environment Consistency** - Ensure development, staging, and production alignment

### Migration Process

#### SQL Azure Migrations

```sql
-- Migration script example: V1.1.0_Add_New_Customer_Fields.sql
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
               WHERE TABLE_NAME = 'Customers' AND COLUMN_NAME = 'Industry')
BEGIN
    ALTER TABLE Customers ADD Industry VARCHAR(100) NULL;
    PRINT 'Added Industry column to Customers table';
END

-- Update version tracking
INSERT INTO MigrationHistory (Version, Description, AppliedDate)
VALUES ('1.1.0', 'Added Industry field to Customers table', GETUTCDATE());
```

#### PostgreSQL Migrations

```sql
-- PostgreSQL migration script
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                   WHERE table_name = 'customers' AND column_name = 'industry') THEN
        ALTER TABLE customers ADD COLUMN industry VARCHAR(100);
        RAISE NOTICE 'Added industry column to customers table';
    END IF;
END $$;

-- Update version tracking
INSERT INTO migration_history (version, description, applied_date)
VALUES ('1.1.0', 'Added industry field to customers table', NOW());
```

### Data Migration Scripts

Located in `SQL Scripts/resurrection-2025/`:

- **Platform Modernization** - Scripts for migrating from legacy systems
- **Data Transformation** - ETL processes for data format changes
- **Index Optimization** - Performance improvement migrations
- **Security Updates** - Permission and security model updates

## 🚀 Performance Optimization

### Indexing Strategy

#### Primary Indexes

```sql
-- Customer lookup optimization
CREATE INDEX IX_Customers_CustomerEndpoint
ON Customers (CustomerEndpoint) INCLUDE (Id, BusinessName);

-- Feed processing optimization
CREATE INDEX IX_Feeds_SearchIndexId_IsLatest
ON Feeds (SearchIndexId, IsLatest) INCLUDE (FeedType, DataFormat);

-- Analytics query optimization
CREATE INDEX IX_SearchInsightsData_Covering
ON SearchInsightsData (SearchIndexId, DataCategory, Date)
INCLUDE (DataPoint, Count);

-- Document retrieval optimization
CREATE INDEX IX_FeedCurrentDocuments_SearchIndexId_CreatedDate
ON FeedCurrentDocuments (SearchIndexId, CreatedDate DESC)
INCLUDE (DocumentId, DocumentJson);
```

#### PostgreSQL Specific Indexes

```sql
-- Full-text search on document content
CREATE INDEX IF NOT EXISTS idx_feed_documents_fulltext
ON feedcurrentdocuments USING gin(to_tsvector('english', documentjson));

-- Efficient customer endpoint lookups
CREATE INDEX IF NOT EXISTS idx_customers_endpoint_hash
ON customers USING hash(customerendpoint);

-- Analytics aggregation optimization
CREATE INDEX IF NOT EXISTS idx_insights_partial
ON searchinsightsdata (searchindexid, datacategory, date)
WHERE count > 0;
```

### Query Optimization

#### Connection String Optimization

**SQL Azure:**

```csharp
"Server=tcp:s2search-sql-server.database.windows.net,1433;Initial Catalog=S2Search;Persist Security Info=False;User ID=serviceuser;Password=SecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Command Timeout=60;"
```

**PostgreSQL:**

```csharp
"Host=s2search-postgres.example.com;Database=s2search_db;Username=serviceuser;Password=SecurePassword123!;Port=5432;Pooling=true;MinPoolSize=5;MaxPoolSize=100;CommandTimeout=60;"
```

## 📈 Monitoring and Maintenance

### SQL Azure Monitoring

```sql
-- Performance monitoring queries
SELECT
    s.session_id,
    s.login_name,
    s.host_name,
    r.command,
    r.total_elapsed_time,
    r.cpu_time,
    r.logical_reads,
    t.text
FROM sys.dm_exec_sessions s
INNER JOIN sys.dm_exec_requests r ON s.session_id = r.session_id
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) t
WHERE s.is_user_process = 1;

-- Index usage statistics
SELECT
    OBJECT_NAME(ius.object_id) AS TableName,
    i.name AS IndexName,
    ius.user_seeks,
    ius.user_scans,
    ius.user_lookups,
    ius.user_updates
FROM sys.dm_db_index_usage_stats ius
INNER JOIN sys.indexes i ON ius.object_id = i.object_id AND ius.index_id = i.index_id
WHERE ius.database_id = DB_ID();
```

### PostgreSQL Monitoring

```sql
-- Performance monitoring
SELECT
    pid,
    usename,
    application_name,
    client_addr,
    query_start,
    state,
    query
FROM pg_stat_activity
WHERE state = 'active' AND pid <> pg_backend_pid();

-- Index usage statistics
SELECT
    schemaname,
    tablename,
    indexname,
    idx_scan,
    idx_tup_read,
    idx_tup_fetch
FROM pg_stat_user_indexes
ORDER BY idx_scan DESC;

-- Table statistics
SELECT
    schemaname,
    tablename,
    n_tup_ins,
    n_tup_upd,
    n_tup_del,
    n_live_tup,
    n_dead_tup,
    last_vacuum,
    last_autovacuum,
    last_analyze,
    last_autoanalyze
FROM pg_stat_user_tables;
```

## 🧪 Testing and Development

### Development Database Setup

#### 1. Local SQL Server (Development)

```bash
# Using Docker for local SQL Server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=DevPassword123!" \
  -p 1433:1433 --name s2search-sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest

# Deploy database project
SqlPackage.exe /Action:Publish \
  /SourceFile:"S2Search.DB.Azure.dacpac" \
  /TargetConnectionString:"Server=localhost,1433;Database=S2Search;User Id=sa;Password=DevPassword123!;TrustServerCertificate=true;"
```

#### 2. Local PostgreSQL (Development)

```bash
# Using Docker for local PostgreSQL
docker run --name s2search-postgres \
  -e POSTGRES_PASSWORD=DevPassword123! \
  -e POSTGRES_DB=s2search_db \
  -p 5432:5432 \
  -d postgres:14

# Connect and setup
psql -h localhost -U postgres -d s2search_db -f setup-postgres.sql
```

### Test Data Generation

```sql
-- Generate test customers
INSERT INTO Customers (Id, BusinessName, CustomerEndpoint, CreatedDate)
VALUES
    (NEWID(), 'Test Motors Ltd', 'testmotors', GETUTCDATE()),
    (NEWID(), 'Demo Car Sales', 'democars', GETUTCDATE()),
    (NEWID(), 'Sample Dealership', 'sampledealer', GETUTCDATE());

-- Generate test search indexes
DECLARE @CustomerId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Customers WHERE CustomerEndpoint = 'testmotors');
INSERT INTO SearchIndex (Id, IndexName, ServiceEndpoint, ApiKey, CreatedDate)
VALUES
    (NEWID(), 'testmotors-vehicles', 'https://test-search.search.windows.net', 'test-api-key', GETUTCDATE());

-- Generate test analytics data
DECLARE @SearchIndexId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM SearchIndex WHERE IndexName = 'testmotors-vehicles');
INSERT INTO SearchInsightsData (SearchIndexId, DataCategory, DataPoint, Count, Date)
VALUES
    (@SearchIndexId, 'search-term', 'ford focus', 25, GETUTCDATE()),
    (@SearchIndexId, 'search-term', 'bmw x5', 18, GETUTCDATE()),
    (@SearchIndexId, 'facet-filter', 'make:ford', 45, GETUTCDATE());
```

## 🔧 Integration with Applications

### Connection String Configuration

#### .NET Applications

```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "SqlAzure": "Server=tcp:s2search-sql-server.database.windows.net,1433;Initial Catalog=S2Search;Persist Security Info=False;User ID=FeedServicesFunc;Password=SecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
    "PostgreSQL": "Host=s2search-postgres.example.com;Database=s2search_db;Username=feedservicesfunc;Password=SecurePassword123!;Port=5432;Pooling=true;MinPoolSize=5;MaxPoolSize=100;"
  }
}

// Database context configuration
services.AddDbContext<S2SearchContext>(options =>
{
    if (configuration.GetValue<bool>("UsePostgreSQL"))
    {
        options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
    }
    else
    {
        options.UseSqlServer(configuration.GetConnectionString("SqlAzure"));
    }
});
```

#### Azure Functions

```csharp
// Function app configuration
[FunctionName("ProcessFeedData")]
public static async Task<IActionResult> ProcessFeedData(
    [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
    [Sql("SELECT * FROM FeedCurrentDocuments WHERE SearchIndexId = @SearchIndexId",
         CommandType = CommandType.Text,
         ConnectionStringSetting = "SqlConnectionString")] IEnumerable<FeedDocument> documents,
    ILogger log)
{
    // Function logic here
}
```

### Service Integration Patterns

#### Repository Pattern Implementation

```csharp
public interface IFeedRepository
{
    Task<IEnumerable<Feed>> GetLatestFeedsAsync(Guid searchIndexId);
    Task<FeedDocument> GetFeedDocumentAsync(int documentId);
    Task MergeFeedDocumentsAsync(Guid searchIndexId, IEnumerable<FeedDocument> documents);
}

public class FeedRepository : IFeedRepository
{
    private readonly S2SearchContext _context;

    public FeedRepository(S2SearchContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Feed>> GetLatestFeedsAsync(Guid searchIndexId)
    {
        return await _context.Feeds
            .Where(f => f.SearchIndexId == searchIndexId && f.IsLatest)
            .ToListAsync();
    }
}
```

## 🔒 Security Best Practices

### Database Security Checklist

#### Authentication & Authorization

- ✅ **Service-specific users** with minimal required permissions
- ✅ **Strong passwords** with complexity requirements
- ✅ **Schema-level security** isolating service operations
- ✅ **Connection encryption** for all database connections
- ✅ **API key encryption** for stored search service credentials

#### SQL Azure Security

- ✅ **Azure AD integration** for enhanced authentication
- ✅ **Firewall rules** restricting access to known IP ranges
- ✅ **Threat detection** enabled for suspicious activity monitoring
- ✅ **Transparent Data Encryption (TDE)** for data at rest
- ✅ **Dynamic Data Masking** for sensitive fields

#### PostgreSQL Security

- ✅ **SSL/TLS encryption** for all connections
- ✅ **Row Level Security (RLS)** for multi-tenant data isolation
- ✅ **pg_audit extension** for comprehensive audit logging
- ✅ **Connection limits** and rate limiting
- ✅ **Regular security updates** and patch management

## 📚 Documentation and Resources

### Database Documentation

- **Entity Relationship Diagrams** - Visual database schema documentation
- **Stored Procedure Documentation** - Parameter and usage documentation
- **Index Strategy Guide** - Performance optimization guidelines
- **Migration Procedures** - Step-by-step upgrade instructions

### External Resources

#### SQL Azure

- [Azure SQL Database Documentation](https://docs.microsoft.com/en-us/azure/azure-sql/)
- [SQL Database DTU Calculator](https://dtucalculator.azurewebsites.net/)
- [Azure SQL Performance Best Practices](https://docs.microsoft.com/en-us/azure/azure-sql/database/performance-guidance)

#### PostgreSQL

- [PostgreSQL Official Documentation](https://www.postgresql.org/docs/)
- [PostgreSQL Performance Tuning](https://wiki.postgresql.org/wiki/Performance_Optimization)
- [PostgreSQL Extensions Library](https://pgxn.org/)

## 🤝 Contributing

### Database Changes

1. **Create migration scripts** for both SQL Azure and PostgreSQL
2. **Test migrations** on development databases
3. **Update stored procedures** maintaining cross-platform compatibility
4. **Document schema changes** in migration notes
5. **Verify performance impact** with realistic data volumes

### Best Practices

- **Always backup** before applying schema changes
- **Test migrations** in non-production environments first
- **Use transactions** for complex migration scripts
- **Maintain compatibility** between SQL Azure and PostgreSQL
- **Document breaking changes** and upgrade procedures

## 📄 License

This project is proprietary software. See [LICENSE](../LICENSE) for details.

## 🔗 Related Documentation

- [Main S2Search Documentation](../README.md)
- [Backend APIs](../APIs/README.md)
- [Azure Functions](../AzureFunctions/README.md)
- [Kubernetes Deployment](../K8s/README.md)

---

_Built for enterprise-scale data management with SQL Azure and PostgreSQL support_
