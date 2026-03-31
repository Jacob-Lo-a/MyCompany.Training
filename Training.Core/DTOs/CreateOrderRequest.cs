namespace Training.Core.DTOs
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItem> Items { get; set; } = new();
    }
}
