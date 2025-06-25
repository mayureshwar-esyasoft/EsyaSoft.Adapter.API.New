using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class MeterLocationDetail
{
    public long MeterLocationDetailId { get; set; }

    public Guid MeterLocationDetailAltId { get; set; }

    public Guid MeterLocationHeaderAltId { get; set; }

    public long MeterLocationHeaderId { get; set; }

    public long ParentServiceCallLogId { get; set; }

    public Guid MeterLocationDetailMhuuid { get; set; }

    public DateTime MeterLocationDetailMhcreationTime { get; set; }

    public string MeterLocationDetailMhsenderBusinessSystemId { get; set; } = null!;

    public string UtilitiesDeviceId { get; set; } = null!;

    public bool LogicalLocationListCompleteTransmissionIndicator { get; set; }

    public bool LocationListCompleteTransmissionIndicator { get; set; }

    public DateOnly LogicalLocationStartDate { get; set; }

    public DateOnly LogicalLocationEndDate { get; set; }

    public string LogicalInstallationPointId { get; set; } = null!;

    public DateOnly LocationStartDate { get; set; }

    public DateOnly LocationEndDate { get; set; }

    public string InstallationPointId { get; set; } = null!;

    public string StreetPostalCode { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string RegionCode { get; set; } = null!;

    public string TimeZoneCode { get; set; } = null!;

    public string ParentInstallationPointId { get; set; } = null!;

    public DateOnly InstallationDate { get; set; }

    public DateOnly RemoveDate { get; set; }

    public string ModificationTimeZoneCode { get; set; } = null!;

    public string UtilitiesAdvancedMeteringSystemId { get; set; } = null!;

    public bool IsItemProcessed { get; set; }

    public bool? IsConfirmationSent { get; set; }

    public bool? IsMdminvoked { get; set; }

    public bool IsCancelled { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
