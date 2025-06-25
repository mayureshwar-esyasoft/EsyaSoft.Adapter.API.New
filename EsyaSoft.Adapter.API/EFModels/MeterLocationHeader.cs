using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterLocationHeader
{
    public long MeterLocationHeaderId { get; set; }

    public Guid MeterLocationHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MlmessageUuid { get; set; }

    public DateTime? MlcreationDatetime { get; set; }

    public string? MlsenderSystemBusinessId { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
