using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncAccountServicePointAssociation
{
    public long FlexSyncAccountServicePointAssociationId { get; set; }

    public Guid FlexSyncAccountServicePointAssociationAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? AccountId { get; set; }

    public string? AccountType { get; set; }

    public Guid? FlexSyncAccountAltId { get; set; }

    public string? ServicePointId { get; set; }

    public string? ServicePointType { get; set; }

    public Guid? FlexSyncServicePointAltId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
