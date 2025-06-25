using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterRegisterCreateConfirmationHeader
{
    public long MeterRegisterCreateConfirmationHeaderId { get; set; }

    public Guid MeterRegisterCreateConfirmationHeaderAltId { get; set; }

    public long MeterRegisterCreateHeaderId { get; set; }

    public Guid MeterRegisterCreateHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? ConfirmationPayload { get; set; }

    public Guid? MeterRegisterCreateConfirmationMhuuid { get; set; }

    public Guid? MeterRegisterCreateConfirmationMhrefUuid { get; set; }

    public DateTime? MeterRegisterCreateConfirmationMhcreationDatetime { get; set; }

    public string? MeterRegisterCreateConfirmationMhrecipientSystemBusinessId { get; set; }

    public int? MeterRegisterCreateConfirmationMainLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MeterRegisterCreateConfirmationMainLogMaximumLogItemSeverityCode { get; set; }

    public string? MeterRegisterCreateConfirmationMainLogItemTypeId { get; set; }

    public int? MeterRegisterCreateConfirmationMainLogItemSeverityCode { get; set; }

    public string? MeterRegisterCreateConfirmationMainLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
