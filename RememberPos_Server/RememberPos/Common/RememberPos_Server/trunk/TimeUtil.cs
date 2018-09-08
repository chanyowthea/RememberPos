using System;

namespace RememberPos
{
	public class TimeUtil
	{
		public const long _toTicks = 10000000; 

		// 时间戳转为C#格式时间
		public DateTime StampToDateTime(long timeStamp)
		{
			DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long lTime = long.Parse(timeStamp + "0000000");
			TimeSpan toNow = new TimeSpan(lTime);
			return dateTimeStart.Add(toNow);
		}

		// DateTime时间格式转换为Unix时间戳格式
		public long DateTimeToStamp(System.DateTime time)
		{
			System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
			return (long)(time - startTime).TotalSeconds;
		}

		public float GetTimeGap(DateTime recent, DateTime before)
		{
			return (recent.Ticks - before.Ticks) / (float)_toTicks;
		}
	}
}
