using DataAccess.DataAccess;
using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using System.Security.Cryptography;

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
		app.MapPut("/OrganizationCreator", SetupOrgCreatorRelationship);
		app.MapPut("/PipelineUser", SetupPipelineUserRelationship);
		app.MapPut("/StagePipeline", SetupStagePipelineRelathionship);
		app.MapPut("/FieldPipeline", SetupFieldPipelineRelathionship); ;
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
	public static async Task<IResult> SetupUserOrgRelationship(
		string userId,
		string orgId,
		IForeignKeyRelationships relationship,
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

			await relationship.SetupUserOrgRelationship(userId, orgId);
			return Results.Ok("Relationship created successfully");

		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Updates the organization's "CreatorId" field with the given UserId
	/// </summary>
	/// <param name="orgId">ID for the query</param>
	/// <param name="userId">ID for the query</param>
	/// <param name="relationship">DAO object from Dependency Injection</param>
	/// <param name="orgDAO">DAO object from Dependency Injection</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200] -> Relationship was successful.
	/// [404] -> User or Organization with given IDs were not found
	/// [Any other error] -> Query failed
	/// </returns>
	public static async Task<IResult> SetupOrgCreatorRelationship(string orgId,
		string userId,
		IForeignKeyRelationships relationship,
		IOrganizationDAO orgDAO,
		IUserDAO userDAO)
	{
		try
		{
			/*
			 * Before doing anything we will check if the IDs exist
			 */
			var org = await orgDAO.GetOrganization(orgId);
			var user = await userDAO.GetUser(userId);

			if (org == null)
			{
				return Results.NotFound($"Organization not found: {orgId}");
			}

			if (user == null)
			{
				return Results.NotFound($"User not found: {userId}");
			}

			await relationship.SetupOrgCreatorRelationship(orgId, userId);
			return Results.Ok("Relationship created successfully");
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Updates the pipeline's "CreatorId" field with the given UserId
	/// </summary>
	/// <param name="userId">ID for the query</param>
	/// <param name="pipelineId">ID for the query</param>
	/// <param name="relationship">DAO object from Dependency Injection</param>
	/// <param name="userDAO">DAO object from Dependency Injection</param>
	/// <param name="pipelineDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200] -> Relationship was successful.
	/// [404] -> User or Pipeline with given IDs were not found
	/// [Any other error] -> Query failed
	/// </returns>
	public static async Task<IResult> SetupPipelineUserRelationship(
		string userId,
		string pipelineId,
		IForeignKeyRelationships relationship,
		IUserDAO userDAO,
		IPipelineDAO pipelineDAO)
	{
		try
		{
			/*
			 * Before doing anything we will check if the IDs exist
			 */
			var user = await userDAO.GetUser(userId);
			var pipeline = await pipelineDAO.GetPipeline(pipelineId);

			if (pipeline == null)
			{
				return Results.NotFound($"Pipeline not found: {pipelineId}");
			}

			if (user == null)
			{
				return Results.NotFound($"User not found: {userId}");
			}

			await relationship.SetupPipelineUserRelationship(userId, pipelineId);
			return Results.Ok("Relationship created successfully");
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Updates the stage's "PipelineId" field with the given PipelineId
	/// </summary>
	/// <param name="pipelineId">ID for the query</param>
	/// <param name="stageId">ID for the query</param>
	/// <param name="relationship">DAO object from Dependency Injection</param>
	/// <param name="pipelineDAO">DAO object from Dependency Injection</param>
	/// <param name="stageDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200] -> Relationship was successful.
	/// [404] -> Stage or Pipeline with given IDs were not found
	/// [Any other error] -> Query failed
	/// </returns>
	public static async Task<IResult> SetupStagePipelineRelathionship(
		string pipelineId,
		string stageId,
		IForeignKeyRelationships relationship,
		IPipelineDAO pipelineDAO,
		IStageDAO stageDAO)
	{
		try
		{
			/*
			 * Before doing anything we will check if the IDs exist
			 */
			var pipeline = await pipelineDAO.GetPipeline(pipelineId);
			var stage = await stageDAO.GetStage(stageId);

			if (pipeline == null)
			{
				return Results.NotFound($"Pipeline not found: {pipelineId}");
			}

			if (stage == null)
			{
				return Results.NotFound($"Stage not found: {stageId}");
			}

			await relationship.SetupStagePipelineRelathionship(pipelineId, stageId);
			return Results.Ok("Relationship created successfully");
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}

	/// <summary>
	/// Updates the field's "PipelineId" field with the given PipelineId
	/// </summary>
	/// <param name="pipelineId">ID for the query</param>
	/// <param name="fieldId">ID for the query</param>
	/// <param name="relationship">DAO object from Dependency Injection</param>
	/// <param name="pipelineDAO">DAO object from Dependency Injection</param>
	/// <param name="fieldDAO">DAO object from Dependency Injection</param>
	/// <returns>
	/// [200] -> Relationship was successful.
	/// [404] -> Stage or Pipeline with given IDs were not found
	/// [Any other error] -> Query failed
	/// </returns>
	public static async Task<IResult> SetupFieldPipelineRelathionship(
		string pipelineId,
		string fieldId,
		IForeignKeyRelationships relationship,
		IPipelineDAO pipelineDAO,
		IFieldDAO fieldDAO)
	{
		try
		{
			/*
			 * Before doing anything we will check if the IDs exist
			 */
			var pipeline = await pipelineDAO.GetPipeline(pipelineId);
			var field = await fieldDAO.GetField(fieldId);

			if (pipeline == null)
			{
				return Results.NotFound($"Pipeline not found: {pipelineId}");
			}

			if (field == null)
			{
				return Results.NotFound($"Field not found: {fieldId}");
			}

			await relationship.SetupFieldPipelineRelathionship(pipelineId, fieldId);
			return Results.Ok("Relationship created successfully");
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
