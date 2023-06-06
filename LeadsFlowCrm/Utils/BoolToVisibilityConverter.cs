using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LeadsFlowCrm.Utils;

/// <summary>
/// Class for converting a boolean into a valid Visibility value
/// </summary>
public class BoolToVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool hasText = !(bool)values[0];
		bool hasFocus = (bool)values[1];

		if (hasText || hasFocus)
		{
			return Visibility.Collapsed;
		}

		return Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
