namespace LeadsFlowCrm.EventModels;

/// <summary>
/// Event model class for handling navigation
/// </summary>
public class NavigationEvent
{
	/// <summary>
	/// Main navigation routes for the application
	/// </summary>
	public enum NavigationRoutes
	{
		Inbox,
		Pipelines
	}

	/// <summary>
	/// Selected route
	/// </summary>
	public NavigationRoutes Route { get; }

	public NavigationEvent(NavigationRoutes route)
	{
		Route = route;
	}
}
