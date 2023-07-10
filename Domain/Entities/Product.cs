using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Product
{
    public string ProductName { get; set; } = null!;

    public int? Count { get; set; }

    public int? PricePerPiece { get; set; }
}
