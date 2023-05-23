
namespace DataAccess.Models;

public class Field
{
	public string? Id { get; set; } = Guid.NewGuid().ToString();
	public string? Name { get; set; }
    public string? Type { get; set; }
    public string? PipelineId { get; set; }
}
