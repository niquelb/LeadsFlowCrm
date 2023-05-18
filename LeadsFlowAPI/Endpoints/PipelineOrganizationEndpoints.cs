namespace LeadsFlowAPI.Endpoints;

public static class PipelineOrganizationEndpoints
{
	/// <summary>
	/// Configures all the PipelineOrganization endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigurePipelineOrgEndpoints(this WebApplication app)
	{
		app.MapGet("/PipelineOrganizations", GetPipelineOrgs);
		app.MapGet("/PipelineOrganizations/{id}", GetPipelineOrg);
		app.MapPost("/PipelineOrganizations", PostPipelineOrg);
		app.MapPut("/PipelineOrganizations", PutPipelineOrg);
		app.MapDelete("/PipelineOrganizations", DeletePipelineOrg);
	}

	/// <summary>
	/// Endpoint for getting all the pipelineOrgs
	/// </summary>
	/// <param name="pipelineOrgDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, all the pipelineOrgs are returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetPipelineOrgs(IPipelineOrgDAO pipelineOrgDAO)
	{
		try
		{
			return Results.Ok(await pipelineOrgDAO.GetPipelineOrgs());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for getting a specific pipelineOrg based on an ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="pipelineOrgDAO">DAO object from Dependency Injection</param>
	/// [200 -> The call was successful, requested pipelineOrg is returned]
	/// [404 -> No pipelineOrg with that ID was found, used ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetPipelineOrg(String id, IPipelineOrgDAO pipelineOrgDAO)
	{
		try
		{
			var result = await pipelineOrgDAO.GetPipelineOrg(id);
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
	/// Endpoint for inserting a pipelineOrg
	/// </summary>
	/// <param name="pipelineOrg">pipelineOrg to be inserted</param>
	/// <param name="pipelineOrgDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, inserted pipelineOrg's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PostPipelineOrg(PipelineOrganization pipelineOrg, IPipelineOrgDAO pipelineOrgDAO)
	{
		try
		{
			await pipelineOrgDAO.InsertPipelineOrg(pipelineOrg);
			return Results.Ok(pipelineOrg.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for updating a pipelineOrg
	/// </summary>
	/// <param name="pipelineOrg">pipelineOrg to be updated</param>
	/// <param name="pipelineOrgDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was succesful, updated pipelineOrg's ID is returned]
	/// [404 -> No pipelineOrg with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PutPipelineOrg(PipelineOrganization pipelineOrg, IPipelineOrgDAO pipelineOrgDAO)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await pipelineOrgDAO.GetPipelineOrg(pipelineOrg.Id);
			if (result == null)
			{
				return Results.NotFound(pipelineOrg.Id);
			}

			await pipelineOrgDAO.UpdatePipelineOrg(pipelineOrg);
			return Results.Ok(pipelineOrg.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for deleting a pipelineOrg with a specified ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="pipelineOrgDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, deleted pipelineOrg's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> DeletePipelineOrg(string id, IPipelineOrgDAO pipelineOrgDAO)
	{
		try
		{
			await pipelineOrgDAO.DeletePipelineOrg(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
