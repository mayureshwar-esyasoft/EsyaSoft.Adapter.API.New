using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterRegisterHeader
{
    public long MeterRegisterHeaderId { get; set; }

    public Guid MeterRegisterHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? RegisterType { get; set; }

    public Guid? MeterRegisterHeaderMessageUuid { get; set; }

    public DateTime? MeterRegisterHeaderCreationDatetime { get; set; }

    public string? MeterRegisterHeaderMessageSenderSystemBusinessId { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
