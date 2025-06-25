using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class Mdrdetail
{
    public long MdrdetailId { get; set; }

    public Guid MdrdetailAltId { get; set; }

    public Guid MdrheaderAltId { get; set; }

    public long MdrheaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public Guid DetailMhuuid { get; set; }

    public DateTime DetailMhcreationTime { get; set; }

    public string DetailMhsenderBusinessSystemId { get; set; } = null!;

    public string MeterReadingDocumentId { get; set; } = null!;

    public int MeterReadingReasonCode { get; set; }

    public DateTime ScheduledMeterReadingDate { get; set; }

    public int UtilitiesAdvancedMeteringDataSourceTypeCode { get; set; }

    public string UtilitiesMeasurementTaskId { get; set; } = null!;

    public string UtilitiesObjectIdentificationSystemCodeText { get; set; } = null!;

    public string UtilitiesDeviceId { get; set; } = null!;

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
