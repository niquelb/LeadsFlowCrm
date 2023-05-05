CREATE PROCEDURE [dbo].[spPipeline_Insert]
	@Id CHAR(36), 
	@Name NVARCHAR(50), 
	@CreatorId CHAR(36)
AS
BEGIN
	INSERT INTO [Pipeline] ([Id], [Name], [CreatorId], [CreatedAt], [LastModifiedAt])
	VALUES (@Id, @Name, @CreatorId, getutcdate(), getutcdate())

END
