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

	public Organization Organization { get; set; } = new();

    /// <summary>
    /// ID for Organization that this user belongs to.
	/// 
	/// This is mainly for parsing the response from the API which returns this ID as a Foreign Key.
    /// </summary>
    public string OrganizationId { get; set; } = string.Empty;
}
