using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncServicePoint
{
    public long FlexSyncServicePointId { get; set; }

    public Guid FlexSyncServicePointAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? ServicePointId { get; set; }

    public string? ClassName { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public string? LocInfo { get; set; }

    public string? PremiseMrid { get; set; }

    public string? PremiseDescription { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
