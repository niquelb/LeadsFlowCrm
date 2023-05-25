using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LeadsFlowCrm.ViewModels;

public class LoadingViewModel : Screen
{
	public LoadingViewModel(string text)
    {
		Text = text;
	}

	public string Text { get; }
}
