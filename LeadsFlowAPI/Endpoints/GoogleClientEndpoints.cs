using LeadsFlowAPI.Models;
using System.Reflection;
using System.Text.Json;

namespace LeadsFlowAPI.Endpoints;

/// <summary>
/// Class for accessing the necessary information regarding the Google API client
/// </summary>
public static class GoogleClientEndpoints
{
	/// <summary>
	/// Configures all the endpoints for accessing the necessary information regarding the Google API client
	/// </summary>
	/// <param name="app">Application</param>
	public static void ConfigureGoogleClientEndpoints(this WebApplication app)
	{
		app.MapGet("Google/ClientSecrets", GetClientInfo);
	}

	/// <summary>
	/// Method for getting the "client_id"
	/// </summary>
	/// <returns>
	/// [200] -> Call successful
	/// [404] -> The necessary JSON file with the client information was not found
	/// [Any other error] -> Call failed
	/// </returns>
	public static IResult GetClientInfo()
	{
		try
		{
			/*
			 * We read and parse the contents of the JSON file that stores the client secrets for the Google API
			 */
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "LeadsFlowAPI.Resources.google_client_secrets.json";

			using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					return Results.NotFound("Configuration JSON not found.");
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
						return Results.Problem("Problem parsing the configuration JSON");
					}

					return Results.Ok(clientSecrets);
				}
			}
		}
		catch (Exception ex)
		{
			return Results.Problem(ex.Message);
		}
	}
}
