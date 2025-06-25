using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MdrresultHeader
{
    public long MdrresultHeaderId { get; set; }

    public Guid MdrresultHeaderAltId { get; set; }

    public long MdrheaderId { get; set; }

    public Guid MdrheaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MdrresultHeaderUuid { get; set; }

    public Guid? MdrresultHeaderRefUuid { get; set; }

    public DateTime? MdrresultHeaderCreationDatetime { get; set; }

    public string? MdrresultHeaderRecipientSystemBusinessId { get; set; }

    public bool? IsResultSent { get; set; }

    public bool? IsResultReceived { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
