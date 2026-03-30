using System;
using System.Collections.Generic;

namespace Training.Core.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int AuthorId { get; set; }

    public string? CoverUrl { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
