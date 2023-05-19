CREATE PROCEDURE [dbo].[spField_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [Field]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END
