CREATE PROCEDURE [dbo].[spStage_Insert]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @Color CHAR(8), 
    @Order tinyint,
    @PipelineId CHAR(36)
AS
	INSERT INTO [Stage] ([Id], [Name], [Color], [Order], [PipelineId])
    VALUES (@Id, @Name, @Color, @Order, @PipelineId);
RETURN 0
