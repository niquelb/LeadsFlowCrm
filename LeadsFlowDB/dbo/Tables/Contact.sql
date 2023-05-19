CREATE TABLE [dbo].[Contact]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [Email] NVARCHAR(256) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastNames] NVARCHAR(75) NULL, 
    [Phone] NVARCHAR(15) NULL, 
    [Address] NVARCHAR(150) NULL, 
    [Company] NVARCHAR(50) NULL, 
    [JobTitle] NVARCHAR(50) NULL, 
    [Website] NVARCHAR(50) NULL, 
    [Location] NVARCHAR(50) NULL, 
    [Notes] NVARCHAR(MAX) NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate(),
    [Deleted] BIT NOT NULL DEFAULT 0
)
