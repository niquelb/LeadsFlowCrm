CREATE PROCEDURE [dbo].[spStage_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Name], [Color], [PipelineId], [CreatedAt], [LastModifiedAt]
	FROM [Stage]
	WHERE [Id] = @Id;
END
