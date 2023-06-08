using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices;

/// <summary>
/// Service class for the Pipeline model
/// </summary>
/// <see cref="Pipeline"/>
public class PipelineService : IPipelineService
{
	private readonly HttpClient _apiClient;
	private readonly IStageService _stageService;
	private readonly LoggedInUser _loggedInUser;

	public PipelineService(IApiHelper apiHelper,
						IStageService stageService,
						LoggedInUser loggedInUser)
	{
		_apiClient = apiHelper.ApiClient;
		_stageService = stageService;
		_loggedInUser = loggedInUser;
	}

	/// <summary>
	/// Method for getting a specific Pipeline
	/// </summary>
	/// <returns></returns>
	/// <exception cref="UnauthorizedAccessException"></exception>
	/// <exception cref="Exception"></exception>
	public async Task<Pipeline> GetPipelineAsync()
	{
		// TODO: Filter by Org
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loggedInUser.Token);

		// TODO: replace hard-coded ID
		using HttpResponseMessage resp = await _apiClient.GetAsync($"api/Pipelines/a5ab0ed1-1f36-4c4c-993f-acd7a09bdf70");
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}

			throw new Exception(resp.ReasonPhrase);
		}

		var output = await resp.Content.ReadAsAsync<Pipeline>();

		// Fill in the stages
		output.Stages = await _stageService.GetStagesByPipelineAsync(output.Id);

		return output;
	}

}
