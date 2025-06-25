using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncHeader
{
    public long HeaderId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? Verb { get; set; }

    public string? Noun { get; set; }

    public int? Revision { get; set; }

    public DateTime? HeaderDateTime { get; set; }

    public string? Source { get; set; }

    public string? MessageId { get; set; }

    public string? AsyncReplyTo { get; set; }

    public string? SyncMode { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsProcessed { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
