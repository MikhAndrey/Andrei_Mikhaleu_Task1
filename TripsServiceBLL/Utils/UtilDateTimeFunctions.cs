namespace TripsServiceBLL.Utils
{
	public static class UtilDateTimeFunctions
	{
		public static string GetTimeSpanString(TimeSpan time)
		{
			int days = time.Days;
			int hours = time.Hours % 24;
			int minutes = time.Minutes % 60;
			return $"{days} days, {hours} hours, {minutes} minutes";
		}

		public static string GetTimeAgoFromNow(DateTime date)
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan dateDiff = utcNow - date;
			int years = (int)(dateDiff.TotalDays / UtilConstants.DaysInYear);
			if (years > 0)
			{
				return $"{years} years ago";
			}

			int months = (int)(dateDiff.TotalDays / UtilConstants.DaysInMonth);
			if (months > 0)
			{
				return $"{months} months ago";
			}

			int days = dateDiff.Days;
			if (days > 0)
			{
				return $"{days} days ago";
			}

			int hours = dateDiff.Hours;
			if (hours > 0)
			{
				return $"{hours} hours ago";
			}

			int minutes = dateDiff.Minutes;
			return $"{minutes} minutes ago";
		}
	}
}
