CREATE PROCEDURE [dbo].[spPipelineOrganization_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [Pipeline_Organization]
	WHERE [Id] = @Id;
END
