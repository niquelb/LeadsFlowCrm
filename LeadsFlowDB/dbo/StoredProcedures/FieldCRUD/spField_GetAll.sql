CREATE PROCEDURE [dbo].[spField_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [Type], [PipelineId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Field];
END
