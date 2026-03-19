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

            
    }
}
