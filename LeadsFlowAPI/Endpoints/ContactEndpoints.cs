using DataAccess.DAO;

namespace LeadsFlowAPI.Endpoints;

public static class ContactEndpoints
{
	/// <summary>
	/// Configures all the Contact endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureContactEndpoints(this WebApplication app)
	{
		app.MapGet("/Contacts", GetContacts);
		app.MapGet("/Contacts/{id}", GetContact);
		app.MapPost("/Contacts", PostContact);
		app.MapPut("/Contacts", PutContact);
		app.MapDelete("/Contact", DeleteContact);
	}

	/// <summary>
	/// Endpoint for getting all the contacts
	/// </summary>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, all the contacts are returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetContacts(IContactDAO contactDAO)
	{
		try
		{
			return Results.Ok(await contactDAO.GetContacts());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for getting a specific contact based on an ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// [200 -> The call was successful, requested contact is returned]
	/// [404 -> No contact with that ID was found, used ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetContact(String id, IContactDAO contactDAO)
	{
		try
		{
			var result = await contactDAO.GetContact(id);
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
	/// Endpoint for inserting a contact
	/// </summary>
	/// <param name="contact">contact to be inserted</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, inserted contact's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PostContact(Contact contact, IContactDAO contactDAO)
	{
		try
		{
			await contactDAO.InsertContact(contact);
			return Results.Ok(contact.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for updating a contact
	/// </summary>
	/// <param name="contact">contact to be updated</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was succesful, updated contact's ID is returned]
	/// [404 -> No contact with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PutContact(Contact contact, IContactDAO contactDAO)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await contactDAO.GetContact(contact.Id);
			if (result == null)
			{
				return Results.NotFound(contact.Id);
			}

			await contactDAO.UpdateContact(contact);
			return Results.Ok(contact.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for deleting a contact with a specified ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="contactDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, deleted contact's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> DeleteContact(string id, IContactDAO contactDAO)
	{
		try
		{
			await contactDAO.DeleteContact(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
