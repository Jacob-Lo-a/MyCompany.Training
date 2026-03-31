namespace Training.Core.DTOs
{
    public class CreateOrderItem
    {
        public int UserId { get; set; }
        public List<CreateBookDetail> CreateBookDetails { get; set; }
    }

    public class CreateBookDetail
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
