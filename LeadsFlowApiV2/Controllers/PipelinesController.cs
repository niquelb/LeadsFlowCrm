using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the Pipeline table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class PipelinesController : ControllerBase
{
	private readonly IPipelineDAO _pipelineDAO;

	public PipelinesController(IPipelineDAO pipelineDAO)
    {
		_pipelineDAO = pipelineDAO;
	}

    // GET: api/<PipelinesController>
    [HttpGet]
	public async Task<ActionResult> Get()
	{
		try
		{
			return Ok(await _pipelineDAO.GetPipelines());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<PipelinesController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
	{
		try
		{
			var result = await _pipelineDAO.GetPipeline(id);
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

	// POST api/<PipelinesController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] Pipeline pipeline)
	{
		try
		{
			await _pipelineDAO.InsertPipeline(pipeline);
			return Ok(pipeline.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<PipelinesController>
	[HttpPut]
	public async Task<ActionResult> Put([FromBody] Pipeline pipeline)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _pipelineDAO.GetPipeline(pipeline.Id);
			if (result == null)
			{
				return NotFound(pipeline.Id);
			}

			await _pipelineDAO.UpdatePipeline(pipeline);
			return Ok(pipeline.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<PipelinesController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			await _pipelineDAO.DeletePipeline(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
