namespace DataAccess.Models;

public class Contact
{
	public string? Id { get; set; } = Guid.NewGuid().ToString();
	public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastNames { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public string? Website { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public string? StageId { get; set; }
    public string? UserId { get; set; }
}
