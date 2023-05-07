namespace DataAccess.Models;

public class Organization
{
	public String Id { get; set; } = Guid.NewGuid().ToString();
	public String Name { get; set; }
    public String? CreatorId { get; set; }
}
