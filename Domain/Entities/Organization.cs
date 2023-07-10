using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities;

public partial class Organization
{
    public int Id { get; set; }

    public int? OrganizationTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Income { get; set; }

    public int? Expenses { get; set; }

    public int? Percent { get; set; }

    public int? CollectorId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual OrganizationType? OrganizationType { get; set; }
}
