using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class RegisterSpecificDetail
{
    public long RegisterSpecificDetailId { get; set; }

    public Guid RegisterSpecificDetailAltId { get; set; }

    public long ReferenceDetailId { get; set; }

    public Guid ReferenceDetailAltId { get; set; }

    public long MasterServiceCallLogId { get; set; }

    public string? PayloadType { get; set; }

    public string? RegisterSourceType { get; set; }

    public DateOnly RegisterStartDate { get; set; }

    public DateOnly RegisterEndDate { get; set; }

    public string UtilitiesMeasurementTaskId { get; set; } = null!;

    public string UtilitiesObjectIdentificationSystemCodeText { get; set; } = null!;

    public int UtiltiesMeasurementTaskCategoryCode { get; set; }

    public int UtilitiesDivisionCategoryCode { get; set; }

    public string TimeZoneCode { get; set; } = null!;

    public string SpecMeasureUnitCode { get; set; } = null!;

    public int DecimalValuePrecisionTotalDigitNumberValue { get; set; }

    public int DecimalValuePrecisionFractionDigitNumberValue { get; set; }

    public decimal SpecMeterReadingResultAdjustmentFactorValue { get; set; }

    public bool IsActive { get; set; }

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }
}
