using EsyaSoft.Adapter.API.Controllers;
using Hangfire;
using EsyaSoft.Adapter.API.EFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;

namespace EsyaSoft.Adapter.API
{
    public interface IMeterReads
    {
        Task<string> MeterReadBULKRESULTConfirmationTask();
    }
}
