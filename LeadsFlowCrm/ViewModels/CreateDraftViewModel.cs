﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class CreateDraftViewModel : Screen
{


	public async void Exit()
	{
		await TryCloseAsync();
	}
}
