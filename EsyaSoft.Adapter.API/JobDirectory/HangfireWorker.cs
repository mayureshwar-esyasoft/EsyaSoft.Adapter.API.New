using EsyaSoft.Adapter.API.Controllers;
using Hangfire;
using EsyaSoft.Adapter.API.EFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;

namespace EsyaSoft.Adapter.API
{
    public static class HangfireWorker
    {
        private static IConfiguration _configuration;
        private static AdapterContext _dbContext;

        private static ILogger<MeterReadRequestConfirmationController> _loggerMeterReadRequestConfirmationController;
        private static ILogger<ConnectionDisconnectionController> _loggeronnectionDisconnectionControlle;

        private static string HangfireIntervalSection = "HangfireInterval";

        
       public static void StartRecurringJobs(this IApplicationBuilder app)
        {

            ///* INTERVAL FOR JOBS : START */
            string? MeterReadSINGLERESULTConfirmationInterval = "*/3 * * * *";// _configuration.GetSection("HangfireInterval")["MeterReadSINGLERESULTConfirmationInterval"];
            string? MeterReadBULKRESULTConfirmationInterval = "*/15 * * * *"; //_configuration.GetSection("HangfireInterval")["MeterReadBULKRESULTConfirmationInterval"];
            string? ConnectionDisconnectionConfirmationSINGLEInterval = "*/2 * * * *"; //_configuration.GetSection("HangfireInterval")["ConnectionDisconnectionConfirmationSINGLEInterval"];

            /* INTERVAL FOR JOBS : END */




            MeterReadRequestConfirmationController objMeterReadRequestConfirmationController
                = new(_loggerMeterReadRequestConfirmationController, _dbContext, _configuration);
            
            //==>> #1: Single Meter Read Result
            RecurringJob.AddOrUpdate("MeterReadSINGLERESULTConfirmation",
                () => objMeterReadRequestConfirmationController.MeterReadSINGLERESULTConfirmation(),
                string.IsNullOrEmpty(MeterReadSINGLERESULTConfirmationInterval) ? "*/3 * * * *" : MeterReadSINGLERESULTConfirmationInterval, TimeZoneInfo.Local);

            //==>> #2: BULK Meter Read Result
            RecurringJob.AddOrUpdate("MeterReadBULKRESULTConfirmation",
                () => objMeterReadRequestConfirmationController.MeterReadBULKRESULTConfirmation(),
                string.IsNullOrEmpty(MeterReadBULKRESULTConfirmationInterval) ? "*/15 * * * *" : MeterReadBULKRESULTConfirmationInterval,
                TimeZoneInfo.Local);


            //==>> #3: Connection Disconnection Single
            ConnectionDisconnectionController objConnectionDisconnectionController
                    = new(_loggeronnectionDisconnectionControlle, _dbContext, _configuration);
            RecurringJob.AddOrUpdate("ConnectionDisconnectionConfirmationSINGLE",
                () => objConnectionDisconnectionController.ConnectionDisconnectionConfirmationSINGLE(),
                string.IsNullOrEmpty(ConnectionDisconnectionConfirmationSINGLEInterval) ? "*/2 * * * *" : ConnectionDisconnectionConfirmationSINGLEInterval,
                TimeZoneInfo.Local);

        }

    }
}
