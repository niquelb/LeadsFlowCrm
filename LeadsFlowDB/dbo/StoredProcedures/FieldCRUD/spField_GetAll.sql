CREATE PROCEDURE [dbo].[spField_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [Type], [PipelineId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Field]
	WHERE [Deleted] = 0;
END
