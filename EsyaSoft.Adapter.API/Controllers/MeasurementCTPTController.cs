using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class MeasurementCTPTController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<MeasurementCTPTController> _logger;
        //private readonly ILogger<DeviceCreateConfirmationController> _confirmationLogger;

        private static readonly HttpClient httpClient = new HttpClient();

        public MeasurementCTPTController(ILogger<MeasurementCTPTController> logger, AdapterContext dbContext,
            IConfiguration configuration)//, ILogger<DeviceCreateConfirmationController> confirmationLogger)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;

            //_confirmationLogger = confirmationLogger;
        }

        #region SINGLE - SAP OUT - Device Create Single

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.MeasurementCTPTSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.MeasurementCTPTSingle;

        [HttpPost("SAPAdapterWS/SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification_OutService")]
        [Consumes("text/xml")]

        public void MeasurementCTPTRequestSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:MeasurementCTPTRequestSingle >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotificationRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotificationRoot>(Data.ToString());
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
                    

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : DeviceCreateSingle >> Data Parsing Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    _logger.LogInformation("Debug : DeviceCreateSingle >> DB Insert Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    ContextRepository.SaveMeasurmentCTPTRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : DeviceCreateSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:MeterReadingSingle >> Updated DB with Final Value 1 - Success");
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
