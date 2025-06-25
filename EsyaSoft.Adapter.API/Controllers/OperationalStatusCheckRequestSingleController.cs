using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class OperationalStatusCheckRequestSingleController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<OperationalStatusCheckRequestSingleController> _logger;

        private static readonly HttpClient httpClient = new HttpClient();

        public OperationalStatusCheckRequestSingleController(ILogger<OperationalStatusCheckRequestSingleController> logger, AdapterContext dbContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }



        /// <summary>
        /// Default Action - Ignore
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        #region SINGLE - SAP OUT - Operational Status Check Request Single

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.OperationalStatusCheckRequestSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.OperationalStatusCheckRequestSingle;

        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterOperationalStateByIDQuery_OutService")]
        [Consumes("text/xml")]
        [Authorize]
        public void OperationalStatusCheckRequestSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:OperationalStatusCheckRequestSingle >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterOperationalStateByIDQuery_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:OperationalStatusCheckRequestSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:OperationalStatusCheckRequestSingle: {Data}");
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
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:OperationalStatusCheckRequestSingle >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:OperationalStatusCheckRequestSingle: {ex.Message}");
                    _logger.LogCritical("Debug:OperationalStatusCheckRequestSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion
    }
}
