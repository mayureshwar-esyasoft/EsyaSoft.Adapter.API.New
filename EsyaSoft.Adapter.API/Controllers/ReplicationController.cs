using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace EsyaSoft.Adapter.API.Controllers
{
    public class ReplicationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<ReplicationController> _logger;
        private readonly ILogger<ReplicationConfirmationController> _confirmationLogger;

        private readonly string SectionHeader = "EndPoints";
        //private readonly string key = "RegisterCreate";
        private static readonly HttpClient httpClient = new HttpClient();

        public ReplicationController(ILogger<ReplicationController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<ReplicationConfirmationController> confirmationLogger)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;

            _confirmationLogger = confirmationLogger;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        #region BULK - SAP OUT - Replication BULK [#22]

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForReplicationBulk = ServiceEnum.ReplicationDeviceBulk.ToString();
        private readonly int ServiceEnumValueForReplicationBulk = (int)ServiceEnum.ReplicationDeviceBulk;
        
        //[Authorize]
        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterReplicationBulkRequest_OutService")]
        [Consumes("text/xml")]
        public void ReplicationBulk([FromBody] XElement Data)
        {

            _logger.LogInformation("Debug:ReplicationBulk >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterReplicationBulkRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {

                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterReplicationBulkRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterReplicationBulkRequestRoot>(Data.ToString());
                }
                catch (Exception)
                {

                    throw;
                }
                ///=====>>> XSS Part End ======
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForReplicationBulk, ServiceEnumValueForReplicationBulk);
                    _logger.LogInformation("Debug:ReplicationBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ReplicationBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : ReplicationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");

                    ContextRepository.SaveReplicationRequestBULK(_dbContext, IDval, obj, "BULK", keyForReplicationBulk, ServiceEnumValueForReplicationBulk);

                    _logger.LogInformation("Debug : ReplicationBulk >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************
                        
                        ReplicationConfirmationController RegisterCreateConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = RegisterCreateConfirmationControllerObj.SendReplicationConfirmationBulk(obj);

                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {
                            //Update Confirmation Header
                            //********* TODO >>> isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
                            ContextRepository.UpdateConfirmationSent(_dbContext, IDval, "BULK");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForReplicationBulk, ServiceEnumValueForReplicationBulk);
                            _logger.LogInformation("Debug:MeterReadingBulk >> Updated DB with Final Value 1 - Success");
                        }
                        
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:ReplicationBulk: {ex.Message}");
                    _logger.LogCritical("Debug:ReplicationBulk >> Error occurred: " + ex.Message.ToString());
                }
            }

        }
        #endregion

    }
}
