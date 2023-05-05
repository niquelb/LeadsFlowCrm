CREATE PROCEDURE [dbo].[spOrganization_Delete]
	@Id char(36)
AS
BEGIN
	DELETE
	FROM [Organization]
	WHERE [Id] = @Id;
END