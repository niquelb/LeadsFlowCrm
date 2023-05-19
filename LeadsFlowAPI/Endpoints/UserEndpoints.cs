using DataAccess.DataAccess;

namespace LeadsFlowAPI.Endpoints;

public static class UserEndpoints
{
	// TODO: Make method to assign an organizationId to an existing user (new SP?)

	/// <summary>
	/// Configures all the User endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureUserEndpoints(this WebApplication app)
	{
		app.MapGet("/Users", GetUsers);
		app.MapGet("/Users/{id}", GetUser);
		app.MapPost("/Users", PostUser);
		app.MapPut("/Users", PutUser);
		app.MapDelete("/Users", DeleteUser);
		app.MapPut("/Users/Organization", PutUserOrgRelationship);
	}

	/// <summary>
	/// Endpoint for getting all the users
	/// </summary>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, all the users are returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetUsers(IUserDAO userDAO)
	{
		try
		{
			return Results.Ok(await userDAO.GetUsers());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for getting a specific user based on an ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>	
	/// [200 -> The call was successful, requested user is returned]
	/// [404 -> No user with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetUser(String id, IUserDAO userDAO)
	{
		try
		{
			var result = await userDAO.GetUser(id);
			if (result == null)
			{
				return Results.NotFound(id);
			}

			return Results.Ok(result);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for inserting a user
	/// </summary>
	/// <param name="user">User to be inserted</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, inserted user's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PostUser(User user, IUserDAO userDAO)
	{
		try
		{
			await userDAO.InsertUser(user);
			return Results.Ok(user.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for updating a user
	/// </summary>
	/// <param name="user">User to be updated</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was succesful, updated user's ID is returned]
	/// [404 -> No user with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PutUser(User user, IUserDAO userDAO)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await userDAO.GetUser(user.Id);
			if (result == null)
			{
				return Results.NotFound(user.Id);
			}

			await userDAO.UpdateUser(user);
			return Results.Ok(user.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for deleting a user with a specified ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, deleted user's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> DeleteUser(string id, IUserDAO userDAO)
	{
		try
		{
			await userDAO.DeleteUser(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Updates the user with the given ID's relationship using the given organization ID
	/// </summary>
	/// <param name="userId">ID for the query</param>
	/// <param name="orgId">ID for the query</param>
	/// <param name="relationship">DAO object from Dependency Injection</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <param name="orgDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200] -> Relationship was successful.
	/// [404] -> User or Organization with given IDs were not found
	/// [Any other error] -> Query failed
	/// </returns>
	public static async Task<IResult> PutUserOrgRelationship(
		string userId,
		string orgId,
		IUserDAO userDAO,
		IOrganizationDAO orgDAO)
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

			await userDAO.SetupUserOrgRelationship(userId, orgId);
			return Results.Ok("Relationship created successfully");

		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
