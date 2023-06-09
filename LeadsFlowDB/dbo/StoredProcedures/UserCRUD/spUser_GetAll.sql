CREATE PROCEDURE [dbo].[spUser_GetAll]
    @Filter NVARCHAR(MAX) = NULL
AS
BEGIN
DECLARE @SqlQuery NVARCHAR(MAX);

    SET @SqlQuery = N'
        SELECT [Id], [OauthToken], [Email], [DisplayName], [OrganizationId], [CreatedAt], [LastModifiedAt], [Deleted]
	FROM [User]
	WHERE [Deleted] = 0';

    IF (@Filter IS NOT NULL)
    BEGIN
        SET @SqlQuery = @SqlQuery + N' AND ' + @Filter;
    END;

    EXEC(@SqlQuery);
END
