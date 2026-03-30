using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
