using System;
using System.Collections.Generic;

namespace SecondProj.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public int Price { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public string? Image { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
