CREATE PROCEDURE [dbo].[spStage_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [Color], [PipelineId], [CreatedAt], [LastModifiedAt]
	FROM [Stage];
END
