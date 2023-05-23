using System.Text.Json.Serialization;

namespace LeadsFlowApiV2.Models;

/// <summary>
/// Model class that contains the Client ID and Client Secret for the Google Oauth system.
/// 
/// It is NOT Google's own ClientSecrets class
/// </summary>
public class ClientSecrets
{
	/// <summary>
	/// Client ID
	/// </summary>
	[JsonPropertyName("client_id")]
	public string ClientId { get; set; }

	/// <summary>
	/// Client secret
	/// </summary>
	[JsonPropertyName("client_secret")]
	public string ClientSecret { get; set; }
}
