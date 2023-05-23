using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the Field table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class FieldController : ControllerBase
{
	private readonly IFieldDAO _fieldDAO;

	public FieldController(IFieldDAO fieldDAO)
    {
		_fieldDAO = fieldDAO;
	}

    // GET: api/<FieldController>
    [HttpGet]
	public async Task<ActionResult> Get()
	{
		try
		{
			return Ok(await _fieldDAO.GetFields());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<FieldController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
	{
		try
		{
			var result = await _fieldDAO.GetField(id);
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

	// POST api/<FieldController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] Field field)
	{
		try
		{
			await _fieldDAO.InsertField(field);
			return Ok(field.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<FieldController>
	[HttpPut]
	public async Task<ActionResult> Put([FromBody] Field field)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _fieldDAO.GetField(field.Id);
			if (result == null)
			{
				return NotFound(field.Id);
			}

			await _fieldDAO.UpdateField(field);
			return Ok(field.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<FieldController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			await _fieldDAO.DeleteField(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
