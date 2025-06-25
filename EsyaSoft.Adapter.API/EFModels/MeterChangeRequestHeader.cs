using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterChangeRequestHeader
{
    public long MeterChangeRequestHeaderId { get; set; }

    public Guid MeterChangeRequestHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MchangeRmessageUuid { get; set; }

    public DateTime? MchangeRcreationDatetime { get; set; }

    public string? MchangeRsenderSystemBusinessId { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
