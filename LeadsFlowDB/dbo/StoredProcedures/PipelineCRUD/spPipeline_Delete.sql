CREATE PROCEDURE [dbo].[spPipeline_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [Pipeline]
	WHERE [Id] = @Id;
END
