using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;
using Training.Core.Repositories;

namespace Training.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public List<Book> GetDiscountBooks()
        {
            var books = _bookRepository.GetAllBooks();

            foreach (var book in books)
            {
                book.Price = book.Price * 0.8m;
            }

            return books;
        }
        public async Task<Book> CreatedBookAsync(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Price = dto.Price,
                Stock = dto.Stock,
                AuthorId = dto.AuthorId
            };
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
        }

    }
}
