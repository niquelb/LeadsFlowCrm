CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
	SELECT [Id], [OauthToken], [Email], [DisplayName], [OrganizationId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [User]
	WHERE [Deleted] = 0;
END
