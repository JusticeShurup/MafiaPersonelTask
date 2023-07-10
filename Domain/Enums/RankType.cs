using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Enums;

public partial class RankType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
}
