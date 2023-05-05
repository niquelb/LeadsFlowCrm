CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [User]
	WHERE [Id] = @Id;
END
