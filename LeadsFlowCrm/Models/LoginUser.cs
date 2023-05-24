
namespace LeadsFlowCrm.Models;

/// <summary>
/// Model class for the Login method
/// </summary>
public class LoginUser
{
	public string Email { get; set; } = string.Empty;
	public string OAuthToken { get; set; } = string.Empty;
}
