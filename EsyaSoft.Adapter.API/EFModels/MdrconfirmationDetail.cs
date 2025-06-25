using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MdrconfirmationDetail
{
    public long MdrconfirmationDetailId { get; set; }

    public Guid MdrconfirmationDetailAltId { get; set; }

    public long MdrconfirmationHeaderId { get; set; }

    public Guid MdrconfirmationHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MdrconfirmationDetailUuid { get; set; }

    public Guid? MdrconfirmationDetailRefUuid { get; set; }

    public DateTime? MdrconfirmationDetailCreationDatetime { get; set; }

    public string? MdrconfirmationDetailRecipientSystemBusinessId { get; set; }

    public string? MdrconfirmationDetailMeterReadingDocumentId { get; set; }

    public int? MdrconfirmationDetailLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MdrconfirmationDetailLogMaximumLogItemSeverityCode { get; set; }

    public string? MdrconfirmationDetailLogItemTypeId { get; set; }

    public int? MdrconfirmationDetailLogItemSeverityCode { get; set; }

    public string? MdrconfirmationDetailLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
