using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Inventory
{
    public int Id { get; set; }

    public int? BrassKnuckles { get; set; }

    public int? BaseballBat { get; set; }

    public int? M4 { get; set; }

    public int? Ak47 { get; set; }

    public int? Glock { get; set; }

    public int _545x39mm { get; set; }

    public int _762x39mm { get; set; }

    public int _9x19mm { get; set; }

    public virtual FamilyMember IdNavigation { get; set; } = null!;
}
