using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterChangeRequestDetail
{
    public long MeterChangeRequestDetailId { get; set; }

    public Guid MeterChangeRequestDetailAltId { get; set; }

    public Guid MeterChangeRequestHeaderAltId { get; set; }

    public long MeterChangeRequestHeaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public Guid MeterChangeRequestDetailMhuuid { get; set; }

    public DateTime MeterChangeRequestDetailMhcreationTime { get; set; }

    public string MeterChangeRequestDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string UtilitiesDeviceId { get; set; } = null!;

    public string UtilitiesDeviceSerialId { get; set; } = null!;

    public string UtilitiesDeviceMaterialId { get; set; } = null!;

    public string PartyInternalId { get; set; } = null!;

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
