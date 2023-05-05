CREATE PROCEDURE [dbo].[spContact_Insert]
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
    INSERT INTO [Contact] ([Id], [Email], [FirstName], [LastNames], [Phone], [Address], [Company], [JobTitle], [Website], [Location], [Notes], [CreatedAt], [LastModifiedAt])
    VALUES (@Id, @Email, @FirstName, @LastNames, @Phone, @Address, @Company, @JobTitle, @Website, @Location, @Notes, getutcdate(), getutcdate())
END
