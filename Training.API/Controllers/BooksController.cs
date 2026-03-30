using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
            //  限制最大筆數
            parameters.PageSize = Math.Min(parameters.PageSize, 50);

            IQueryable<Book> query = _bookStoreDbContext.Books.AsQueryable();



            //  排序
            query = query.OrderBy(b => b.Id);

            //  總筆數
            var totalCount = await query.CountAsync(ct);

            //  分頁資料
            var books = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(ct);

            return Ok(new
            {
                Total = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                Data = books
            });
        }

        [HttpPost("{id}/cover")]
        public async Task<IActionResult> UploadCover(int id, IFormFile image)
        {
            try
            {
                var url = await _bookService.UploadCoverAsync(id, image);
                return Ok(new { imageUrl = url });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
