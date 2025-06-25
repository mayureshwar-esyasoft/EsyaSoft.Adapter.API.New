using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterRegisterCreateConfirmationDetail
{
    public long MeterRegisterCreateConfirmationDetailId { get; set; }

    public Guid MeterRegisterCreateConfirmationDetailAltId { get; set; }

    public long MeterRegisterCreateConfirmationHeaderId { get; set; }

    public Guid MeterRegisterCreateConfirmationHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MeterRegisterCreateConfirmationDetailUuid { get; set; }

    public Guid? MeterRegisterCreateConfirmationDetailRefUuid { get; set; }

    public DateTime? MeterRegisterCreateConfirmationDetailCreationDatetime { get; set; }

    public string? MeterRegisterCreateConfirmationDetailRecipientSystemBusinessId { get; set; }

    public string? MeterRegisterCreateConfirmationDetailUtilitiesDeviceId { get; set; }

    public int? MeterRegisterCreateConfirmationDetailLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MeterRegisterCreateConfirmationDetailLogMaximumLogItemSeverityCode { get; set; }

    public string? MeterRegisterCreateConfirmationDetailLogItemTypeId { get; set; }

    public int? MeterRegisterCreateConfirmationDetailLogItemSeverityCode { get; set; }

    public string? MeterRegisterCreateConfirmationDetailLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
