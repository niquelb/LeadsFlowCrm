CREATE PROCEDURE [dbo].[spOrganization_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50) = null, 
    @CreatorId CHAR(36) = null
AS
BEGIN
    UPDATE [Organization]
    SET
    [Name] = COALESCE(@Name, [Name]),
    [CreatorId] = COALESCE(@CreatorId, [CreatorId]),
    [LastModifiedAt] = getutcdate()
	WHERE [Id] = @Id;
END
