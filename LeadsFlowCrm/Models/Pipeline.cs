using System.Collections.Generic;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Pipeline model
/// </summary>
public class Pipeline
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
	/// Organization which owns this pipeline
	/// </summary>
    public Organization Organization { get; set; } = new Organization();

	/// <summary>
	/// Stages for the pipeline
	/// </summary>
	public IList<Stage> Stages { get; set; } = new List<Stage>();
}
