namespace DataAccess.DataAccess
{
	public interface IUserOrgRelationship
	{
		Task SetupRelationship(string UserId, string OrgId);
	}
}