using LeadsFlowApiV2.Auth;
using LeadsFlowApiV2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeadsFlowApiV2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientSecretsController : ControllerBase
	{
		private readonly IAuthFilter _authFilter;

		public ClientSecretsController(IAuthFilter authFilter)
        {
			_authFilter = authFilter;
		}

        // GET: api/<ClientSecretsController>
        [HttpGet]
		public ActionResult Get(string apiKey)
		{
			/*
			 * We first check if the provided API key is valid for the request
			 */
			if (_authFilter.CheckApiKey(apiKey) == false)
			{
				return Unauthorized("Invalid API Key");
			}

			try
			{
				/*
				 * We read and parse the contents of the JSON file that stores the client secrets for the Google API
				 */
				var assembly = Assembly.GetExecutingAssembly();
				var resourceName = "LeadsFlowApiV2.Resources.client_secrets.json";

				using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
				{
					if (stream == null)
					{
						return NotFound("Configuration JSON not found.");
					}

					using (StreamReader reader = new(stream))
					{
						string jsonContents = reader.ReadToEnd();

						JsonDocument jsonDocument = JsonDocument.Parse(jsonContents);

						JsonElement root = jsonDocument.RootElement;
						JsonElement installedElement = root.GetProperty("installed");

						ClientSecrets? clientSecrets = installedElement.Deserialize<ClientSecrets>();

						if (clientSecrets == null)
						{
							return Problem("Problem parsing the configuration JSON");
						}

						return Ok(clientSecrets);
					}
				}
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
