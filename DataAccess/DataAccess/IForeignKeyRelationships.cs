namespace DataAccess.DataAccess
{
	public interface IForeignKeyRelationships
	{
		Task SetupUserOrgRelationship(string UserId, string OrgId);
	}
}