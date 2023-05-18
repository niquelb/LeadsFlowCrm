CREATE PROCEDURE [dbo].[spUserOrganization_Relationship]
	@UserId CHAR(36),
	@OrgId CHAR(36)
AS
BEGIN
	UPDATE [User]
	SET
	[OrganizationId] = @OrgId
	WHERE [Id] = @UserId;
END
