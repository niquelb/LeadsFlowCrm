using System.Text.Json.Serialization;

namespace LeadsFlowCrm.Models;

public class LoggedInUser
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;
	[JsonPropertyName("token")]
	public string Token { get; set; } = string.Empty;
}
