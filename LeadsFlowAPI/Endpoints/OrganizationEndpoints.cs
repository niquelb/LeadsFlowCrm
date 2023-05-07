using DataAccess.DAO;

namespace LeadsFlowAPI.Endpoints;

public static class OrganizationEndpoints
{
	public static void ConfigureOrganizationEndpoints(this WebApplication app)
	{
		app.MapGet("/Organizations", GetOrganizations);
		app.MapGet("/Organizations/{id}", GetOrganization);
		app.MapPost("/Organizations", PostOrganization);
		app.MapPut("/Organizations", PutOrganization);
		app.MapDelete("/Organizations", DeleteOrganization);
	}

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

	private static async Task<IResult> PutOrganization(Organization organization, IOrganizationDAO organizationDAO)
	{
		try
		{
			/*
			 Before doing the updating we check if the organization exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any organization with that ID
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
