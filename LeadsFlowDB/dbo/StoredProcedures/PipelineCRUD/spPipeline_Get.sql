CREATE PROCEDURE [dbo].[spPipeline_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Name], [CreatorId], [CreatedAt], [LastModifiedAt]
	FROM [Pipeline]
	WHERE [Id] = @Id;
END
