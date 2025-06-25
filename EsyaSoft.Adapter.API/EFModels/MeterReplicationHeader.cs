using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterReplicationHeader
{
    public long MeterReplicationHeaderId { get; set; }

    public Guid MeterReplicationHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? ReplicationMessageUuid { get; set; }

    public DateTime? ReplicationCreationDatetime { get; set; }

    public string? ReplicationSenderSystemBusinessId { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsProcessCompleted { get; set; }
}
