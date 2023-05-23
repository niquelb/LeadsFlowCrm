CREATE PROCEDURE [dbo].[spUser_Update]
	@Id char(36),
	@OauthToken NVARCHAR(50) = null,
    @Email NVARCHAR(256) = null,
    @DisplayName NVARCHAR(50) = null,
    @OrganizationId CHAR(36) = null
AS
BEGIN
    UPDATE [User]
    SET 
    [OauthToken] = COALESCE(@OauthToken, [OauthToken]), 
    [Email] = COALESCE(@Email, [Email]), 
    [DisplayName] = COALESCE(@DisplayName, [DisplayName]), 
    [OrganizationId] = COALESCE(@OrganizationId, [OrganizationId]),
    [LastModifiedAt] = getutcdate()
    WHERE [Id] = @Id;
END
