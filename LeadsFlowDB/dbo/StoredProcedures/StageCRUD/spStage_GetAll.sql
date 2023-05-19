CREATE PROCEDURE [dbo].[spStage_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [Color], [PipelineId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Stage]
	WHERE [Deleted] = 0;
END
