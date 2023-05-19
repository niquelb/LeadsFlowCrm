CREATE PROCEDURE [dbo].[spContact_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [Contact]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END
