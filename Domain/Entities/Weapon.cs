using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Weapon
{
    public int Id { get; set; }

    public int FamilyMemberId { get; set; }

    public string Name { get; set; } = null!;

    public int? AmmunitionTypeId { get; set; }

    public int? AmmunitionCount { get; set; }

    public virtual AmmunitionType? AmmunitionType { get; set; }

    public virtual FamilyMember FamilyMember { get; set; } = null!;
}
