CREATE PROCEDURE [dbo].[spPipelineOrganization_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [Pipeline_Organization]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END
