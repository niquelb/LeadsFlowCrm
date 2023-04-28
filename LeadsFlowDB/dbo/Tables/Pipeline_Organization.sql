CREATE TABLE [dbo].[Pipeline_Organization]
(
	Id CHAR(36) NOT NULL PRIMARY KEY, 
    [PipelineId] CHAR(36) NOT NULL, 
    [OrganizationId] CHAR(36) NOT NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT getutcdate(), 
    [LastModifiedAt] DATETIME2 NULL DEFAULT getutcdate()
)
