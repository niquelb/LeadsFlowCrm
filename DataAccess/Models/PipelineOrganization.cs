
namespace DataAccess.Models;

public class PipelineOrganization
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string PipelineId { get; set; }
	public string OrganizationId { get; set; }
}
