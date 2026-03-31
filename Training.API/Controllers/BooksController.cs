using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.Core.DTOs;
using Training.Core.interfaces;
using Training.Core.Models;

namespace Training.Core.Controllers
{
    [ApiController]
    [Route("api/book/[controller]")]

    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;
        private readonly BookStoreDbContext _bookStoreDbContext;


        public BooksController(IBookService bookService, ILogger<BooksController> logger, BookStoreDbContext bookStoreDbContext, IWebHostEnvironment env)
        {
            _bookService = bookService;
            _logger = logger;
            _bookStoreDbContext = bookStoreDbContext;

        }


        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookService.GetDiscountBooks();
            return Ok(books);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
        {
            try
            {
                var result = await _bookService.CreatedBookAsync(dto);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 查詢書籍（支援關鍵字 + 分頁）
        /// </summary>
        /// <param name="parameters">查詢參數</param>
        /// <param name="ct">取消權杖</param>
        /// <returns>書籍清單</returns>
        [HttpGet("GetBooks")]
        public async Task<IActionResult> GetBooks(
            [FromQuery] BookQueryParameters parameters,
            CancellationToken ct)
        {
            var result = await _bookService.GetBooksAsync(parameters, ct);

            return Ok(result);
        }

        [HttpPost("{BookId}/cover")]
        public async Task<IActionResult> UploadCover(BookCover bookCover)
        {
            try
            {
                var url = await _bookService.UploadCoverAsync(bookCover);
                return Ok(new { imageUrl = url });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
