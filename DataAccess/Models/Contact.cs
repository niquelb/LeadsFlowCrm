namespace DataAccess.Models;

public class Contact
{
	public String Id { get; set; } = Guid.NewGuid().ToString();
	public String Email { get; set; }
    public String FirstName { get; set; }
    public String? LastNames { get; set; }
    public String? Phone { get; set; }
    public String? Address { get; set; }
    public String? Company { get; set; }
    public String? JobTitle { get; set; }
    public String? Website { get; set; }
    public String? Location { get; set; }
    public String? Notes { get; set; }

}
