CREATE TABLE [dbo].[Pipeline]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [CreatorId] CHAR(36) NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate()
    
    CONSTRAINT [FK_Pipeline_User] FOREIGN KEY ([CreatorId]) REFERENCES [User]([Id])
)
