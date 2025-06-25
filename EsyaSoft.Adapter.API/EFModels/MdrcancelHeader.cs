using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MdrcancelHeader
{
    public long MdrcancelHeaderId { get; set; }

    public Guid MdrcancelHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? Mcuuid { get; set; }

    public DateTime? MccreationDatetime { get; set; }

    public string? McsenderSystemBusinessId { get; set; }

    public bool? IsCancellationConfirmationSentToSap { get; set; }

    public bool? IsMdminvokedCancellation { get; set; }

    public bool? IsCancellationProcessedByMdm { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
