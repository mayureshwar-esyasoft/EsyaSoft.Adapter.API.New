using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterCreateConfirmationDetail
{
    public long MeterCreateConfirmationDetailId { get; set; }

    public Guid MeterCreateConfirmationDetailAltId { get; set; }

    public long MeterCreateConfirmationHeaderId { get; set; }

    public Guid MeterCreateConfirmationConfirmationHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MeterCreateConfirmationDetailUuid { get; set; }

    public Guid? MeterCreateConfirmationDetailRefUuid { get; set; }

    public DateTime? MeterCreateConfirmationDetailCreationDatetime { get; set; }

    public string? MeterCreateConfirmationDetailRecipientSystemBusinessId { get; set; }

    public string? MeterCreateConfirmationDetailUtilitiesDeviceId { get; set; }

    public int? MeterCreateConfirmationDetailLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MeterCreateConfirmationDetailLogMaximumLogItemSeverityCode { get; set; }

    public string? MeterCreateConfirmationDetailLogItemTypeId { get; set; }

    public int? MeterCreateConfirmationDetailLogItemSeverityCode { get; set; }

    public string? MeterCreateConfirmationDetailLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
