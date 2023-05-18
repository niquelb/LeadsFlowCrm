CREATE PROCEDURE [dbo].[spOrganization_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @CreatorId CHAR(36)
AS
BEGIN
    UPDATE [Organization]
    SET
    [Name] = @Name,
    [CreatorId] = @CreatorId,
    [LastModifiedAt] = getutcdate()
	WHERE [Id] = @Id;
END
