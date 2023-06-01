using System.Text.Json.Serialization;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Model for the currently logged in user
/// </summary>
public class LoggedInUser
{
	/// <summary>
	/// Assigned User model
	/// </summary>
    public User AssignedUser { get; set; } = new User();

	/// <summary>
	/// Assigned user ID
	/// </summary>
    [JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// JWT token for the API
	/// </summary>
	[JsonPropertyName("token")]
	public string Token { get; set; } = string.Empty;
}
