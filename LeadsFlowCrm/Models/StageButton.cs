using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LeadsFlowCrm.Models;

public class StageButton
{
	public string Label { get; set; }
	public SolidColorBrush BackgroundColor { get; set; }
	public Action ClickAction { get; set; }
}
