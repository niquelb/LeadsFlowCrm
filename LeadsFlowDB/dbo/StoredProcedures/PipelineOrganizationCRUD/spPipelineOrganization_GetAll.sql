CREATE PROCEDURE [dbo].[spPipelineOrganization_GetAll]
AS
BEGIN
	SELECT [Id], [PipelineId], [OrganizationId], [CreatedAt], [LastModifiedAt]
	FROM [Pipeline_Organization];
END
