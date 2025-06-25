using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MdrconfirmationHeader
{
    public long MdrconfirmationHeaderId { get; set; }

    public Guid MdrconfirmationHeaderAltId { get; set; }

    public long MdrheaderId { get; set; }

    public Guid MdrheaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? ConfirmationPayload { get; set; }

    public Guid? MdrhconfirmationUuid { get; set; }

    public Guid? MdrhconfirmationRefUuid { get; set; }

    public DateTime? MdrhconfirmationCreationDatetime { get; set; }

    public string? MdrhconfirmationRecipientSystemBusinessId { get; set; }

    public int? MdrhconfirmationLogBusinessDocumentProcessingResultCode { get; set; }

    public int? MdrhconfirmationLogMaximumLogItemSeverityCode { get; set; }

    public string? MdrhconfirmationLogItemTypeId { get; set; }

    public int? MdrhconfirmationLogItemSeverityCode { get; set; }

    public string? MdrhconfirmationLogItemNote { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
