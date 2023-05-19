CREATE PROCEDURE [dbo].[spPipeline_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [Pipeline]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END
