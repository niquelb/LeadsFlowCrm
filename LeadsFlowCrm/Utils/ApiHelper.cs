using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils;

/// <summary>
/// Helper class for interacting with the proprietary API
/// </summary>
public class ApiHelper : IApiHelper
{
	private readonly LoggedInUser _user;

	public ApiHelper(LoggedInUser user)
	{
		InitializeClient();
		_user = user;
	}

	public HttpClient ApiClient { get; set; } = new();

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

		ApiClient = new HttpClient
		{
			BaseAddress = new Uri(apiUrl)
		};

		ApiClient.DefaultRequestHeaders.Accept.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	/// <summary>
	/// Method for retrieving the necessary client secrets from the API
	/// </summary>
	/// <returns>Client secrets for the Google sign in</returns>
	public async Task<ClientSecrets?> GetGoogleClientSecretsAsync()
	{
		var config = new ConfigurationBuilder()
		.AddUserSecrets<ApiHelper>()
		.Build();

		string? apiKey = config["apiKey"];

		if (string.IsNullOrWhiteSpace(apiKey))
		{
			return null;
		}

		string formattedPath = $"Auth/ClientSecrets?apiKey={apiKey}";

		using HttpResponseMessage resp = await ApiClient.GetAsync(formattedPath);
		if (resp.IsSuccessStatusCode)
		{
			var output = await resp.Content.ReadAsAsync<ClientSecrets>();
			return output;
		}

		return null;
	}

}
