using Microsoft.EntityFrameworkCore;
using Training.Core.interfaces;
using Training.Core.Models;

namespace Training.Core.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _dbContext;

        public BookRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Book> GetAllBooks()
        {
            return _dbContext.Books.ToList();
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            return book;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateCoverAsync(int id, string coverUrl)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book != null)
            {
                book.CoverUrl = coverUrl;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<(List<Book> Books, int TotalCount)> GetBooksAsync(BookQueryParameters parameters, CancellationToken ct)
        {
            IQueryable<Book> query = _dbContext.Books.AsQueryable();

            query = query.OrderBy(b => b.Id);

            var totalCount = await query.CountAsync(ct);

            var books = await query
                        .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                        .Take(parameters.PageSize)
                        .ToListAsync();
            return (books, totalCount);
        }
    }
}
