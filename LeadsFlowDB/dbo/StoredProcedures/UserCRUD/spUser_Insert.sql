CREATE PROCEDURE [dbo].[spUser_Insert]
	@Id char(36),
	@OauthToken NVARCHAR(50),
    @Email NVARCHAR(256),
    @DisplayName NVARCHAR(50),
    @OrganizationId CHAR(36)
AS
BEGIN
    INSERT INTO [User] ([Id], [OauthToken], [Email], [DisplayName], [OrganizationId])
    VALUES (@Id, @OauthToken, @Email, @DisplayName, @OrganizationId);
END
