CREATE PROCEDURE [dbo].[spField_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Name], [Type], [PipelineId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Field]
	WHERE [Id] = @Id
	AND [Deleted] = 0;
END
