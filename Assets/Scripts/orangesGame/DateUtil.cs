using System;

namespace orangesGame
{
	internal class DateUtil : Singleton<DateUtil>
	{
		public bool IsSameDay(DateTime d1, DateTime d2)
		{
			if (d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day)
			{
				return true;
			}
			return false;
		}

		public DateTime parseDateTimeByHour(int hour)
		{
			DateTime today = DateTime.Today;
			if (hour < 0)
			{
				return today;
			}
			if (hour >= 24)
			{
				return today.AddDays(1.0);
			}
			today.AddHours(hour);
			return today;
		}

		public DateTime parseDateTimeByDay(DateTime d, int day)
		{
			return d.AddDays(day);
		}
	}
}
