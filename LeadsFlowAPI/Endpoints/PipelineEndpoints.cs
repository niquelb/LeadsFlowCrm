using DataAccess.DAO;

namespace LeadsFlowAPI.Endpoints;

public static class PipelineEndpoints
{
	public static void ConfigurePipelineEndpoints(this WebApplication app)
	{
		app.MapGet("/Pipelines", GetPipelines);
		app.MapGet("/Pipelines/{id}", GetPipeline);
		app.MapPost("/Pipelines", PostPipeline);
		app.MapPut("/Pipelines", PutPipeline);
		app.MapDelete("/Pipelines", DeletePipeline);
	}

	private static async Task<IResult> GetPipelines(IPipelineDAO pipelineDAO)
	{
		try
		{
			return Results.Ok(await pipelineDAO.GetPipelines());
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	private static async Task<IResult> GetPipeline(string id, IPipelineDAO pipelineDAO)
	{
		try
		{
			var result = await pipelineDAO.GetPipeline(id);
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

	private static async Task<IResult> PostPipeline(Pipeline pipeline, IPipelineDAO pipelineDAO)
	{
		try
		{
			await pipelineDAO.InsertPipeline(pipeline);
			return Results.Ok(pipeline.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	private static async Task<IResult> PutPipeline(Pipeline pipeline, IPipelineDAO pipelineDAO)
	{
		try
		{
			/*
			 Before doing the updating we check if the user exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any user with that ID
			*/
			var result = await pipelineDAO.GetPipeline(pipeline.Id);
			if (result == null)
			{
				return Results.NotFound(pipeline.Id);
			}

			await pipelineDAO.UpdatePipeline(pipeline);
			return Results.Ok(pipeline.Id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	private static async Task<IResult> DeletePipeline(string id, IPipelineDAO pipelineDAO)
	{
		try
		{
			await pipelineDAO.DeletePipeline(id);
			return Results.Ok(id);
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
