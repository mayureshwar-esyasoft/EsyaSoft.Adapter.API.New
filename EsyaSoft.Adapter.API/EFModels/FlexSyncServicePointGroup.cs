using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncServicePointGroup
{
    public long FlexSyncServicePointGroupId { get; set; }

    public Guid FlexSyncServicePointGroupAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? ServicePointGroupId { get; set; }

    public string? Name { get; set; }

    public string? RouteType { get; set; }

    public string? RouteSubType { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
