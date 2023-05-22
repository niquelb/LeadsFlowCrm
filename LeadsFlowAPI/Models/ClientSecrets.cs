using System.Text.Json.Serialization;

namespace LeadsFlowAPI.Models;

public class ClientSecrets
{
	[JsonPropertyName("client_id")]
	public string ClientId { get; set; }
	[JsonPropertyName("client_secret")]
	public string ClientSecret { get; set; }
	[JsonPropertyName("redirect_uris")]
	public IEnumerable<string> RedirectUris { get; set; }
}
