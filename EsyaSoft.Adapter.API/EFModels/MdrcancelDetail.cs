using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MdrcancelDetail
{
    public long MdrcancelDetailId { get; set; }

    public Guid MdrcancelDetailAltId { get; set; }

    public Guid MdrcancelHeaderAltId { get; set; }

    public long MdrcancelHeaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid CancelDetailUuid { get; set; }

    public DateTime CancelDetailMhcreationTime { get; set; }

    public string CancelDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string MeterReadingDocumentId { get; set; } = null!;

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool? IsCancellationConfirmationSentToSap { get; set; }

    public bool? IsMdminvokedCancellation { get; set; }

    public bool? IsCancellationProcessedByMdm { get; set; }
}
