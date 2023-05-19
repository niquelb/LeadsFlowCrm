CREATE PROCEDURE [dbo].[spPipelineUser_Relationship]
	@UserId char(36),
	@PipelineId char(36)
AS
BEGIN
	UPDATE [Pipeline]
	SET [CreatorId] = @UserId
	WHERE [Id] = @PipelineId;
END
