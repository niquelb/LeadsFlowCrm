using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for the Contact table
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "base")]
public class ContactsController : ControllerBase
{
	private readonly IContactDAO _contactDAO;

	public ContactsController(IContactDAO contactDAO)
    {
		_contactDAO = contactDAO;
	}

    // GET: api/<ContactsController>
    [HttpGet]
	public async Task<ActionResult> Get()
	{
		try
		{
			return Ok(await _contactDAO.GetContacts());
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// GET api/<ContactsController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult> Get(string id)
	{
		try
		{
			var result = await _contactDAO.GetContact(id);
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

	// POST api/<ContactsController>
	[HttpPost]
	public async Task<ActionResult> Post([FromBody] Contact contact)
	{
		try
		{
			await _contactDAO.InsertContact(contact);
			return Ok(contact.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// PUT api/<ContactsController>
	[HttpPut]
	public async Task<ActionResult> Put([FromBody] Contact contact)
	{
		try
		{
			/*
			Before doing the updating we check if the entry exists, this is done because otherwise
			the API will return 200 even if the updating failed due to not being any entries with that ID
			*/
			var result = await _contactDAO.GetContact(contact.Id);
			if (result == null)
			{
				return NotFound(contact.Id);
			}

			await _contactDAO.UpdateContact(contact);
			return Ok(contact.Id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	// DELETE api/<ContactsController>/5
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		try
		{
			await _contactDAO.DeleteContact(id);
			return Ok(id);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
