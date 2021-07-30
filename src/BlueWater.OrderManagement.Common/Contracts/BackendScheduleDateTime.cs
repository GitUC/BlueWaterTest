namespace BlueWater.OrderManagement.Common.Contracts
{
    public class BackendScheduleDateTime
    {
        public ScheduleType JobType { get; set; }

        public ScheduleDateTime ScheduleDateTime { get; set; }
    }
}
