﻿CREATE PROCEDURE [dbo].[spUser_Get]
	@Id char(36)
AS
BEGIN
	SELECT [Id], [OauthToken], [Email], [DisplayName], [OrganizationId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [User]
	WHERE [Id] = @Id
	AND [Deleted] = 0;
END
