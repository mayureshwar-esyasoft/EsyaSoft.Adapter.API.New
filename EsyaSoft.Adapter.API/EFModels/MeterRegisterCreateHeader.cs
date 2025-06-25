using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterRegisterCreateHeader
{
    public long MeterRegisterCreateHeaderId { get; set; }

    public Guid MeterRegisterCreateHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MeterRegisterCreateMessageUuid { get; set; }

    public DateTime? MeterRegisterCreateCreationDatetime { get; set; }

    public string? MeterRegisterCreateMessageSenderSystemBusinessId { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
