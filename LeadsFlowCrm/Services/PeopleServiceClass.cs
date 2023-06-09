using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services;

public class PeopleServiceClass : IPeopleServiceClass
{
	private readonly IBaseGoogleServiceClass _baseGoogleService;
	private IList<Person>? _people;
	private PeopleServiceService? _service;

	public PeopleServiceClass(IBaseGoogleServiceClass baseGoogleServiceClass)
	{
		_baseGoogleService = baseGoogleServiceClass;
	}

	/// <summary>
	/// Method for retrieving the PeopleService service obect from the People API
	/// </summary>
	/// <returns>PeopleServiceService object (who came up with that name)</returns>
	/// <see cref="PeopleServiceService"/>
	public async Task<PeopleServiceService> GetPeopleServiceAsync()
	{
		if (_service == null)
		{
			_service = new PeopleServiceService(await _baseGoogleService.GetServiceAsync());
		}

		return _service;
	}

	/// <summary>
	/// Method for getting a list of Person objects which represent the user's contacts from the People API
	/// </summary>
	/// <returns>List of Person objects</returns>
	/// <see cref="Person"/>
	public async Task<List<Person>> GetPeopleAsync()
	{
		if (_people == null)
		{
			_people = await RetrievePeopleAsync();
        }

		return _people.ToList();
	}

	/// <summary>
	/// Method for retrieving the contacts from the user using the People API
	/// </summary>
	/// <returns>List of Person obects</returns>
	private async Task<IList<Person>> RetrievePeopleAsync()
	{
		// We retrieve the service
		var service = await GetPeopleServiceAsync();

		// We fetch and process the contacts
		var connections = service.People.Connections.List("people/me");
		connections.RequestMaskIncludeField = "person.names,person.emailAddresses"; // ← Here we specify the info we want from the contacts

		return (await connections.ExecuteAsync()).Connections;
	} 

}
