CREATE PROCEDURE [dbo].[spContact_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [Contact]
	WHERE [Id] = @Id;
END
