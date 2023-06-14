using System;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Contact model
/// </summary>
public class Contact
{
	public string Id { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastNames { get; set; } = string.Empty;
    public string FullName { get
		{
			return FirstName + " " + LastNames;
		}
	}
    public string Phone { get; set; } = string.Empty;
	public string Notes { get; set; } = string.Empty;
    public DateTime? LastEmailAt { get; set; }
}
