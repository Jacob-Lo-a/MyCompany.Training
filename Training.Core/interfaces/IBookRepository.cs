using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
