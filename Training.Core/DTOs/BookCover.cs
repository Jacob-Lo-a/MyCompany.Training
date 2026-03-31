using Microsoft.AspNetCore.Http;

namespace Training.Core.DTOs
{
    public class BookCover
    {
        public int BookId { get; set; }
        public IFormFile? Image {  get; set; }
    }
}
