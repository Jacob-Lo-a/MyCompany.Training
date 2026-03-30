using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core.DTOs;
using Training.Core.Models;

namespace Training.Core.interfaces
{
    public interface IBookService
    {
        List<Book> GetDiscountBooks();
        Task<Book> CreatedBookAsync(CreateBookDto dto);

        Task<string> UploadCoverAsync(int bookId, IFormFile image);
    }
}
