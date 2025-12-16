using System;
using System.Collections.Generic;

namespace SecondProj.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public int Tprice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Register User { get; set; } = null!;
}
