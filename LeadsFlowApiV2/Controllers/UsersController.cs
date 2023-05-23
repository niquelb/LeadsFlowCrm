using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;


namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the User table
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IUserDAO _userDAO;
	private readonly IOrganizationDAO _organizationDAO;

	public UsersController(IUserDAO userDAO, IOrganizationDAO organizationDAO)
    {
		_userDAO = userDAO;
		_organizationDAO = organizationDAO;
	}

    // GET: api/<UsersController>
    [HttpGet]
	public async Task<ActionResult<IEnumerable<User>>> Get()
	{
		try
		{
			return Ok(await _userDAO.GetUsers());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<UsersController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult<string>> Get(string id)
	{
		try
		{
			var result = await _userDAO.GetUser(id);
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

	// POST api/<UsersController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] User user)
	{
		try
		{
			await _userDAO.InsertUser(user);
			return Ok(user.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<UsersController>
	[HttpPut]
	public async Task<ActionResult> Put([FromBody] User user)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _userDAO.GetUser(user.Id);
			if (result == null)
			{
				return NotFound(user.Id);
			}

			await _userDAO.UpdateUser(user);
			return Ok(user.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<UsersController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _userDAO.GetUser(id);
			if (result == null)
			{
				return NotFound(id);
			}

			await _userDAO.DeleteUser(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<UsersController>/
	[HttpPut("Organization")]
	public async Task<ActionResult> PutOrgRelationship(string userId, string orgId)
	{
		try
		{
			/*
			 * Before doing anything we will check if the IDs exist
			 */
			var user = await _userDAO.GetUser(userId);
			var org = await _organizationDAO.GetOrganization(orgId);

			if (user == null)
			{
				return NotFound($"User not found: {userId}");
			}

			if (org == null)
			{
				return NotFound($"Organization not found: {orgId}");
			}

			await _userDAO.SetupUserOrgRelationship(userId, orgId);
			return Ok("Relationship created successfully");

		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
