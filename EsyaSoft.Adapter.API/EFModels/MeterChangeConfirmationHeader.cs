using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterChangeConfirmationHeader
{
    public long MeterChangeConfirmationHeaderId { get; set; }

    public Guid MeterChangeConfirmationHeaderAltId { get; set; }

    public long MeterChangeRequestHeaderId { get; set; }

    public Guid MeterChangeRequestHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? ConfirmationPayload { get; set; }

    public Guid? MeterChangeConfirmationMhuuid { get; set; }

    public Guid? MeterChangeConfirmationMhrefUuid { get; set; }

    public DateTime? MeterChangeConfirmationMhcreationDatetime { get; set; }

    public string? MeterChangeConfirmationMhrecipientSystemBusinessId { get; set; }

    public int? MeterChangeConfirmationMainLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MeterChangeConfirmationMainLogMaximumLogItemSeverityCode { get; set; }

    public string? MeterChangeConfirmationMainLogItemTypeId { get; set; }

    public int? MeterChangeConfirmationMainLogItemSeverityCode { get; set; }

    public string? MeterChangeConfirmationMainLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
