USE iPoolDb;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Users_NormalizedEmail')
BEGIN
    CREATE UNIQUE INDEX IX_Users_NormalizedEmail ON dbo.Users (NormalizedEmail);
END
GO
