namespace Training.API.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public int BookId { get; set; }

        public BookNotFoundException(int bookId) : base($"書籍不存在 (ID: {bookId})")
        {
            BookId = bookId;
        }
    }
}
