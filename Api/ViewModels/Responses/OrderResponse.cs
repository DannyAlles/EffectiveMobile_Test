namespace Api.ViewModels.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public double Weight { get; set; }
        public Guid DistrictId { get; set; }
        public DateTimeOffset DeliveryTime { get; set; }
    }
}
