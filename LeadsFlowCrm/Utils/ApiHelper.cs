using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils;

/// <summary>
/// Helper class for interacting with the proprietary API
/// </summary>
public class ApiHelper : IDisposable, IApiHelper
{
	private HttpClient apiClient;

	public ApiHelper() => InitializeClient();

	/// <summary>
	/// Method for instanciating our HTTP client
	/// </summary>
	private void InitializeClient()
	{
		string? apiUrl = ConfigurationManager.AppSettings["api"];

		if (string.IsNullOrWhiteSpace(apiUrl))
		{
			throw new Exception("Failure to read AppSettings.json. -> Failure to get the API URL.");
		}

		apiClient = new HttpClient();
		apiClient.BaseAddress = new Uri(apiUrl);
		apiClient.DefaultRequestHeaders.Accept.Clear();
		apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public void Authenticate(string OauthToken)
	{

	}

	public async Task<ClientSecrets?> GetGoogleClientSecrets()
	{
		using (HttpResponseMessage resp = await apiClient.GetAsync("Google/ClientSecrets"))
		{
			if (resp.IsSuccessStatusCode)
			{
				var output = await resp.Content.ReadAsAsync<ClientSecrets>();
				return output;
			}

			return null;
		}
	}

	public void Dispose()
	{
		apiClient.Dispose();
	}
}
