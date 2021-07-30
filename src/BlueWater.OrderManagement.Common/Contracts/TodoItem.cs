namespace BlueWater.OrderManagement.Common.Contracts
{
    public record TodoItem
    {
        public int Id { get; set; }
        public string Task { get; set; }
    };
}
