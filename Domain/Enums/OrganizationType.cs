using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Enums;

public partial class OrganizationType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}
