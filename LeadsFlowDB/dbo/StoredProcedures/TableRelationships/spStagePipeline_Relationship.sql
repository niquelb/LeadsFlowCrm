CREATE PROCEDURE [dbo].[spStagePipeline_Relationship]
	@PipelineId char(36),
	@StageId char(36)
AS
BEGIN
	UPDATE [Stage]
	SET [PipelineId] = @PipelineId
	WHERE [Id] = @StageId;
END
