CREATE PROCEDURE [dbo].[spContact_Update]
	@Id CHAR(36),
    @Email NVARCHAR(256),
    @FirstName NVARCHAR(50), 
    @LastNames NVARCHAR(75), 
    @Phone NVARCHAR(15), 
    @Address NVARCHAR(150), 
    @Company NVARCHAR(50), 
    @JobTitle NVARCHAR(50), 
    @Website NVARCHAR(50), 
    @Location NVARCHAR(50), 
    @Notes NVARCHAR(MAX)
AS
BEGIN
    UPDATE [Contact]
    SET
    [Email] = @Email, 
    [FirstName] = @FirstName, 
    [LastNames] = @LastNames, 
    [Phone] = @Phone, 
    [Address] = @Address, 
    [Company] = @Company, 
    [JobTitle] = @JobTitle, 
    [Website] = @Website, 
    [Location] = @Location, 
    [Notes] = @Notes, 
    [LastModifiedAt] = getutcdate()
    WHERE [Id] = @Id;
END
