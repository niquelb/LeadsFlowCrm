CREATE PROCEDURE [dbo].[spOrganization_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Name], [CreatorId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Organization]
	WHERE [Id] = @Id;
END
