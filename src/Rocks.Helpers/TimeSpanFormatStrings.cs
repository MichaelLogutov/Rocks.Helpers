namespace Rocks.Helpers
{
    public class TimeSpanFormatStrings
    {
        #region Static fields

        private static TimeSpanFormatStrings _default =
            new TimeSpanFormatStrings
            {
                Milliseconds = " ms",
                Seconds = " sec",
                Minutes = " min",
                Hours = " h",
                Days = " d",
            };

        #endregion

        #region Public properties

        public static TimeSpanFormatStrings Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public string Milliseconds { get; set; }
        public string Seconds { get; set; }
        public string Minutes { get; set; }
        public string Hours { get; set; }
        public string Days { get; set; }

        #endregion
    }
}