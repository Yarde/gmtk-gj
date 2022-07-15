using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const string SCHEDULE_TIME_FORMAT = "hh:mm tt";

        public static long ToTimestampInMilliseconds(this DateTime dateTime)
        {
            TimeSpan elapsed = dateTime - Epoch;
            return (long)elapsed.TotalMilliseconds;
        }
        
        public static DateTime ToDateTimeFromMilliseconds(this long timestampInMs) => Epoch.AddMilliseconds(timestampInMs);

        public static DateTime ToDateTimeFromSeconds(this long timeStampInSeconds) => Epoch.AddSeconds(timeStampInSeconds);

        public static DateTime ToDateTimeFromMilliseconds(this int timestampInMs) => Epoch.AddMilliseconds(timestampInMs);

        public static DateTime ToDateTimeFromSeconds(this int timeStampInSeconds) => Epoch.AddSeconds(timeStampInSeconds);

        public static string GetLocalScheduleTime(this DateTime dateTime) => dateTime.ToLocalTime().ToString(SCHEDULE_TIME_FORMAT);
        
        public static bool IsToday(this DateTime nextMatchTime) => nextMatchTime.Date == DateTime.Today;
        
        public static bool IsTomorrow(this DateTime nextMatchTime) => nextMatchTime.Date - TimeSpan.FromDays(1) == DateTime.Today;
        
        public static DateTime GetLocalDateTime(this long timeStamp) => GetUtcDateTime(timeStamp).ToLocalTime();
        
        public static DateTime FromUnixTime(this double unixTime) => Epoch.AddMilliseconds(unixTime);

        public static DateTime GetUtcDateTime(this long timeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(timeStamp);
            return dateTime;
        }

        public static string GetPeriodFromMilliseconds(this long lastActive)
        {
            long seconds = lastActive / 1000;
            long minutes = seconds / 60;
            long hours = minutes / 60;
            long days = hours / 24;

            if (days > 0)
            {
                return $"{days}d";
            }
            if (hours > 0)
            {
                return $"{hours}h";
            }
            if (minutes > 0)
            {
                return $"{minutes}m";
            }
            return $"{seconds}s";
        }
        
        public static async UniTask WaitForDelayInRealTime(long milliseconds, CancellationToken cancellationToken = default(CancellationToken))
        {
            float lastTimestamp = Time.realtimeSinceStartup;
            double timeLeft = milliseconds;
            while (milliseconds > 0 && !cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay((int)Math.Min(milliseconds, 1000), DelayType.Realtime, cancellationToken: cancellationToken);
                timeLeft = Math.Max(0, timeLeft - (Time.realtimeSinceStartup - lastTimestamp) * 1000f);
                lastTimestamp = Time.realtimeSinceStartup;

                milliseconds = (long)timeLeft;
            }
            await UniTask.CompletedTask;
        }
    }
}
