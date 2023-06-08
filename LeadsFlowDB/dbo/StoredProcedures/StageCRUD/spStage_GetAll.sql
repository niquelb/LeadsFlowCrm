CREATE PROCEDURE [dbo].[spStage_GetAll]
    @Filter NVARCHAR(MAX) = NULL
AS
BEGIN
    DECLARE @SqlQuery NVARCHAR(MAX);

    SET @SqlQuery = N'
        SELECT [Id], [Name], [Color], [PipelineId], [CreatedAt], [LastModifiedAt], [Deleted]
        FROM [Stage]
        WHERE [Deleted] = 0';

    IF (@Filter IS NOT NULL)
    BEGIN
        SET @SqlQuery = @SqlQuery + N' AND ' + @Filter;
    END;

    EXEC(@SqlQuery);
END

