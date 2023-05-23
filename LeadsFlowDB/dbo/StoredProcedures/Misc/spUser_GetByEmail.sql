CREATE PROCEDURE [dbo].[spUser_GetByEmail]
	@email nvarchar(256)
AS
BEGIN
	SELECT [Id]
	FROM [User]
	WHERE [Email] LIKE @email;
END
