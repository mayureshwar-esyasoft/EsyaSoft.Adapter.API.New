using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class Mdrheader
{
    public long MdrheaderId { get; set; }

    public Guid MdrheaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? Mhuuid { get; set; }

    public DateTime? MhcreationDatetime { get; set; }

    public string? SenderSystemBusinessId { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
