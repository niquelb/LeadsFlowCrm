using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeadsFlowCrm.ViewModels
{
	/// <summary>
	/// Lógica de interacción para CreateDraftView.xaml
	/// </summary>
	public partial class CreateDraftView : Window
	{
		public CreateDraftView()
		{
			InitializeComponent();

			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.WindowState = WindowState.Normal;
			this.DragMove();
		}

		private void Maximize_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.WindowState = WindowState.Maximized;
				return;
			}

			this.WindowState = WindowState.Normal;
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}
	}
}
