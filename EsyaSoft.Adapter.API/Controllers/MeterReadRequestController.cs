using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExtensionMethod;
using Microsoft.AspNetCore.Authorization;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class MeterReadRequestController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<MeterReadRequestController> _logger;
        private readonly ILogger<MeterReadRequestConfirmationController> _loggerConfirmationClass;

        private static readonly HttpClient httpClient = new HttpClient();


        
        public MeterReadRequestController(ILogger<MeterReadRequestController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<MeterReadRequestConfirmationController> loggerConfirmationClass)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;

            _loggerConfirmationClass = loggerConfirmationClass;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region BULK 33 - SAP OUT - Meter Read Request BULK

        
        private readonly string keyForBulk = ServiceEnum.MeterReadBulkRequest.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.MeterReadBulkRequest;

        [Authorize]
        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateRequest_OutService")]
        [Consumes("text/xml")]
        public void MeterReadingBulk([FromBody] XElement Data)
        {

            _logger.LogInformation("Debug:MeterReadingBulk >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateConfirmation_Out");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {

                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot>(Data.ToString());
                }
                catch (Exception)
                {

                    throw;
                }
                ///=====>>> XSS Part End ======
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:MeterReadingBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:MeterReadingBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : MeterReadRequestConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
                    
                    ContextRepository.SaveMeterReadRequestTransactionBULK(_dbContext, IDval, obj, "BULK", keyForSingle, ServiceEnumValueForSingle);

                    _logger.LogInformation("Debug : MeterReadingBulk >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        MeterReadRequestConfirmationController MeterReadRequestConfirmationControllerObj = new(_loggerConfirmationClass, _dbContext, _configuration);
                        string confirmationPayload = MeterReadRequestConfirmationControllerObj.MeterReadRequestConfirmationBulk(obj);
                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {
                            //Update Confirmation Header
                            //********* TODO >>> isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
                            ContextRepository.UpdateConfirmationSent(_dbContext, IDval, "BULK");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
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
                    Console.WriteLine($"Error occurred:MeterReadingBulk: {ex.Message}");
                    _logger.LogCritical("Debug:MeterReadingBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion

        #region SINGLE 32 - SAP OUT - Meter Read Request

        private readonly string keyForSingle = ServiceEnum.MeterReadSingleRequest.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.MeterReadSingleRequest;

        [Authorize]
        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateRequest_OutService")]
        [Consumes("text/xml")]
        public void MeterReadingSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:MeterReadingSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;
            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                SmartMeterMeterReadingDocumentERPCreateRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCreateRequestRoot>(Data.ToString());
                }
                catch (Exception)
                {

                    throw;
                }
                ///=====>>> XSS Part End ======

                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:MeterReadingSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:MeterReadingSingle: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Data Parsing Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    _logger.LogInformation("Debug : MeterReadSingleConfirmation >> DB Insert Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    ContextRepository.SaveMeterReadRequestTransactionSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************
                        
                        MeterReadRequestConfirmationController MeterReadRequestConfirmationControllerObj = new(_loggerConfirmationClass, _dbContext, _configuration);
                        string confirmationPayload = MeterReadRequestConfirmationControllerObj.MeterReadSingleConfirmationFromReadController(obj);
                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK")) {

                            //Update Confirmation Header
                            //********* TODO >>> isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
                            ContextRepository.UpdateConfirmationSent(_dbContext, IDval, "SINGLE");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                            _logger.LogInformation("Debug:MeterReadingSingle >> Updated DB with Final Value 1 - Success");
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
                    Console.WriteLine($"Error occurred:MeterReadingSingle: {ex.Message}");
                    _logger.LogCritical("Debug:MeterReadingSingle >> Error occurred: " + ex.Message.ToString());

                }
            }
        }

        #endregion

    }
}
