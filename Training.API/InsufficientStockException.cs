using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Training.API
{
    public class InsufficientStockException : Exception
    {
        public string BookName { get; }
         
        public InsufficientStockException(string bookName)
            : base($"書籍「{bookName}」庫存不足")
        {
            BookName = bookName;
        }
    }
}
