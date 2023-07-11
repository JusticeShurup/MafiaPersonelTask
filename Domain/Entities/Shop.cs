using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Shop
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public int? Count { get; set; }

    public int? PricePerPiece { get; set; }
}
