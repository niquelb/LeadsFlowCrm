using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		string? apiUrl = System.Configuration.ConfigurationManager.AppSettings["api"];

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
		var config = new ConfigurationBuilder()
		.AddUserSecrets<ApiHelper>()
		.Build();

		string? apiKey = config["apiKey"];

		if (string.IsNullOrWhiteSpace(apiKey))
		{
			return null;
		}

		Trace.WriteLine(apiKey);

		string formattedPath = $"api/ClientSecrets?apiKey={apiKey}";

		using (HttpResponseMessage resp = await apiClient.GetAsync(formattedPath))
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
