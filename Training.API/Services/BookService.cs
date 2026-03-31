using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;
using Training.API.Exceptions;

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

        public async Task<string> UploadCoverAsync(BookCover bookCover)
        {
            var book = await _bookRepository.GetByIdAsync(bookCover.BookId);

            if (book == null)
            {
                throw new BookNotFoundException(bookCover.BookId);
            }
            // 檔案大小不超過 5MB
            if (bookCover.Image.Length > 5 * 1024 * 1024)
            {
                throw new FileTooLargeException();
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(bookCover.Image.FileName).ToLower(); // 取得副檔名

            if (!allowedExtensions.Contains(ext))
            {
                throw new InvalidFileTypeException();
            }

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "covers");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await bookCover.Image.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/covers/{fileName}";

            await _bookRepository.UpdateCoverAsync(bookCover.BookId, relativePath);
            return relativePath;
        }
        public async Task<object> GetBooksAsync(BookQueryParameters parameters, CancellationToken ct)
        {

            parameters.PageSize = Math.Min(parameters.PageSize, 50);

            var (books, totalCount) = await _bookRepository.GetBooksAsync(parameters, ct);

            return new
            {
                Total = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                Data = books
            };
        }
    }
}
