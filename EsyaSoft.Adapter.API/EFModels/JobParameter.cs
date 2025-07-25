﻿using System;
using System.Collections.Generic;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class JobParameter
{
    public long JobId { get; set; }

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public virtual Job Job { get; set; } = null!;
}
