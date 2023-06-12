using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.PeopleService.v1.Data;
using System.Windows.Markup;
using System.Text.Json;

namespace LeadsFlowCrm.Services.ModelServices;

/// <summary>
/// Service class for the Contact model
/// </summary>
public class ContactService : IContactService
{
	private readonly HttpClient _apiClient;
	private readonly IPeopleServiceClass _peopleService;
	private readonly LoggedInUser _loggedInUser;

	public ContactService(IApiHelper apiHelper,
					   IPeopleServiceClass peopleService,
					   LoggedInUser loggedInUser)
	{
		_apiClient = apiHelper.ApiClient;
		_peopleService = peopleService;
		_loggedInUser = loggedInUser;
	}

	/// <summary>
	/// Method for retrieving all the contacts associated with the user from the API
	/// </summary>
	/// <param name="userId">User's ID</param>
	/// <returns>List of all contacts associated with the user</returns>
	/// <exception cref="UnauthorizedAccessException">If the token is incorrect</exception>
	/// <exception cref="Exception">If there was another issue with the API or the request</exception>
	public async Task<IList<Contact>> GetByUserAsync(string userId)
	{
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loggedInUser.Token);

		/*
		 * We make the query for filtering by the Stage ID, for this we need to encode it
		 * 
		 * The final URL should look like this without encoding → "[URL]/Contacts?query=stageId='ID'
		 */
		string encodedPipelineId = WebUtility.UrlEncode(userId);
		string query = $"userId = '{encodedPipelineId}'";

		using HttpResponseMessage resp = await _apiClient.GetAsync($"api/Contacts?query={query}");
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}

			throw new Exception(resp.ReasonPhrase);
		}

		var output = await resp.Content.ReadAsAsync<IList<Contact>>();

		return output;
	}

	/// <summary>
	/// Method for retrieving the contacts that match the given Stage ID from the API
	/// </summary>
	/// <param name="stageId">Stage ID</param>
	/// <returns>List of desired contacts</returns>
	/// <exception cref="UnauthorizedAccessException">If the token is incorrect</exception>
	/// <exception cref="Exception">If there was another issue with the API or the request</exception>
	public async Task<IList<Contact>> GetByStageAsync(string stageId)
	{
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loggedInUser.Token);

		/*
		 * We make the query for filtering by the Stage ID, for this we need to encode it
		 * 
		 * The final URL should look like this without encoding → "[URL]/Contacts?query=stageId='ID'
		 */
		string encodedPipelineId = WebUtility.UrlEncode(stageId);
		string query = $"stageId = '{encodedPipelineId}'";

		using HttpResponseMessage resp = await _apiClient.GetAsync($"api/Contacts?query={query}");
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}

			throw new Exception(resp.ReasonPhrase);
		}

		var output = await resp.Content.ReadAsAsync<IList<Contact>>();

		return output;
	}

	/// <summary>
	/// Method for retrieving the user's Google contacts from the People API and converting them to a ContactModel
	/// </summary>
	/// <returns>List of contacts from the user's Google account</returns>
	public async Task<IList<Contact>> GetFromPeopleApiAsync()
	{
		var output = new List<Contact>();
		
		// We retrieve the contacts from the user
		var people = await _peopleService.GetPeopleAsync();

		// We parse them into Contact objects
		foreach (var person in people)
        {
			var contact = new Contact()
			{
				Email = person.EmailAddresses?.FirstOrDefault()?.Value ?? "-",
				FirstName = person.Names?.FirstOrDefault()?.DisplayName ?? "-",
				Phone = person.PhoneNumbers?.FirstOrDefault()?.Value ?? "-"
			};

			output.Add(contact);
        }

		return output;
    }

	/// <summary>
	/// Method for uploading the given Contact to the API
	/// </summary>
	/// <param name="contact">Contact object</param>
	/// <param name="StageId">ID for the stage the contact belongs to</param>
	/// <param name="UserId">ID of the user that created/uploaded the contact</param>
	/// <returns></returns>
	/// <exception cref="UnauthorizedAccessException">If the token is invalid/exception>
	/// <exception cref="Exception">If there is any other problem with the request</exception>
	public async Task PostToApiAsync(Contact contact, string UserId, string? StageId = null)
	{
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loggedInUser.Token);

		/*
		 Expected json for the body:
			{
				"id": "string",			← This is automatically generated, not needed
				"email": "string",
				"firstName": "string",
				"lastNames": "string",
				"phone": "string",
				"address": "string",
				"company": "string",
				"jobTitle": "string",
				"website": "string",
				"location": "string",
				"notes": "string",
				"stageId": "string",
				"userId": "string"
			}
		*/

		// We create the body of the request
		var bodyMap = new Dictionary<string, string?>
		{
			["email"] = contact.Email,
			["firstName"] = contact.FirstName,
			["lastNames"] = contact.LastNames,
			["phone"] = contact.Phone,
			["address"] = null, //TODO ←
			["company"] = null,
			["jobTitle"] = null,
			["location"] = null,
			["notes"] = contact.Notes,
			["stageId"] = StageId,
			["userId"] = UserId
		};

		// We parse the body to a Json string
		string bodyStr = JsonSerializer.Serialize(bodyMap);

		// We create the content object for the request
		StringContent body = new(bodyStr, Encoding.UTF8, "application/json");

		using HttpResponseMessage resp = await _apiClient.PostAsync($"api/Contacts", body);
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}

			throw new Exception(resp.ReasonPhrase);
		}
	}
}
