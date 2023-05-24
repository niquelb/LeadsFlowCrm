using System.Text.Json.Serialization;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Model for the currently logged in user
/// </summary>
public class LoggedInUser
{
	[JsonPropertyName("id")]
	public string Id { get; set; }
	[JsonPropertyName("token")]
	public string Token { get; set; }
}
