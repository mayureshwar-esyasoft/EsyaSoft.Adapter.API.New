using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterRegisterDetail
{
    public long MeterRegisterDetailId { get; set; }

    public Guid MeterRegisterDetailAltId { get; set; }

    public Guid MeterRegisterHeaderAltId { get; set; }

    public long MeterRegisterHeaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? RegisterType { get; set; }

    public Guid MeterRegisterDetailMhuuid { get; set; }

    public DateTime MeterRegisterDetailMhcreationTime { get; set; }

    public string MeterRegisterDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string UtilitiesDeviceId { get; set; } = null!;

    public int RegisterListCompleteTransmissionIndicatorBool { get; set; }

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
