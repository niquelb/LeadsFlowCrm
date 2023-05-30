using Caliburn.Micro;
using LeadsFlowCrm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LeadsFlowCrm.ViewModels;

public class PipelinesViewModel : Screen
{
    public PipelinesViewModel()
    {
    }

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		GenerateButtons();
	}

	private void GenerateButtons()
	{
		/*
		 * TEST CODE
		 */

		// Clear existing buttons
		StageButtons.Clear();

		// Generate new buttons
		for (int i = 0; i < 5; i++) // Replace 5 with the desired number of buttons
		{
			var button = new StageButton
			{
				Label = $"Button {i + 1}",
				BackgroundColor = GetRandomBrush(),
				ClickAction = () => HandleButtonClick(i + 1)
			};

			StageButtons.Add(button);
		}
	}
	private static SolidColorBrush GetRandomBrush()
	{
		Random random = new Random();
		Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
		return new SolidColorBrush(randomColor);
	}

	private void HandleButtonClick(int buttonNumber)
	{
		// Custom click event handling for each button
		// You can implement your logic here
		MessageBox.Show($"Button {buttonNumber} clicked!");
	}


	private ObservableCollection<StageButton> _stageButtons = new();

	public ObservableCollection<StageButton> StageButtons
	{
		get { return _stageButtons; }
		set { 
			_stageButtons = value;
			NotifyOfPropertyChange();
		}
	}


}
