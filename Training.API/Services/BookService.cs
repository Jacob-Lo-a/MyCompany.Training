using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.API;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;
using Training.Core.Repositories;

namespace Training.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWebHostEnvironment _env;
        public BookService(IBookRepository bookRepository, IWebHostEnvironment env)
        {
            _bookRepository = bookRepository;
            _env = env;
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

        public async Task<string> UploadCoverAsync(int bookId, IFormFile image)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book == null)
            {
                throw new BookNotFoundException(bookId);
            }
            // 檔案大小不超過 5MB
            if (image.Length > 5 * 1024 * 1024)
            {
                throw new FileTooLargeException();
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(image.FileName).ToLower(); // 取得副檔名

            if (!allowedExtensions.Contains(ext))
            {
                throw new Exception("Invalid file type");
            }

            // 避免出現 Value cannot be null (path1)
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot)) 
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploadPath = Path.Combine(webRoot, "uploads", "covers");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/covers/{fileName}";

            await _bookRepository.UpdateCoverAsync(bookId, relativePath);
            return relativePath;
        }
    }
}
