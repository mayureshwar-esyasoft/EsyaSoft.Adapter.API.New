using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterCreateConfirmationHeader
{
    public long MeterCreateConfirmationHeaderId { get; set; }

    public Guid MeterCreateConfirmationHeaderAltId { get; set; }

    public long MeterCreateRequestHeaderId { get; set; }

    public Guid MeterCreateRequestHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? ConfirmationPayload { get; set; }

    public Guid? MeterCreateConfirmationMhuuid { get; set; }

    public Guid? MeterCreateConfirmationMhrefUuid { get; set; }

    public DateTime? MeterCreateConfirmationMhcreationDatetime { get; set; }

    public string? MeterCreateConfirmationMhrecipientSystemBusinessId { get; set; }

    public int? MeterCreateConfirmationMainLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MeterCreateConfirmationMainLogMaximumLogItemSeverityCode { get; set; }

    public string? MeterCreateConfirmationMainLogItemTypeId { get; set; }

    public int? MeterCreateConfirmationMainLogItemSeverityCode { get; set; }

    public string? MeterCreateConfirmationMainLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
