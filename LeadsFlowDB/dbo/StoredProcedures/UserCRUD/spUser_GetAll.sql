CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
	SELECT [Id], [OauthToken], [Email], [DisplayName], [OrganizationId], [CreatedAt], [LastModifiedAt]
	FROM [User];
END
