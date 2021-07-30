namespace BlueWater.OrderManagement.Common.Contracts
{
    public class ScheduleDateTime
    {
        /// <summary>
        /// Schedule hours
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Schedule minutes
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Schedule seconds
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Schedule dateTime in format "yyyyMMddTHH:mm:ssZ"
        /// </summary>
        public string ScheduleTime {get; set;}
    }
}
