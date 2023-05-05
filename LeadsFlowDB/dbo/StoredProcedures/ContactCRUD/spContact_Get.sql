﻿CREATE PROCEDURE [dbo].[spContact_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [Email], [FirstName], [LastNames], [Phone], [Address], [Company], [JobTitle], [Website], [Location], [Notes], [CreatedAt], [LastModifiedAt]
	FROM [Contact]
	WHERE [Id] = @Id;
END