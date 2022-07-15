using System;
using System.Globalization;
using UnityEngine;

namespace Yarde.Utils.Extensions
{
    public static class FloatExtensions
    {
        public static int ToMilliseconds(this float seconds) => Mathf.RoundToInt(seconds * 1000);
        public static string AbbreviateNumber(this int num)
        {
            return AbbreviateNumber((long)num);
        }

        public static string AbbreviateNumber(this long num)
        {
            var us = CultureInfo.GetCultureInfo("en-US");

            if (num < 10000)
            {
                return string.Format(us, "{0:n0}", num);
            }

            var i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;
            if (num >= 10000000000)
            {
                return (num / 10000000000D).ToString("0") + "B";
            }
            if (num >= 1000000000)
            {
                return (num / 1000000000D).ToString("0.#") + "B";
            }
            if (num >= 10000000)
            {
                return (num / 1000000D).ToString("0") + "M";
            }
            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.#") + "M";
            }
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.#") + "K";
            }
            return num.ToString("#,0");
        }
    }
}
