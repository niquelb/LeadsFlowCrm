﻿CREATE PROCEDURE [dbo].[spOrganization_Update]
	@Id CHAR(36), 
    @Name NVARCHAR(50), 
    @CreatorId CHAR(36)
AS
BEGIN
    UPDATE [Organization]
    SET
    [Id] = @Id,
    [Name] = @Name,
    [CreatorId] = @CreatorId,
    [LastModifiedAt] = getutcdate()
END