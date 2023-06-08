CREATE PROCEDURE [dbo].[spContact_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Email], [FirstName], [LastNames], [Phone], [Address], [Company], [JobTitle], [Website], [Location], [Notes], [StageId], [UserId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Contact]
	WHERE [Id] = @Id
	AND [Deleted] = 0;
END
