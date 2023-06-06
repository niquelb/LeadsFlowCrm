using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
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
    public PipelinesViewModel(IContactService contactService)
	{
		_contactService = contactService;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		Contacts = new ObservableCollection<Contact>(await _contactService.GetAllAsync());
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
		Random random = new();
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
	private ObservableCollection<Contact> _contacts = new();
	private Contact _selectedContact;
	private readonly IContactService _contactService;

	public ObservableCollection<StageButton> StageButtons
	{
		get { return _stageButtons; }
		set { 
			_stageButtons = value;
			NotifyOfPropertyChange();
		}
	}

	public ObservableCollection<Contact> Contacts
	{
		get { return _contacts; }
		set { 
			_contacts = value;
			NotifyOfPropertyChange();
		}
	}

	public Contact SelectedContact
	{
		get { return _selectedContact; }
		set { 
			_selectedContact = value;
			NotifyOfPropertyChange();
		}
	}

}
