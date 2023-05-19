CREATE PROCEDURE [dbo].[spOrganization_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [CreatorId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [Organization]
	WHERE [Deleted] = 0;
END
