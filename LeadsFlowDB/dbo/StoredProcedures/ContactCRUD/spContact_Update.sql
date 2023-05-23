CREATE PROCEDURE [dbo].[spContact_Update]
	@Id CHAR(36),
    @Email NVARCHAR(256) = null,
    @FirstName NVARCHAR(50) = null, 
    @LastNames NVARCHAR(75) = null, 
    @Phone NVARCHAR(15) = null, 
    @Address NVARCHAR(150) = null, 
    @Company NVARCHAR(50) = null, 
    @JobTitle NVARCHAR(50) = null, 
    @Website NVARCHAR(50) = null, 
    @Location NVARCHAR(50) = null, 
    @Notes NVARCHAR(MAX) = null
AS
BEGIN
    UPDATE [Contact]
    SET
    [Email] = COALESCE(@Email, [Email]), 
    [FirstName] = COALESCE(@FirstName, [FirstName]), 
    [LastNames] = COALESCE(@LastNames, [LastNames]), 
    [Phone] = COALESCE(@Phone, [Phone]), 
    [Address] = COALESCE(@Address, [Address]), 
    [Company] = COALESCE(@Company, [Company]), 
    [JobTitle] = COALESCE(@JobTitle, [JobTitle]), 
    [Website] = COALESCE(@Website, [Website]), 
    [Location] = COALESCE(@Location, [Location]), 
    [Notes] = COALESCE(@Notes, [Notes]), 
    [LastModifiedAt] = getutcdate()
    WHERE [Id] = @Id;
END
