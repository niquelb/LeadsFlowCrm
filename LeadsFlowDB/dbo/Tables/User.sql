CREATE TABLE [dbo].[User]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [OauthToken] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [DisplayName] NVARCHAR(50) NOT NULL, 
    [OrganizationId] CHAR(36) NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate(),
    [Deleted] BIT NOT NULL DEFAULT 0 

    CONSTRAINT [FK_User_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Organization]([Id])
)
