CREATE PROCEDURE [dbo].[spField_Insert]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @Type VARCHAR(10), 
    @PipelineId CHAR(36)
AS
BEGIN
    INSERT INTO [Field] ([Id], [Name], [Type], [PipelineId])
    VALUES (@Id, @Name, @Type, @PipelineId);
END
