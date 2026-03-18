using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core.Models
{
    public class BuyBookRequest
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
