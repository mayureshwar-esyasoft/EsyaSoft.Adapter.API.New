using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class ServiceCallLog
{
    public long EntryId { get; set; }

    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string ServiceParamJson { get; set; } = null!;

    public DateTime CallTimings { get; set; }

    public bool? IsSuccess { get; set; }

    public string Remark { get; set; } = null!;
}
