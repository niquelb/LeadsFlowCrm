CREATE PROCEDURE [dbo].[spContact_GetAll]
    @Filter NVARCHAR(MAX) = NULL
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX);
	SET @SqlQuery = N'
        SELECT [Id], [Email], [FirstName], [LastNames], [Phone], [Address], [Company], [JobTitle], [Website], [Location], [Notes], [StageId], [UserId], [CreatedAt], [LastModifiedAt], [Deleted]
		FROM [Contact]
		WHERE [Deleted] = 0';

	IF (@Filter IS NOT NULL)
    BEGIN
        SET @SqlQuery = @SqlQuery + N' AND ' + @Filter;
    END;

    EXEC(@SqlQuery);
	
END
