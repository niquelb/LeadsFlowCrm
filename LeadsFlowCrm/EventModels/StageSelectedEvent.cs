using LeadsFlowCrm.Models;

namespace LeadsFlowCrm.EventModels;

/// <summary>
/// Event model class used for switching between stages
/// </summary>
public class StageSelectedEvent
{
    /// <summary>
    /// Selected stage
    /// </summary>
    public Stage SelectedStage { get; set; } = new();
}
