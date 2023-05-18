CREATE PROCEDURE [dbo].[spStage_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @Color CHAR(8), 
    @PipelineId CHAR(36)
AS
	UPDATE [Stage]
    SET
    [Name] = @Name,
    [Color] = @Color,
    [PipelineId] = @PipelineId,
    [LastModifiedAt] = getutcdate()
	WHERE [Id] = @Id;
RETURN 0
