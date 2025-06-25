using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class DeviceChangeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<DeviceChangeController> _logger;
        private readonly ILogger<DeviceChangeConfirmationController> _confirmationLogger;

        private static readonly HttpClient httpClient = new HttpClient();

        public DeviceChangeController(ILogger<DeviceChangeController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<DeviceChangeConfirmationController> confirmationLogger)
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

        #region SINGLE - SAP OUT - Device Change Single

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.DeviceChangeSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.DeviceChangeSingle;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterChangeRequest_OutService")]
        [Consumes("text/xml")]
        public void DeviceChangeSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:DeviceChangeSingle >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterChangeRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterChangeRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterChangeRequestRoot>(Data.ToString());
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
                    _logger.LogInformation("Debug:DeviceCreateSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:DeviceCreateSingle: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }
                    /* COMMENTED 20.11
                    #region serialization
                    //XMLHelper objXML = new XMLHelper();
                    //SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot>(Data.ToString());
                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:DeviceCreateSingle >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);

                    */

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : DeviceCreateSingle >> Data Parsing Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    _logger.LogInformation("Debug : DeviceCreateSingle >> DB Insert Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    ContextRepository.SaveMeterChangeRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : DeviceCreateSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    #region RETURN CONFIRMATION
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        DeviceChangeConfirmationController DeviceChangeConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = DeviceChangeConfirmationControllerObj.SendMeterChangeConfirmationSingle(obj);
                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {

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
                    #endregion
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:DeviceCreateSingle: {ex.Message}");
                    _logger.LogCritical("Debug:DeviceCreateSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion

        #region BULK - SAP OUT - Device Change BULK & Confirmation [9 & 10]

        private readonly string keyForBulk = ServiceEnum.DeviceChangeBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.DeviceChangeBulk;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterBulkChangeRequest_Outervice")]
        [Consumes("text/xml")]
        public void DeviceChangeBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:DeviceChangeBulk >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterBulkChangeRequest_Outervice");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterBulkChangeRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterBulkChangeRequestRoot>(Data.ToString());
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
                    _logger.LogInformation("Debug:DeviceChangeBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:DeviceChangeBulk: {Data}");
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : DeviceChangeBulk >> Data Parsing Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    _logger.LogInformation("Debug : DeviceChangeBulk >> DB Insert Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    ContextRepository.SaveMeterChangeRequestBULK(_dbContext, IDval, obj, "BULK", keyForBulk, ServiceEnumValueForBulk); //, confirmationPayload);

                    _logger.LogInformation("Debug : DeviceCreateSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    #region RETURN CONFIRMATION
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        DeviceChangeConfirmationController DeviceChangeConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload =DeviceChangeConfirmationControllerObj.SendMeterChangeConfirmationBULK(obj);
                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {

                            //Update Confirmation Header
                            //********* TODO >>> isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
                            ContextRepository.UpdateConfirmationSent(_dbContext, IDval, "BULK");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                            _logger.LogInformation("Debug:DeviceChangeBulk >> Updated DB with Final Value 1 - Success");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:DeviceChangeBulk: {ex.Message}");
                    _logger.LogCritical("Debug:DeviceChangeBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion
    }
}
