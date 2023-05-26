using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class SelectedEmailViewModel : Screen
{
	protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		await Task.Delay(2000);

		IsLoading = false;
	}

	/*
	 * This property is used to control the visibility of the loading spinner and the actual email
	 */
	private bool _isLoading = true;
	public bool IsLoading
	{
		get { return _isLoading; }
		set { 
			_isLoading = value;
			NotifyOfPropertyChange();
		}
	}

}
