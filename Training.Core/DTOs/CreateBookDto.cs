using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public int AuthorId { get; set; }
    }
}
