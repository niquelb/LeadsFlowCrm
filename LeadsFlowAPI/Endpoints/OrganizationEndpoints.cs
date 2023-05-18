namespace LeadsFlowAPI.Endpoints;

public static class OrganizationEndpoints
{
	/// <summary>
	/// Configures all the Organization endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureOrganizationEndpoints(this WebApplication app)
	{
		app.MapGet("/Organizations", GetOrganizations);
		app.MapGet("/Organizations/{id}", GetOrganization);
		app.MapPost("/Organizations", PostOrganization);
		app.MapPut("/Organizations", PutOrganization);
		app.MapDelete("/Organizations", DeleteOrganization);
	}

	/// <summary>
	/// Endpoint for getting all the organizations
	/// </summary>
	/// <param name="organizationDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, all the organizations are returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetOrganizations(IOrganizationDAO organizationDAO)
	{
		try
		{
			return Results.Ok(await organizationDAO.GetOrganizations());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for getting a specific organization based on an ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="organizationDAO">DAO object from Dependency Injection</param>
	/// [200 -> The call was successful, requested organization is returned]
	/// [404 -> No organization with that ID was found, used ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetOrganization(String id, IOrganizationDAO organizationDAO)
	{
		try
		{
			var result = await organizationDAO.GetOrganization(id);
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
	/// Endpoint for inserting an organization
	/// </summary>
	/// <param name="organization">Organization to be inserted</param>
	/// <param name="organizationDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, inserted organization's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PostOrganization(Organization organization, IOrganizationDAO organizationDAO)
	{
		try
		{
			await organizationDAO.InsertOrganization(organization);
			return Results.Ok(organization.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for updating an organization
	/// </summary>
	/// <param name="organization">Organization to be updated</param>
	/// <param name="organizationDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was succesful, updated organization's ID is returned]
	/// [404 -> No organization with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PutOrganization(Organization organization, IOrganizationDAO organizationDAO)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await organizationDAO.GetOrganization(organization.Id);
			if (result == null)
			{
				return Results.NotFound(organization.Id);
			}

			await organizationDAO.UpdateOrganization(organization);
			return Results.Ok(organization.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for deleting an organization with a specified ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="organizationDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, deleted organization's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> DeleteOrganization(string id, IOrganizationDAO organizationDAO)
	{
		try
		{
			await organizationDAO.DeleteOrganization(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
