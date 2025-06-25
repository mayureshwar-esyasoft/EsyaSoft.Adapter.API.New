using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace EsyaSoft.Adapter.API.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class DeviceCreateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<DeviceCreateController> _logger;
        private readonly ILogger<DeviceCreateConfirmationController> _confirmationLogger;

        private static readonly HttpClient httpClient = new HttpClient();

        public DeviceCreateController(ILogger<DeviceCreateController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<DeviceCreateConfirmationController> confirmationLogger)
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

        #region Bulk - SAP OUT - Device Create Bulk

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForBulk = ServiceEnum.DeviceCreateBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.DeviceCreateBulk;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterBulkCreateRequest_OutService")]
        [Consumes("text/xml")]

        public void DeviceCreateBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:MeterReadCancelBulk >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterBulkCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:DeviceCreateBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:DeviceCreateBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization
                    //XMLHelper objXML = new XMLHelper();
                    //SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot>(Data.ToString());
                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:DeviceCreateBulk >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:DeviceCreateBulk: {ex.Message}");
                    _logger.LogCritical("Debug:DeviceCreateBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion

        #region SINGLE - SAP OUT - Device Create Single

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.DeviceCreateSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.DeviceCreateSingle;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterCreateRequest_OutService")]
        [Consumes("text/xml")]

        public void DeviceCreateSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:DeviceCreateSingle >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterCreateRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterCreateRequestRoot>(Data.ToString());
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

                    ContextRepository.SaveMeterCreateRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : DeviceCreateSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    #region RETURN CONFIRMATION
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        DeviceCreateConfirmationController DeviceCreateConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = DeviceCreateConfirmationControllerObj.SendMeterCreateConfirmationSingle(obj);
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

    }
}
