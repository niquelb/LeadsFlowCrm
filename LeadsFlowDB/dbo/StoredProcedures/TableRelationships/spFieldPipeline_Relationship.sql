CREATE PROCEDURE [dbo].[spFieldPipeline_Relationship]
	@PipelineId char(36),
	@FieldId char(36)
AS
BEGIN
	UPDATE [Field]
	SET [PipelineId] = @PipelineId
	WHERE [Id] = @FieldId;
END
