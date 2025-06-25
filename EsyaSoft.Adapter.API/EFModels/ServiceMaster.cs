using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class ServiceMaster
{
    public int ServiceId { get; set; }

    public Guid ServiceAltId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceEndPointName { get; set; } = null!;

    public string ServiceBusinessName { get; set; } = null!;

    public string MessageDesc { get; set; } = null!;

    public string ServiceDescription { get; set; } = null!;

    public int ServiceDirectionTypeId { get; set; }

    public string? WsdlservicePayLoad { get; set; }

    public string? Xmlpayload { get; set; }

    public bool IsDeveloped { get; set; }

    public bool IsDeployed { get; set; }

    public DateTime? DeploymentDate { get; set; }

    public string? DeployedBy { get; set; }
}
