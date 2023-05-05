CREATE PROCEDURE [dbo].[spPipeline_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [CreatorId], [CreatedAt], [LastModifiedAt]
	FROM [Pipeline];
END
