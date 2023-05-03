CREATE PROCEDURE [dbo].[spPipeline_Update]
	@Id CHAR(36), 
	@Name NVARCHAR(50), 
	@CreatorId CHAR(36)
AS
BEGIN
	UPDATE [Pipeline]
	SET
	[Id] = @Id, 
	[Name] = @Name, 
	[CreatorId] = @CreatorId, 
	[LastModifiedAt] = getutcdate()
END
