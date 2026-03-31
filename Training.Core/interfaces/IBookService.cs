using Training.Core.DTOs;
using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IBookService
    {
        List<Book> GetDiscountBooks();
        Task<Book> CreatedBookAsync(CreateBookDto dto);

        Task<string> UploadCoverAsync(BookCover bookCover);
        Task<Object> GetBooksAsync(BookQueryParameters parameters, CancellationToken ct);
    }
}
