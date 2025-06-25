using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncConsumerAddress
{
    public long FlexSyncConsumerAddressId { get; set; }

    public Guid FlexSyncConsumerAddressAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? ConsumerAddressId { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? StateOrProvince { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Timezone { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
