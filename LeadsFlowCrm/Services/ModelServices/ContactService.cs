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

namespace LeadsFlowCrm.Services.ModelServices;

/// <summary>
/// Service class for the Contact model
/// </summary>
public class ContactService : IContactService
{
	private readonly HttpClient _apiClient;
	private readonly LoggedInUser _loggedInUser;

	public ContactService(
		IApiHelper apiHelper,
		LoggedInUser loggedInUser)
	{
		_apiClient = apiHelper.ApiClient;
		_loggedInUser = loggedInUser;
	}

	/// <summary>
	/// Method for retrieving all the contacts from the API
	/// </summary>
	/// <returns>List of all contacts</returns>
	/// <exception cref="UnauthorizedAccessException">If the token is incorrect</exception>
	/// <exception cref="Exception">If there was another issue with the API or the request</exception>
	public async Task<IList<Contact>> GetAllAsync()
	{
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loggedInUser.Token);

		using HttpResponseMessage resp = await _apiClient.GetAsync($"api/Contacts");
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
}
