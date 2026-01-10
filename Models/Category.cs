using System;
using System.Collections.Generic;

namespace SecondProj.Models;

public partial class Category
{
    public int Id { get; set; }

    public string CateName { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
