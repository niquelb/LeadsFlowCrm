CREATE PROCEDURE [dbo].[spStage_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Name], [Color], [PipelineId], [CreatedAt], [Order], [LastModifiedAt], [Deleted]
	FROM [Stage]
	WHERE [Id] = @Id
	AND [Deleted] = 0;
END
