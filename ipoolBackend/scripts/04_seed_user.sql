USE iPoolDb;
GO

-- Example seed (replace hash with a real one from your API)
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE NormalizedEmail = 'admin@ipool.com')
BEGIN
    INSERT INTO dbo.Users (Id, Name, Email, NormalizedEmail, PasswordHash, IsActive, CreatedAt)
    VALUES (NEWID(), 'Admin iPool', 'admin@ipool.com', 'admin@ipool.com', 'REPLACE_WITH_HASH', 1, SYSUTCDATETIME());
END
GO
