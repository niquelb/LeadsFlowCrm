using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the Stage table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class StagesController : ControllerBase
{
	private readonly IStageDAO _stageDAO;

	public StagesController(IStageDAO stageDAO)
	{
		_stageDAO = stageDAO;
	}

	// GET: api/<StagesController>?query=<WHERE-CLAUSE>
	[HttpGet]
	public async Task<ActionResult> GetAll(string? query)
	{
		try
		{
			if (string.IsNullOrEmpty(query) == false)
			{
				return Ok(await _stageDAO.GetStages(query));
			}

			return Ok(await _stageDAO.GetStages());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<StagesController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
	{
		try
		{
			var result = await _stageDAO.GetStage(id);
			if (result == null)
			{
				return NotFound(id);
			}
			return Ok(result);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// POST api/<StagesController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] Stage stage)
	{
		try
		{
			await _stageDAO.InsertStage(stage);
			return Ok(stage.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<StagesController>
	[HttpPut]
	public async Task<ActionResult> Put([FromBody] Stage stage)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _stageDAO.GetStage(stage.Id);
			if (result == null)
			{
				return NotFound(stage.Id);
			}

			await _stageDAO.UpdateStage(stage);
			return Ok(stage.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<StagesController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			await _stageDAO.DeleteStage(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
