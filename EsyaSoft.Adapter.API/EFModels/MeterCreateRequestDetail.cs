using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterCreateRequestDetail
{
    public long MeterCreateRequestDetailId { get; set; }

    public Guid MeterCreateRequestDetailAltId { get; set; }

    public Guid MeterCreateRequestHeaderAltId { get; set; }

    public long MeterCreateRequestHeaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public Guid MeterCreateRequestDetailMhuuid { get; set; }

    public DateTime MeterCreateRequestDetailMhcreationTime { get; set; }

    public string MeterCreateRequestDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string UtilitiesDeviceId { get; set; } = null!;

    public DateOnly UtilitiesDeviceStartDate { get; set; }

    public DateOnly UtilitiesDeviceEndDate { get; set; }

    public string UtilitiesDeviceSerialId { get; set; } = null!;

    public string UtilitiesDeviceMaterialId { get; set; } = null!;

    public string? PartyInternalId { get; set; }

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
