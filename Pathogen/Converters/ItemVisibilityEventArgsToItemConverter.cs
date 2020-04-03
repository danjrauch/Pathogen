using System;
using System.Globalization;
using Pathogen.Models;
using Xamarin.Forms;

namespace Pathogen.Converters
{
	public class ItemVisibilityEventArgsToItemConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var eventArgs = value as ItemVisibilityEventArgs;

			switch (eventArgs.Item)
			{
				case NewsItem item:
					return item;
				case Message item:
					return item;
				case string item:
					return item;
				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}