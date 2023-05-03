CREATE PROCEDURE [dbo].[spUser_Update]
	@Id char(36),
	@OauthToken NVARCHAR(50),
    @Email NVARCHAR(256),
    @DisplayName NVARCHAR(50),
    @OrganizationId CHAR(36)
AS
BEGIN
    UPDATE [User]
    SET 
    [OauthToken] = @OauthToken, 
    [Email] = @Email, 
    [DisplayName] = @DisplayName, 
    [OrganizationId] = @OrganizationId,
    [LastModifiedAt] = getutcdate()
END
