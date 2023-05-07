namespace DataAccess.Models;

public class Pipeline
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string? CreatorId { get; set; }
    
}
