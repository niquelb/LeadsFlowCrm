using DataAccess.DAO;

namespace LeadsFlowAPI.Endpoints;

public static class FieldEndpoints
{
	/// <summary>
	/// Configures all the Field endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureFieldEndpoints(this WebApplication app)
	{
		app.MapGet("/Fields", GetFields);
		app.MapGet("/Fields/{id}", GetField);
		app.MapPost("/Fields", PostField);
		app.MapPut("/Fields", PutField);
		app.MapDelete("/Fields", DeleteField);
	}

	/// <summary>
	/// Endpoint for getting all the fields
	/// </summary>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, all the fields are returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetFields(IFieldDAO fieldDAO)
	{
		try
		{
			return Results.Ok(await fieldDAO.GetFields());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for getting a specific field based on an ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// [200 -> The call was successful, requested field is returned]
	/// [404 -> No field with that ID was found, used ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetField(string id, IFieldDAO fieldDAO)
	{
		try
		{
			var result = await fieldDAO.GetField(id);
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
	/// Endpoint for inserting a field
	/// </summary>
	/// <param name="contact">field to be inserted</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, inserted field's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PostField(Field field, IFieldDAO fieldDAO)
	{
		try
		{
			await fieldDAO.InsertField(field);
			return Results.Ok(field.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for updating a field
	/// </summary>
	/// <param name="contact">field to be updated</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was succesful, updated field's ID is returned]
	/// [404 -> No field with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PutField(Field field,  IFieldDAO fieldDAO)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await fieldDAO.GetField(field.Id);
			if (result == null)
			{
				return Results.NotFound(field.Id);
			}

			await fieldDAO.UpdateField(field);
			return Results.Ok(field.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for deleting a field with a specified ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, deleted field's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> DeleteField(string id, IFieldDAO fieldDAO)
	{
		try
		{
			await fieldDAO.DeleteField(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
