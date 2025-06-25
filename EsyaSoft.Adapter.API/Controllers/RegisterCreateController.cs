using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{

    public class RegisterCreateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<RegisterCreateController> _logger;
        private readonly ILogger<RegisterCreateConfirmationController> _confirmationLogger;

        private readonly string SectionHeader = "EndPoints";
        private readonly string key = "RegisterCreate";
        private static readonly HttpClient httpClient = new HttpClient();

        public RegisterCreateController(ILogger<RegisterCreateController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<RegisterCreateConfirmationController> confirmationLogger)
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

        #region SINGLE - SAP OUT - Register Create Single [#13]

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.RegisterCreateSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.RegisterCreateSingle;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterCreateRequest_OutService")]
        [Consumes("text/xml")]

        public void RegisterCreateSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:RegisterCreateSingle >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterRegisterCreateRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterRegisterCreateRequestRoot>(Data.ToString());
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
                    _logger.LogInformation("Debug:RegisterCreateSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:RegisterCreateSingle: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : RegisterCreateSingle >> Data Parsing Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    _logger.LogInformation("Debug : RegisterCreateSingle >> DB Insert Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    ContextRepository.SaveMeterRegisterCreateRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : RegisterCreateSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    #region RETURN CONFIRMATION
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        RegisterCreateConfirmationController RegisterCreateConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = RegisterCreateConfirmationControllerObj.SendMeterRegisterCreateConfirmationSingle(obj);
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
                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - "+ Data.Length.ToString());
                    #endregion
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:RegisterCreateSingle: {ex.Message}");
                    _logger.LogCritical("Debug:RegisterCreateSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion

        #region BULK - SAP OUT - Register Create BULK [#15]

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForRegisterCreateBulk = ServiceEnum.RegisterCreateBulk.ToString();
        private readonly int ServiceEnumValueForRegisterCreateBulk = (int)ServiceEnum.RegisterCreateBulk;
        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequest_OutService")]
        [Consumes("text/xml")]
        public void RegisterCreateBulk([FromBody] XElement Data)
        {

            _logger.LogInformation("Debug:RegisterCreateBulk >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {

                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequestRoot>(Data.ToString());
                }
                catch (Exception)
                {

                    throw;
                }
                ///=====>>> XSS Part End ======
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForRegisterCreateBulk, ServiceEnumValueForRegisterCreateBulk);
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

                    ContextRepository.SaveMeterRegisterCreateRequestBULK(_dbContext, IDval, obj, "BULK", keyForRegisterCreateBulk, ServiceEnumValueForRegisterCreateBulk);

                    _logger.LogInformation("Debug : MeterReadingBulk >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************
                        RegisterCreateConfirmationController RegisterCreateConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = RegisterCreateConfirmationControllerObj.SendMeterRegisterCreateConfirmationBulk(obj);

                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {
                            //Update Confirmation Header
                            //********* TODO >>> isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
                            ContextRepository.UpdateConfirmationSent(_dbContext, IDval, "BULK");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForRegisterCreateBulk, ServiceEnumValueForRegisterCreateBulk);
                            _logger.LogInformation("Debug:MeterReadingBulk >> Updated DB with Final Value 1 - Success");
                        }
                        
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
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

        
    }
}
