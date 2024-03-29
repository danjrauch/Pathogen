﻿using System;
using System.Globalization;
using Pathogen.Models;
using Xamarin.Forms;

namespace Pathogen.Converters
{
	public class SelectedItemEventArgsToSelectedItemConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var eventArgs = value as ItemTappedEventArgs;

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

            //if (parameter != null)
            //{
            //    int ageParam = int.Parse(parameter.ToString());
            //    person = new Person(person.Name, person.Age, ageParam);
            //}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}