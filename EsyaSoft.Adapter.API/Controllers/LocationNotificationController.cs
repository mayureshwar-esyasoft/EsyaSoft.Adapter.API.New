using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class LocationNotificationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<LocationNotificationController> _logger;

        private static readonly HttpClient httpClient = new HttpClient();

        public LocationNotificationController(ILogger<LocationNotificationController> logger, AdapterContext dbContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Bulk - SAP OUT - Location Notification Bulk

        private readonly string keyForBulk = ServiceEnum.LocationNotificationBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.LocationNotificationBulk;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterLocationBulkNotification_OutService")]
        [Consumes("text/xml")]
        public void LocationNotificationBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:LocationNotificationBulk >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterLocationBulkNotification_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterLocationBulkNotificationRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterLocationBulkNotificationRoot>(Data.ToString());
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
                    _logger.LogInformation("Debug:LocationNotificationBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:LocationNotificationBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }
                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : ReplicationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");

                    ContextRepository.SaveLocationRequestBULK(_dbContext, IDval, obj, "BULK", keyForBulk, ServiceEnumValueForBulk);

                    _logger.LogInformation("Debug : ReplicationBulk >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:LocationNotificationBulk >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:LocationNotificationBulk: {ex.Message}");
                    _logger.LogCritical("Debug:LocationNotificationBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion

        #region SINGLE - SAP OUT - Location Notification Single

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.LocationNotificationSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.LocationNotificationSingle;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterLocationNotification_OutService")]
        [Consumes("text/xml")]

        public void LocationNotificationSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:LocationNotificationSingle >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterLocationNotification_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                //
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                UtilitiesDeviceERPSmartMeterLocationNotificationRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<UtilitiesDeviceERPSmartMeterLocationNotificationRoot>(Data.ToString());
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
                    _logger.LogInformation("Debug:LocationNotificationSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:LocationNotificationSingle: {Data}");
                    
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : LocationNotificationSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");

                    ContextRepository.SaveLocationRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle);

                    _logger.LogInformation("Debug : LocationNotificationSingle >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:LocationNotificationSingle >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:LocationNotificationSingle: {ex.Message}");
                    _logger.LogCritical("Debug:LocationNotificationSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion
        
    }
}
