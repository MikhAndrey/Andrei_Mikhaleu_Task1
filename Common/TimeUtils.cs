﻿namespace Andrei_Mikhaleu_Task1.Common
{
    public class TimeUtils
    {
        public static string GetTimeSpanString(TimeSpan time)
        {
            int days = time.Days;
            int hours = time.Hours % 24;
            int minutes = time.Minutes % 60;
            return $"{days} days, {hours} hours, {minutes} minutes";
        }
    }
}
