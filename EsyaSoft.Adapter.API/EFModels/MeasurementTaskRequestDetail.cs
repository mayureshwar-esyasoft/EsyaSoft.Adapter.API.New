using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeasurementTaskRequestDetail
{
    public long MeasurementTaskRequestDetailId { get; set; }

    public Guid MeasurementTaskRequestDetailAltId { get; set; }

    public Guid MeasurementTaskRequestHeaderAltId { get; set; }

    public long MeasurementTaskRequestHeaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public Guid MeasurementTaskRequestDetailMhuuid { get; set; }

    public DateTime MeasurementTaskRequestDetailMhcreationTime { get; set; }

    public string MeasurementTaskRequestDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string UtilitiesMeasurementTaskId { get; set; } = null!;

    public string? UtilitiesObjectIdentificationSystemCodeText { get; set; }

    public bool UtilitiesPointOfDeliveryAssignmentListCompleteTransmissionIndicator { get; set; }

    public bool DeviceAssignmentListCompleteTransmissionIndicator { get; set; }

    public DateOnly? DeviceAssignmentStartDate { get; set; }

    public DateOnly? DeviceAssignmentEndDate { get; set; }

    public int? DeviceAssignmentTypeCode { get; set; }

    public decimal? UtilitiesQuantityAdjustmentFactorValue { get; set; }

    public string? AssignedUtilitiesDeviceId { get; set; }

    public string UtilitiesDeviceId { get; set; } = null!;

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
