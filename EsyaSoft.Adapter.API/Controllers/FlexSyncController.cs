using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class FlexSyncController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<FlexSyncController> _logger;

        private static readonly HttpClient httpClient = new HttpClient();

        
        public FlexSyncController(ILogger<FlexSyncController> logger, AdapterContext dbContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        private readonly string keyForFlexSync= ServiceEnum.ReadFlexSyncRequest.ToString();
        private readonly int ServiceEnumValueForFlexSync = (int)ServiceEnum.ReadFlexSyncRequest;

        [HttpPost("SAPAdapterWS/MT_FlexSyncAMI_Req_OutService")]
        //[HttpPost("SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest_OutService")]

        [Consumes("text/xml")]
        public void ReadFlexSyncRequest([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:DeviceCreReadFlexSyncRequestateSingle >> Reached the controller - SAPAdapterWS/ReadFlexSyncRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {
                    ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                    MTFlexSyncAMIReqRoot obj = null;
                    try
                    {
                        XMLHelper objXML = new XMLHelper();
                        obj = objXML.Deserialize<MTFlexSyncAMIReqRoot>(Data.ToString());

                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Console.WriteLine($"Error occurred:ReadFlexSyncRequest could not parsed: {ex.Message}");
                        _logger.LogCritical("Debug:ReadFlexSyncRequest Parsing Issue >> Error occurred: " + ex.Message.ToString());
                    }
                    // ************** FlexSync Parsing
                    
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForFlexSync, ServiceEnumValueForFlexSync);
                    _logger.LogInformation("Debug:ReadFlexSyncRequest >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ReadFlexSyncRequest: {Data}");
                    
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : ReadFlexSyncRequest >> Data Insert to HEADER and DETAIL Tables of FlexSync Request - Starts: Serilizaing - SAPAdapterWS/ReadFlexSyncRequest_OutService");

                    ContextRepository.FLexSyncRequestParse(_dbContext, IDval, obj, "SINGLE", keyForFlexSync, ServiceEnumValueForFlexSync); //, confirmationPayload);

                    _logger.LogInformation("Debug : ReadFlexSyncRequest >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/ReadFlexSyncRequest_OutService");

                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForFlexSync, ServiceEnumValueForFlexSync);
                    _logger.LogInformation("Debug:ReadFlexSyncRequest >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                    


                    //***********FlexSync Parsing
                }
                catch (Exception ex)
                {

                    // Log the exception
                    Console.WriteLine($"Error occurred:ReadFlexSyncRequest: {ex.Message}");
                    _logger.LogCritical("Debug:ReadFlexSyncRequest >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return fa
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
