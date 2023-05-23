CREATE PROCEDURE [dbo].[spField_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50) = null, 
    @Type VARCHAR(10) = null, 
    @PipelineId CHAR(36) = null
AS
BEGIN
    UPDATE [Field]
    SET
    [Name] = COALESCE(@Name, [Name]),
    [Type] = COALESCE(@Type, [Type]),
    [PipelineId] = COALESCE(@PipelineId, [PipelineId]),
    [LastModifiedAt] = getutcdate()
	WHERE [Id] = @Id;
END
