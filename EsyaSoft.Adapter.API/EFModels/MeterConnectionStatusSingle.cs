using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterConnectionStatusSingle
{
    public long MeterConnectionStatusSingleId { get; set; }

    public Guid MeterConnectionStatusSingleAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid Mcsuuid { get; set; }

    public Guid McsrefUuidbyMdm { get; set; }

    public DateTime? McscreationDatetimeBySap { get; set; }

    public DateTime? McscreationDatetimeByMdm { get; set; }

    public string? McssenderBusinessSystemIdbySap { get; set; }

    public string? McsrecipientSystemBusinessIdbyMdm { get; set; }

    public string? McschangeRequestId { get; set; }

    public int? McschangeRequestCategoryCode { get; set; }

    public DateTime? McsplannedProcessingDateTimeBySap { get; set; }

    public string? McsutilitiesDeviceId { get; set; }

    public string McsutilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public DateTime? McsprocessingDateTimeByMdm { get; set; }

    public int? McsutilitiesDeviceConnectionStatusProcessingResultCodeByMdm { get; set; }

    public int? McslogBusinessDocumentProcessingResultCodeByMdm { get; set; }

    public int? McslogMaximumLogItemSeverityCodeByMdm { get; set; }

    public string? McslogItemTypeIdByMdm { get; set; }

    public int? McslogItemSeverityCodeByMdm { get; set; }

    public string? McslogItemNoteByMdm { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool IsCompletedByMdm { get; set; }

    public bool IsResultSentByAdapter { get; set; }

    public bool IsSentToMdm { get; set; }
}
