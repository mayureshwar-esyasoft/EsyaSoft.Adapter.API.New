using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class ServiceDirectionType
{
    public int ServiceDirectionTypeId { get; set; }

    public string ServiceDirectionTypeName { get; set; } = null!;

    public string ServiceDirectionTypeDescription { get; set; } = null!;
}
