using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        Task<Book> AddAsync(Book book);
        Task SaveChangesAsync();
        Task<Book?> GetByIdAsync(int id);
        Task UpdateCoverAsync(int id, string coverUrl);
        Task<(List<Book> Books, int TotalCount)> GetBooksAsync(BookQueryParameters parameters, CancellationToken ct);
    }
}
