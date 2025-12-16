using System;
using System.Collections.Generic;

namespace SecondProj.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Tprice { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Register User { get; set; } = null!;
}
