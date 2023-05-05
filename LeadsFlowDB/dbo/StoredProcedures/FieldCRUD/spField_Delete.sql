CREATE PROCEDURE [dbo].[spField_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [Field]
	WHERE [Id] = @Id;
END
