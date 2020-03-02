using System;
using System.Globalization;
using Pathogen.Models;
using Xamarin.Forms;

namespace Pathogen.Converters
{
	public class DateTimeToTimeSince : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            string outputDateTime = string.Empty;
            var inputDateTime = ((DateTime)value).ToLocalTime();

			if (inputDateTime != null)
			{
                TimeSpan ts = DateTime.Now - inputDateTime;

                if (ts.Days > 7)
                {
                    outputDateTime = inputDateTime.ToString("MMMM d, yyyy");
                }
                else if (ts.Days > 0)
                {
                    outputDateTime = ts.Days == 1 ? ("a day ago") : ("about " + ts.Days.ToString() + " days ago");
                }
                else if (ts.Hours > 0)
                {
                    outputDateTime = ts.Hours == 1 ? ("an hour ago") : (ts.Hours.ToString() + " hours ago");
                }
                else if (ts.Minutes > 0)
                {
                    outputDateTime = ts.Minutes == 1 ? ("1 minute ago") : (ts.Minutes.ToString() + " minutes ago");
                }
                else outputDateTime = "few seconds ago";
            }

            return outputDateTime;
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}