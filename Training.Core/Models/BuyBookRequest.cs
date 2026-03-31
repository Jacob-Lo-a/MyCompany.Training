namespace Training.Core.Models
{
    public class BuyBookRequest
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
