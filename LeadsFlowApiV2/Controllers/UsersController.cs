using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the User table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class UsersController : ControllerBase
{
	private readonly IUserDAO _userDAO;
	private readonly IOrganizationDAO _organizationDAO;

	public UsersController(IUserDAO userDAO, IOrganizationDAO organizationDAO)
    {
		_userDAO = userDAO;
		_organizationDAO = organizationDAO;
	}

	// GET: api/<UsersController>?query=<WHERE-CLAUSE>
	[HttpGet]
	public async Task<ActionResult> GetAll(string? query)
	{
		try
		{
			if (string.IsNullOrEmpty(query) == false)
			{
				return Ok(await _userDAO.GetUsers(query));
			}

			return Ok(await _userDAO.GetUsers());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<UsersController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
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
            if (string.IsNullOrWhiteSpace(user.Id))
            {
				return NotFound(user.Id);
			}

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
}
