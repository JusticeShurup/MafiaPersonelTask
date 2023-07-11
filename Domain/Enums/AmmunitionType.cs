using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain;


public partial class AmmunitionType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
}
