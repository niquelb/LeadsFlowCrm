using Caliburn.Micro;
using LeadsFlowCrm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeadsFlowCrm;

internal class Bootstrapper : BootstrapperBase
{
    public Bootstrapper()
    {
        Initialize();
    }

	protected override void OnStartup(object sender, StartupEventArgs e)
	{
		DisplayRootViewForAsync<ShellViewModel>();
	}
}
