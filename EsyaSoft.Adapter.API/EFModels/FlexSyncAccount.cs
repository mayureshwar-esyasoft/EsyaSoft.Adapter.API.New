using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncAccount
{
    public long FlexSyncAccountId { get; set; }

    public Guid FlexSyncAccountAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? AccountId { get; set; }

    public string? Name { get; set; }

    public string? AccountType { get; set; }

    public string? AccountClass { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
