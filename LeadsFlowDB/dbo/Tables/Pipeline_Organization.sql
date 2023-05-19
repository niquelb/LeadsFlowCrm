CREATE TABLE [dbo].[Pipeline_Organization]
(
	Id CHAR(36) NOT NULL PRIMARY KEY, 
    [PipelineId] CHAR(36) NOT NULL, 
    [OrganizationId] CHAR(36) NOT NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate(),
    [Deleted] BIT NOT NULL DEFAULT 0

    CONSTRAINT [FK_PipelineOrganization_Pipeline] FOREIGN KEY ([PipelineId]) REFERENCES [Pipeline]([Id])
    CONSTRAINT [FK_PipelineOrganization_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Organization]([Id])
)
