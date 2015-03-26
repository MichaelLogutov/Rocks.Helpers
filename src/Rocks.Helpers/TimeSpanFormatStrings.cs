namespace Rocks.Helpers
{
    public class TimeSpanFormatStrings
    {
        #region Static fields

        private static TimeSpanFormatStrings _default =
            new TimeSpanFormatStrings
            {
                MSec = " ms",
                Sec = " sec",
                Min = " min",
                Hour = " h",
                Day = " d",
            };

        #endregion

        #region Public properties

        public static TimeSpanFormatStrings Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public string MSec { get; set; }
        public string Sec { get; set; }
        public string Min { get; set; }
        public string Hour { get; set; }
        public string Day { get; set; }

        #endregion
    }
}