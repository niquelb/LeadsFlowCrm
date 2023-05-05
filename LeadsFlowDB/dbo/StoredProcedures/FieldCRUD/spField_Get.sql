CREATE PROCEDURE [dbo].[spField_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Name], [Type], [PipelineId], [CreatedAt], [LastModifiedAt]
	FROM [Field]
	WHERE [Id] = @Id;
END
