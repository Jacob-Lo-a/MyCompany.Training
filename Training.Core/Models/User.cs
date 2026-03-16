using System;
using System.Collections.Generic;

namespace Training.Core.Models;

public partial class User
{
    public int Id { get; set; }

    public string Account { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
