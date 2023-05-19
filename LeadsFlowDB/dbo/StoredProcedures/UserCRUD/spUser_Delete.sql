CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [User]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END
