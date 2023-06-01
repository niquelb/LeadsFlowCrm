using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LeadsFlowCrm.Models;

public class StageButton
{
	public string Label { get; set; } = string.Empty;
	public SolidColorBrush BackgroundColor { get; set; } = new SolidColorBrush();
	public Action ClickAction { get; set; } = () => { };
}
