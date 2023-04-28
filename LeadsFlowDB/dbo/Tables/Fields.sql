CREATE TABLE [dbo].[Fields]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Type] VARCHAR(10) NULL, 
    [PipelineId] CHAR(36) NOT NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate()
)
