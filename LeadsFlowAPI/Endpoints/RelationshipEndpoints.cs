using DataAccess.DataAccess;

namespace LeadsFlowAPI.Endpoints;

public static class RelationshipEndpoints
{
	/// <summary>
	/// Configures all the FK relationship endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void SetupRelationshipEndpoints(this WebApplication app)
	{
		app.MapPut("/UserOrganization", SetupUserOrgRelationship);
	}

	/// <summary>
	/// Updates the user with the given ID's relationship using the given organization ID
	/// </summary>
	/// <param name="userId">ID for the query</param>
	/// <param name="orgId">ID for the query</param>
	/// <param name="relationship">DAO object from Dependency Injection</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <param name="orgDAO">DAO object from Dependency Injection</param>
	/// <returns></returns>
	public static async Task<IResult> SetupUserOrgRelationship(string userId, string orgId, IForeignKeyRelationships relationship, IUserDAO userDAO, IOrganizationDAO orgDAO)
	{
		try
		{
			/*
			 * Before doing anything we will check if the IDs exist
			 */
			var user = await userDAO.GetUser(userId);
			var org = await orgDAO.GetOrganization(orgId);

			if (user == null)
			{
				return Results.NotFound($"User not found: {userId}");
			}

			if (org == null)
			{
				return Results.NotFound($"Organization not found: {orgId}");
			}

			await relationship.SetupUserOrgRelationship(userId, orgId);
			return Results.Ok("Relationship created successfully");

		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
