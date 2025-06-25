using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterChangeConfirmationDetail
{
    public long MeterChangeConfirmationDetailId { get; set; }

    public Guid MeterChangeConfirmationDetailAltId { get; set; }

    public long MeterChangeConfirmationHeaderId { get; set; }

    public Guid MeterChangeConfirmationHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MeterChangeConfirmationDetailUuid { get; set; }

    public Guid? MeterChangeConfirmationDetailRefUuid { get; set; }

    public DateTime? MeterChangeConfirmationDetailCreationDatetime { get; set; }

    public string? MeterChangeConfirmationDetailRecipientSystemBusinessId { get; set; }

    public string? MeterChangeConfirmationDetailUtilitiesDeviceId { get; set; }

    public int? MeterChangeConfirmationDetailLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MeterChangeConfirmationDetailLogMaximumLogItemSeverityCode { get; set; }

    public string? MeterChangeConfirmationDetailLogItemTypeId { get; set; }

    public int? MeterChangeConfirmationDetailLogItemSeverityCode { get; set; }

    public string? MeterChangeConfirmationDetailLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
