CREATE PROCEDURE [dbo].[spPipelineOrganization_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [PipelineId], [OrganizationId], [CreatedAt], [LastModifiedAt]
	FROM [Pipeline_Organization]
	WHERE [Id] = @Id;
END
