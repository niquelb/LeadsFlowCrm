using System.Collections.Generic;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Stage model
/// </summary>
public class Stage
{
	public string Id { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Color { get; set; } = string.Empty;
	public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
