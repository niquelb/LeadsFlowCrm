CREATE PROCEDURE [dbo].[spField_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @Type VARCHAR(10), 
    @PipelineId CHAR(36)
AS
BEGIN
    UPDATE [Field]
    SET
    [Name] = @Name,
    [Type] = @Type,
    [PipelineId] = @PipelineId,
    [LastModifiedAt] = getutcdate()
END
