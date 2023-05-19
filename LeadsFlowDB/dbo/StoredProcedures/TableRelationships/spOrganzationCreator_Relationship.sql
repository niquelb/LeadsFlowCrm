CREATE PROCEDURE [dbo].[spOrganzationCreator_Relationship]
	@OrgId CHAR(36),
	@CreatorId CHAR(36)
AS
BEGIN
	UPDATE [Organization]
	SET
	[CreatorId] = @CreatorId
	WHERE [Id] = @OrgId;
END
