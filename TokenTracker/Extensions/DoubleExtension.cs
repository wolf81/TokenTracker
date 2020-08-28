using System;
namespace TokenTracker.Extensions
{
    public static class DoubleExtension
    {
        public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
            {
                return 0;
            }

            var scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

    }
}
