using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using EsyaSoft.Adapter.Domain.Models.RegisterChange.Single;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class RegisterChangeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<RegisterChangeController> _logger;
        private readonly ILogger<RegisterChangeConfirmationController> _confirmationLogger;

        private readonly string SectionHeader = "EndPoints";
        private readonly string key = "RegisterChange";
        private static readonly HttpClient httpClient = new HttpClient();

        public RegisterChangeController(ILogger<RegisterChangeController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<RegisterChangeConfirmationController> confirmationLogger)
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

        #region SINGLE - SAP OUT - Register Change Single [#17]

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.RegisterChangeSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.RegisterChangeSingle;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterChangeRequest_OutService")]
        [Consumes("text/xml")]
        public void RegisterChangeSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:RegisterChangeSingle >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterChangeRequest_Out");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterRegisterChangeRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterRegisterChangeRequestRoot>(Data.ToString());
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

                     ContextRepository.SaveMeterRegisterChangeRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : RegisterCreateSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    ////Update Success/Failure
                    //ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    //_logger.LogInformation("Debug:RegisterCreateSingle >> Updated DB with Final Value 1 - Success");

                    //// Return success response
                    ////return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);

                    #region RETURN CONFIRMATION
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************
                        
                        RegisterChangeConfirmationController RegisterChangeConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = RegisterChangeConfirmationControllerObj.SendMeterRegisterChangeConfirmationSingle(obj);
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


        #region BULK - SAP OUT - Register Change BULK [#19]

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForRegisterChangeBulk = ServiceEnum.RegisterChangeBulk.ToString();
        private readonly int ServiceEnumValueForRegisterChangeBulk = (int)ServiceEnum.RegisterChangeBulk;
        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest_OutService")]
        [Consumes("text/xml")]
        public void RegisterChangeBulk([FromBody] XElement Data)
        {

            _logger.LogInformation("Debug:RegisterChangeBulk >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {

                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequestRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequestRoot>(Data.ToString());
                }
                catch (Exception)
                {

                    throw;
                }
                ///=====>>> XSS Part End ======
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForRegisterChangeBulk, ServiceEnumValueForRegisterChangeBulk);
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

                     ContextRepository.SaveMeterRegisterChangeRequestBULK(_dbContext, IDval, obj, "BULK", keyForRegisterChangeBulk, ServiceEnumValueForRegisterChangeBulk);

                    _logger.LogInformation("Debug : MeterReadingBulk >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    try
                    {
                        
                        //***** Region to Start Creating for Confirmation Payload - START *****************
                        RegisterChangeConfirmationController RegisterChangeConfirmationControllerObj = new(_confirmationLogger, _dbContext, _configuration);
                        string confirmationPayload = RegisterChangeConfirmationControllerObj.SendMeterRegisterChangeConfirmationBulk(obj);

                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {
                            //Update Confirmation Header
                            //********* TODO >>> isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
                            ContextRepository.UpdateConfirmationSent(_dbContext, IDval, "BULK");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForRegisterChangeBulk, ServiceEnumValueForRegisterChangeBulk);
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
