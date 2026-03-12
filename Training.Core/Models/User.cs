using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class ClassA
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Count {  get; set; }
    }
    public class ClassB
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price {  get; set; }
    }

    
}
