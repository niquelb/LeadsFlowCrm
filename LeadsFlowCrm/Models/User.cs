namespace LeadsFlowCrm.Models;

public class User
{
	public string Id { get; set; }
	public string OAuthToken { get; set; }
	public string Email { get; set; }
	public string DisplayName { get; set; }
	public string OrganizationId { get; set; }
}
