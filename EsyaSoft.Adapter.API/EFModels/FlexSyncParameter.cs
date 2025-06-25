using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncParameter
{
    public long FlexSyncParameterId { get; set; }

    public Guid FlexSyncParameterAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public Guid? ParentAltId { get; set; }

    public string? ParentType { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
