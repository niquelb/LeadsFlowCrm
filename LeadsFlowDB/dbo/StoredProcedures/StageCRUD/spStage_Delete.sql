CREATE PROCEDURE [dbo].[spStage_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [Stage]
	WHERE [Id] = @Id;
END
