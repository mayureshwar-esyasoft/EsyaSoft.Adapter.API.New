using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class FlexSyncConsumer
{
    public long FlexSyncConsumerId { get; set; }

    public Guid FlexSyncConsumerAltId { get; set; }

    public Guid FlexSyncHeaderAltId { get; set; }

    public string? ConsumerId { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
