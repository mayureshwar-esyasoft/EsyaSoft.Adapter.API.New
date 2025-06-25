using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using EsyaSoft.Adapter.Domain.Models.ResultConfirmationFromSAPOUT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class BillingMeterReadRequestController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<BillingMeterReadRequestController> _logger;

        private static readonly HttpClient httpClient = new HttpClient();

        public BillingMeterReadRequestController(ILogger<BillingMeterReadRequestController> logger, AdapterContext dbContext,
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

        #region Bulk 39 - SAP OUT - Billing Meter Read Request

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForBulk = ServiceEnum.BillingMeterReadRequestBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.BillingMeterReadRequestBulk;

        [HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateConfirmation_OutService")]
        [Consumes("text/xml")]
        [Authorize]
        public void BillingMeterReadRequestBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:BillingMeterReadRequestBulk >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateConfirmation_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                MeterReadingDocumentERPResultBulkCreateConfirmationRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<MeterReadingDocumentERPResultBulkCreateConfirmationRoot>(Data.ToString());
                }
                catch (Exception)
                {

                    throw;
                }
                ///=====>>> XSS Part End ======
                try
                {
                    //SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:BillingMeterReadRequestBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:BillingMeterReadRequestBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:BillingMeterReadRequestBulk >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:BillingMeterReadRequestBulk: {ex.Message}");
                    _logger.LogCritical("Debug:BillingMeterReadRequestBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion


        #region SINGLE 38 - SAP OUT - Billing Meter Read Request

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.BillingMeterReadRequestSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.BillingMeterReadRequestSingle;

        [HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultCreateConfirmation_OutService")]
        [Consumes("text/xml")]
        [Authorize]
        public void BillingMeterReadRequestSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:BillingMeterReadRequestSingle >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultCreateConfirmation_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                MeterReadingDocumentERPResultCreateConfirmationRoot obj = null;
                try
                {
                    XMLHelper objXML = new XMLHelper();
                    obj = objXML.Deserialize<MeterReadingDocumentERPResultCreateConfirmationRoot>(Data.ToString());
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
                    _logger.LogInformation("Debug:BillingMeterReadRequestSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:BillingMeterReadRequestSingle: {Data}");
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:BillingMeterReadRequestSingle >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:BillingMeterReadRequestSingle: {ex.Message}");
                    _logger.LogCritical("Debug:BillingMeterReadRequestSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion
    }
}
