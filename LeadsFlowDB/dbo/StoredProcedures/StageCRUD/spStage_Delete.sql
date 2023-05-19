CREATE PROCEDURE [dbo].[spStage_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [Stage]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END
