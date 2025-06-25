using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncConsumerConsumerAddressAssociation
{
    public long FlexSyncConsumerConsumerAddressAssociationId { get; set; }

    public Guid FlexSyncConsumerConsumerAddressAssociationAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? ConsumerId { get; set; }

    public string? ConsumerTypeDesc { get; set; }

    public Guid? FlexSyncConsumerAltId { get; set; }

    public string? ConsumerAddressId { get; set; }

    public string? ConsumerAddressTypeDesc { get; set; }

    public Guid? FlexSyncConsumerAddressAltId { get; set; }

    public string? Status { get; set; }

    public string? AddressRole { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
