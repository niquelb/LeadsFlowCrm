using DataAccess.Models;

namespace LeadsFlowAPI.Endpoints;

public static class UserEndpoints
{
	/// <summary>
	/// Configures all the API endpoints
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureUserEndpoints(this WebApplication app)
	{
		app.MapGet("/Users", GetUsers);
		app.MapGet("/Users/{id}", GetUser);
		app.MapPost("/Users", PostUser);
		app.MapPut("/Users", PutUser);
		app.MapDelete("/Users", DeleteUser);
	}

	/// <summary>
	/// Endpoint for getting all the users
	/// </summary>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>Result wrapped in an HTTP response code</returns>
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
	/// <returns>Result wrapped in an HTTP response code</returns>
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
	/// <returns>HTTP response code, if succesful, the ID of the inserted user is returned</returns>
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
	/// <returns>HTTP response code, if succesful, the ID of the updated user is returned</returns>
	private static async Task<IResult> PutUser(User user, IUserDAO userDAO)
	{
		try
		{
			/*
			 Before doing the updating we check if the user exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any user with that ID
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
	/// <returns>Result wrapped in an HTTP response code</returns>
	private static async Task<IResult> DeleteUser(string id, IUserDAO userDAO)
	{
		try
		{
			await userDAO.DeleteUser(id);
			return Results.Ok();
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
