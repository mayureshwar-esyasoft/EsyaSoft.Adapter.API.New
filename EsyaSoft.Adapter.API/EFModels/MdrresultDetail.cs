using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MdrresultDetail
{
    public long MdrresultDetailId { get; set; }

    public Guid MdrresultDetailAltId { get; set; }

    public long MdrresultHeaderId { get; set; }

    public Guid MdrresultHeaderAltId { get; set; }

    public long ServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public Guid? MdrresultDetailUuid { get; set; }

    public Guid? MdrresultDetailRefUuid { get; set; }

    public DateTime? MdrresultDetailCreationDatetime { get; set; }

    public string? MdrresultDetailRecipientSystemBusinessId { get; set; }

    public long MdrresultDetailMeterReadingDocumentId { get; set; }

    public int MdrresultDetailMeterReadingReasonCode { get; set; }

    public DateTime? MdrresultDetailScheduledMeterReadingDate { get; set; }

    public string? MdrresultDetailUtilitiesMeasurementTaskId { get; set; }

    public string? MdrresultDetailUtilitiesObjectIdentificationSystemCodeText { get; set; }

    public string? MdrresultDetailUtilitiesUtilitiesDeviceId { get; set; }

    public string? MdrresultDetailActualMeterReadingDate { get; set; }

    public string? MdrresultDetailActualMeterReadingTime { get; set; }

    public int? MdrresultDetailMeterReadingTypeCode { get; set; }

    public string? MdrresultDetailMeterReadingResultValue { get; set; }

    public bool? IsResultSent { get; set; }

    public bool? IsResultReceived { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public bool? IsPickedByAdapter { get; set; }
}
