CREATE PROCEDURE [dbo].[spPipelineOrganization_Insert]
	@Id CHAR(36), 
    @PipelineId CHAR(36), 
    @OrganizationId CHAR(36)
AS
BEGIN
    INSERT INTO [Pipeline_Organization] ([Id], [PipelineId], [OrganizationId])
    VALUES (@Id, @PipelineId, @OrganizationId);
END
