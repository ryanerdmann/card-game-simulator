using System.Globalization;

namespace CardGameSimulator
{
    public static class Extensions
    {
        public static string ToKMB(this int value)
        {
            var format = value switch
            {
                > 999999999 or < -999999999 => "0,,,.###B",
                > 999999 or < -999999       => "0,,.##M",
                > 999 or < -999             => "0,.#K",
                _                           => "0",
            };

            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
