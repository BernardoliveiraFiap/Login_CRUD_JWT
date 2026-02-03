USE iPoolDb;
GO

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Users;
END
GO
