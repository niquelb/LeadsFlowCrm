using System;
using System.Windows.Media;

namespace LeadsFlowCrm.Models;

public class StageButton
{
	public string Label { get; set; } = string.Empty;
	public SolidColorBrush BackgroundColor { get; set; } = new SolidColorBrush();
	public Action ClickAction { get; set; } = () => { };
}
