using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterRegisterCreateDetail
{
    public long MeterRegisterCreateDetailId { get; set; }

    public Guid MeterRegisterCreateDetailAltId { get; set; }

    public Guid MeterRegisterCreateHeaderAltId { get; set; }

    public long MeterRegisterCreateHeaderId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public Guid MeterRegisterCreateDetailMhuuid { get; set; }

    public DateTime MeterRegisterCreateDetailMhcreationTime { get; set; }

    public string MeterRegisterCreateDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string UtilitiesDeviceId { get; set; } = null!;

    public DateOnly RegisterStartDate { get; set; }

    public DateOnly RegisterEndDate { get; set; }

    public string UtilitiesMeasurementTaskId { get; set; } = null!;

    public string UtilitiesObjectIdentificationSystemCodeText { get; set; } = null!;

    public int UtiltiesMeasurementTaskCategoryCode { get; set; }

    public int UtilitiesDivisionCategoryCode { get; set; }

    public string? TimeZoneCode { get; set; }

    public string SpecMeasureUnitCode { get; set; } = null!;

    public int DecimalValuePrecisionTotalDigitNumberValue { get; set; }

    public decimal SpecMeterReadingResultAdjustmentFactorValue { get; set; }

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
