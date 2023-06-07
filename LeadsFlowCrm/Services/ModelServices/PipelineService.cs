using LeadsFlowCrm.Models;
using System.Collections.Generic;

namespace LeadsFlowCrm.Services.ModelServices;

/// <summary>
/// Service class for the Pipeline model
/// </summary>
/// <see cref="Pipeline"/>
public class PipelineService : IPipelineService
{
	public PipelineService()
	{

	}

	/// <summary>
	/// Method for getting a mock pipeline
	/// </summary>
	/// <returns></returns>
	public Pipeline GetDemoPipeline()
	{
		return new Pipeline()
		{
			Id = "2AAE13CD-E092-4C2E-83C6-90525C6638D7",
			Name = "Sales",
			Stages = new List<Stage>()
			{
				new Stage()
				{
					 Id = "{3FBD527C-EABD-45A0-AA08-E9E469AEF16A}",
					 Name = "Lead",
					 Color = "#2C068E"
				},
				new Stage()
				{
					 Id = "{3FBD527C-EABD-45A0-AA08-E9E469AEF16B}",
					 Name = "Contacted",
					 Color = "#005DFF"
				},
				new Stage()
				{
					 Id = "{3FBD527C-EABD-45A0-AA08-E9E469AEF16C}",
					 Name = "Pitched",
					 Color = "#2300FF"
				},
				new Stage()
				{
					 Id = "{3FBD527C-EABD-45A0-AA08-E9E469AEF16D}",
					 Name = "Closed - Lost",
					 Color = "#FF8A00"
				},
				new Stage()
				{
					 Id = "{3FBD527C-EABD-45A0-AA08-E9E469AEF16E}",
					 Name = "Closed - Won",
					 Color = "#FF003D"
				},
			}
		};
	}
}
