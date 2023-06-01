namespace LeadsFlowCrm.Models;

/// <summary>
/// User model
/// </summary>
public class User
{
	/// <summary>
	/// ID
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Google OAuth 2.0 token
	/// </summary>
	public string OAuthToken { get; set; } = string.Empty;

	/// <summary>
	/// Primary email address
	/// </summary>
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// Display name
	/// </summary>
	public string DisplayName { get; set; } = string.Empty;

	// TODO: change this to be an org model
	/// <summary>
	/// Organization that this user belongs to
	/// </summary>
	public string OrganizationId { get; set; } = string.Empty;
}
