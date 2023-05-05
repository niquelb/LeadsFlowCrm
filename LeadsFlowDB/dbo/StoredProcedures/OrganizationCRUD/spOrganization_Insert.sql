CREATE PROCEDURE [dbo].[spOrganization_Insert]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @CreatorId CHAR(36)
AS
BEGIN
    INSERT INTO [Organization] ([Id], [Name], [CreatorId])
    VALUES (@Id, @Name, @CreatorId);
END
