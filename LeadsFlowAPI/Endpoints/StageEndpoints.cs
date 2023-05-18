namespace LeadsFlowAPI.Endpoints;

public static class StageEndpoints
{
	/// <summary>
	/// Configures all the Field endpoints for the API
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureStageEndpoints(this WebApplication app)
	{
		app.MapGet("/Stages", GetStages);
		app.MapGet("/Stages/{id}", GetStage);
		app.MapPost("/Stages", PostStage);
		app.MapPut("/Stages", PutStage);
		app.MapDelete("/Stages", DeleteStage);
	}

	/// <summary>
	/// Endpoint for getting all the stages
	/// </summary>
	/// <param name="stageDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, all the stages are returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetStages(IStageDAO stageDAO)
	{
		try
		{
			return Results.Ok(await stageDAO.GetStages());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for getting a specific stage based on an ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="stageDAO">DAO object from Dependency Injection</param>
	/// [200 -> The call was successful, requested stage is returned]
	/// [404 -> No stage with that ID was found, used ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> GetStage(String id, IStageDAO stageDAO)
	{
		try
		{
			var result = await stageDAO.GetStage(id);
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
	/// Endpoint for inserting a stage
	/// </summary>
	/// <param name="stage">stage to be inserted</param>
	/// <param name="stageDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, inserted stage's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PostStage(Stage stage, IStageDAO stageDAO)
	{
		try
		{
			await stageDAO.InsertStage(stage);
			return Results.Ok(stage.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for updating a stage
	/// </summary>
	/// <param name="stage">stage to be updated</param>
	/// <param name="stageDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was succesful, updated stage's ID is returned]
	/// [404 -> No stage with that ID was found, used ID is return]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> PutStage(Stage stage, IStageDAO stageDAO)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await stageDAO.GetStage(stage.Id);
			if (result == null)
			{
				return Results.NotFound(stage.Id);
			}

			await stageDAO.UpdateStage(stage);
			return Results.Ok(stage.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Endpoint for deleting a stage with a specified ID
	/// </summary>
	/// <param name="id">ID for the query</param>
	/// <param name="stageDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200 -> The call was successful, deleted stage's ID is returned]
	/// [Any other error -> Call failed]
	/// </returns>
	private static async Task<IResult> DeleteStage(string id, IStageDAO stageDAO)
	{
		try
		{
			await stageDAO.DeleteStage(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
