using System.Collections.Generic;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Organization model
/// </summary>
public class Organization
{
	/// <summary>
	/// ID
	/// </summary>
	public string Id { get; set; } = string.Empty;
	
	/// <summary>
	/// Name
	/// </summary>
	public string Name { get; set; } = string.Empty;
    
	/// <summary>
	/// Users assigned to this org
	/// </summary>
	public IList<User> Users { get; set; } = new List<User>();

	/// <summary>
	/// Pipelines that belong to this org
	/// </summary>
    public IList<Pipeline> Pipelines { get; set; } = new List<Pipeline>();
}
