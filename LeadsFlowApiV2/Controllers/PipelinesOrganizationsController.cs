using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the PipelineOrganization table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class PipelinesOrganizationsController : ControllerBase
{
	private readonly IPipelineOrgDAO _pipelineOrgDAO;

	public PipelinesOrganizationsController(IPipelineOrgDAO pipelineOrgDAO)
    {
		_pipelineOrgDAO = pipelineOrgDAO;
	}

    // GET: api/<PipelinesOrganizationsController>
    [HttpGet]
	public async Task<ActionResult> Get()
	{
		try
		{
			return Ok(await _pipelineOrgDAO.GetPipelineOrgs());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<PipelinesOrganizationsController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
	{
		try
		{
			var result = await _pipelineOrgDAO.GetPipelineOrg(id);
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

	// POST api/<PipelinesOrganizationsController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] PipelineOrganization pipelineOrganization)
	{
		try
		{
			await _pipelineOrgDAO.InsertPipelineOrg(pipelineOrganization);
			return Ok(pipelineOrganization.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<PipelinesOrganizationsController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			await _pipelineOrgDAO.DeletePipelineOrg(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
