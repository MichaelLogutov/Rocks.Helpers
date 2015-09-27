using System;
using System.Globalization;

namespace Rocks.Helpers.Tests
{
    public static class TestHelpers
    {
        public static DateTime AsTestDateTime (this string str, string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact (str, format, CultureInfo.InvariantCulture);
        }
    }
}