using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;

namespace LeadsFlowCrm.Services.ModelServices;

/// <summary>
/// Service class for the Stage model
/// </summary>
/// <see cref="Stage"/>
public class StageService : IStageService
{
	private readonly LoggedInUser _loggedInUser;
	private readonly HttpClient _apiClient;

	public StageService(IApiHelper apiHelper,
					 LoggedInUser loggedInUser)
	{
		_loggedInUser = loggedInUser;
		_apiClient = apiHelper.ApiClient;
	}

	/// <summary>
	/// Method for getting the stages of a given Pipeline
	/// </summary>
	/// <param name="pipelineId">Pipeline ID</param>
	/// <returns>A list of the pipeline's stages</returns>
	/// <exception cref="UnauthorizedAccessException">If the token is invalid</exception>
	/// <exception cref="Exception">If there's an error in the request</exception>
	public async Task<IList<Stage>> GetByPipelineAsync(string pipelineId)
	{
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loggedInUser.Token);

		/*
		 * We make the query for filtering by the Pipeline ID, for this we need to encode it
		 * 
		 * The final URL should look like this without encoding → "[URL]/Stages?query=pipelineId='ID'
		 */
		string encodedPipelineId = WebUtility.UrlEncode(pipelineId);
		string query = $"pipelineId = '{encodedPipelineId}'";

		using HttpResponseMessage resp = await _apiClient.GetAsync($"api/Stages?query={query}");
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}

			throw new Exception(resp.ReasonPhrase);
		}

		var output = await resp.Content.ReadAsAsync<IList<Stage>>();

		return output;
	}
}
