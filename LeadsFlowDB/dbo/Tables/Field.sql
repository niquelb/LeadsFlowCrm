CREATE TABLE [dbo].[Field]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Type] VARCHAR(10) NULL, 
    [PipelineId] CHAR(36) NOT NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate(),
    [Deleted] BIT NOT NULL DEFAULT 0

    CONSTRAINT [FK_Fields_Pipeline] FOREIGN KEY ([PipelineId]) REFERENCES [Pipeline]([Id])
)
