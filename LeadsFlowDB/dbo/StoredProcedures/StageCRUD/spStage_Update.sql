CREATE PROCEDURE [dbo].[spStage_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50) = null, 
    @Color CHAR(8) = null, 
    @Order tinyint = null,
    @PipelineId CHAR(36) = null
AS
	UPDATE [Stage]
    SET
    [Name] = COALESCE(@Name, [Name]),
    [Color] = COALESCE(@Color, [Color]),
    [Order] = COALESCE(@Order, [Order]),
    [PipelineId] = COALESCE(@PipelineId, [PipelineId]),
    [LastModifiedAt] = getutcdate()
	WHERE [Id] = @Id;
RETURN 0
