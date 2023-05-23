﻿using Google.Apis.Auth.OAuth2;
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
	private HttpClient _apiClient = new();
	private readonly LoggedInUser _user;

	public ApiHelper(LoggedInUser user)
	{
		InitializeClient();
		_user = user;
	}

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

		_apiClient = new HttpClient();
		_apiClient.BaseAddress = new Uri(apiUrl);
		_apiClient.DefaultRequestHeaders.Accept.Clear();
		_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	/// <summary>
	/// Method for logging into the API and retrieving and storing the token in the LoggedInUser model
	/// </summary>
	/// <param name="OAuthToken">Google OAuth token from the Google sign in</param>
	/// <param name="UserName">User's email</param>
	/// <returns></returns>
	/// <exception cref="UnauthorizedAccessException">If login call to API fails, most likely due to an invalid OAuth token</exception>
	/// <see cref="LoggedInUser"/>
	public async Task Authenticate(string OAuthToken, string UserName)
	{
		string formattedUrl = $"Auth/Login?OAuthToken={OAuthToken}&UserName={UserName}";

		using HttpResponseMessage resp = await _apiClient.PostAsync(formattedUrl, null);
		if (resp.IsSuccessStatusCode == false)
		{
			throw new UnauthorizedAccessException(resp.ReasonPhrase);
		}

		var output = await resp.Content.ReadAsAsync<string>();

		_user.Token = output;
	}

	/// <summary>
	/// Method for retrieving the necessary client secrets from the API
	/// </summary>
	/// <returns>Client secrets for the Google sign in</returns>
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

		string formattedPath = $"api/ClientSecrets?apiKey={apiKey}";

		using HttpResponseMessage resp = await _apiClient.GetAsync(formattedPath);
		if (resp.IsSuccessStatusCode)
		{
			var output = await resp.Content.ReadAsAsync<ClientSecrets>();
			return output;
		}

		return null;
	}

	public void Dispose()
	{
		_apiClient.Dispose();
	}
}
