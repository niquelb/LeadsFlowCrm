namespace DataAccess.DataAccess
{
	public interface IForeignKeyRelationships
	{
		Task SetupFieldPipelineRelathionship(string PipelineId, string FieldId);
		Task SetupOrgCreatorRelationship(string OrgId, string UserId);
		Task SetupPipelineUserRelationship(string UserId, string PipelineId);
		Task SetupStagePipelineRelathionship(string PipelineId, string StageId);
		Task SetupUserOrgRelationship(string UserId, string OrgId);
	}
}