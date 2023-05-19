CREATE PROCEDURE [dbo].[spOrganization_Delete]
	@Id char(36)
AS
BEGIN
	UPDATE [Organization]
	SET [Deleted] = 1
	WHERE [Id] = @Id;
END