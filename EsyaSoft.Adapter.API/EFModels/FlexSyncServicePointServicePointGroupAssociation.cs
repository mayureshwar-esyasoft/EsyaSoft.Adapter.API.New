using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncServicePointServicePointGroupAssociation
{
    public long FlexSyncServicePointServicePointGroupAssociationId { get; set; }

    public Guid FlexSyncServicePointServicePointGroupAssociationAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public DateTime? StartDate { get; set; }

    public string? ServicePointId { get; set; }

    public string? ServicePointType { get; set; }

    public Guid? FlexSyncServicePointAltId { get; set; }

    public string? ServicePointGroupId { get; set; }

    public string? ServicePointGroupType { get; set; }

    public Guid? FlexSyncServicePointGroupAltId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
