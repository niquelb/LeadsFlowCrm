CREATE PROCEDURE [dbo].[spPipelineOrganization_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [PipelineId], [OrganizationId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Pipeline_Organization]
	WHERE [Id] = @Id;
END
