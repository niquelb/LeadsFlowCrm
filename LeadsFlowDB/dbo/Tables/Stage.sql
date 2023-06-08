CREATE TABLE [dbo].[Stage]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Color] CHAR(8) NULL, 
    [Order] TINYINT NULL DEFAULT 0,
    [PipelineId] CHAR(36) NOT NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate(),
    [Deleted] BIT NOT NULL DEFAULT 0

    CONSTRAINT [FK_Stage_Pipeline] FOREIGN KEY ([PipelineId]) REFERENCES [Pipeline]([Id]), 
)
