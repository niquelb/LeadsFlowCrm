CREATE PROCEDURE [dbo].[spContact_GetAll]
AS
BEGIN
	SELECT [Id], [Email], [FirstName], [LastNames], [Phone], [Address], [Company], [JobTitle], [Website], [Location], [Notes], [CreatedAt], [LastModifiedAt]
	FROM [Contact];
END
