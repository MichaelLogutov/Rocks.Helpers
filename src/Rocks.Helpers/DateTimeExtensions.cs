using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rocks.Helpers
{
	public static class DateTimeExtensions
	{
		public static DateTime EndOfTheDay (this DateTime date)
		{
			return date.Date.AddDays (1).AddSeconds (-1);
		}


		public static int GetQuarter (this DateTime date)
		{
			return (date.Month - 1) / 3 + 1;
		}


		public static int DayOfTheWeek (this DateTime date, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			return ((int) date.DayOfWeek - (int) DayOfWeek.Monday + 7) % 7 + 1;
		}


		public static DateTime StartOfTheWeek (this DateTime date, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			return date.Date.AddDays (-date.DayOfTheWeek (firstDayOfWeek) + 1);
		}


		public static DateTime EndOfTheWeek (this DateTime date, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			return date.Date.AddDays (7 - date.DayOfTheWeek (firstDayOfWeek));
		}


		public static DateTime StartOfTheMonth (this DateTime date)
		{
			return new DateTime (date.Year, date.Month, 1);
		}


		public static DateTime EndOfTheMonth (this DateTime date, CultureInfo culture = null)
		{
			culture = culture ?? CultureInfo.CurrentCulture;

			var days_in_month = culture.Calendar.GetDaysInMonth (date.Year, date.Month);

			return new DateTime (date.Year, date.Month, days_in_month);
		}


		public static DateTime StartOfTheYear (this DateTime date)
		{
			return new DateTime (date.Year, 1, 1);
		}


		public static DateTime EndOfTheYear (this DateTime date)
		{
			return new DateTime (date.Year, 12, 31);
		}


		public static IEnumerable<DateTime> SplitToPeriods (this DateTime startDate, DateTime endDate, TimeSpan period)
		{
			var date = startDate;
			while (date <= endDate)
			{
				yield return date;
				date += period;
			}
		}


		public static IEnumerable<DateTime> SplitToDays (this DateTime startDate, DateTime endDate)
		{
			var date = startDate;
			while (date <= endDate)
			{
				yield return date;
				date = date.AddDays (1);
			}
		}


		public static IEnumerable<Tuple<DateTime, DateTime>> SplitToWeeks (this DateTime startDate,
		                                                                   DateTime endDate,
		                                                                   DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			var date = startDate;
			while (date <= endDate)
			{
				var date2 = date.EndOfTheWeek (firstDayOfWeek);
				if (date2 > endDate)
					date2 = endDate;

				yield return new Tuple<DateTime, DateTime> (date, date2);

				date = date2.AddDays (1);
			}
		}


		public static IEnumerable<Tuple<DateTime, DateTime>> SplitToMonths (this DateTime startDate, DateTime endDate, CultureInfo culture = null)
		{
			var date = startDate;
			while (date <= endDate)
			{
				var date2 = date.EndOfTheMonth (culture);
				if (date2 > endDate)
					date2 = endDate;

				yield return new Tuple<DateTime, DateTime> (date, date2);

				date = date2.AddDays (1);
			}
		}
	}
}