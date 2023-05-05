CREATE PROCEDURE [dbo].[spOrganization_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [CreatorId], [CreatedAt], [LastModifiedAt]
	FROM [Organization];
END
