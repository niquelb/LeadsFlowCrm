using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the Organization table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class OrganizationsController : ControllerBase
{
	private readonly IOrganizationDAO _organizationDAO;

	public OrganizationsController(IOrganizationDAO organizationDAO)
        {
		_organizationDAO = organizationDAO;
	}

        // GET: api/<OrganizationsController>
        [HttpGet]
	public async Task<ActionResult> Get()
	{
		try
		{
			return Ok(await _organizationDAO.GetOrganizations());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<OrganizationsController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
	{
		try
		{
			var result = await _organizationDAO.GetOrganization(id);
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

	// POST api/<OrganizationsController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] Organization organization)
	{
		try
		{
			await _organizationDAO.InsertOrganization(organization);
			return Ok(organization.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<OrganizationsController>
	[HttpPut]
	public async Task<ActionResult> Put([FromBody] Organization organization)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _organizationDAO.GetOrganization(organization.Id);
			if (result == null)
			{
				return NotFound(organization.Id);
			}

			await _organizationDAO.UpdateOrganization(organization);
			return Ok(organization.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<OrganizationsController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			await _organizationDAO.DeleteOrganization(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
