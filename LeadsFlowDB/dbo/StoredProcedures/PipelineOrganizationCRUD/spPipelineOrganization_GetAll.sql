CREATE PROCEDURE [dbo].[spPipelineOrganization_GetAll]
AS
BEGIN
	SELECT [Id], [PipelineId], [OrganizationId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Pipeline_Organization]
	WHERE [Deleted] = 0;
END
