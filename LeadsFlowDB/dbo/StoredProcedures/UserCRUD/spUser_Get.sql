CREATE PROCEDURE [dbo].[spUser_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [OauthToken], [Email], [DisplayName], [OrganizationId], [CreatedAt], [LastModifiedAt]
	FROM [User]
	WHERE [Id] = @Id;
END
