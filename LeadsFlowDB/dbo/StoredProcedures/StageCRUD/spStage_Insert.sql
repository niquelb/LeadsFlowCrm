CREATE PROCEDURE [dbo].[spStage_Insert]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @Color CHAR(8), 
    @PipelineId CHAR(36)
AS
	INSERT INTO [Stage] ([Id], [Name], [Color], [PipelineId])
    VALUES (@Id, @Name, @Color, @PipelineId);
RETURN 0
