using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;

namespace EsyaSoft.Adapter.API.Controllers
{

    public class MeterCreateBulkRequestController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<MeterCreateBulkRequestController> _logger;

        private readonly string SectionHeader = "EndPoints";
        private readonly string key = "MeterCreateBulkRequest";
        private static readonly HttpClient httpClient = new HttpClient();

        public MeterCreateBulkRequestController(ILogger<MeterCreateBulkRequestController> logger, AdapterContext dbContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }


        public string GetRequest()
        {


            string? url = _configuration.GetSection(SectionHeader)[key];
            string result = string.Empty;
            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");

                //result = PostSOAPRequestAsync(url, xmlSOAP);
                //Console.WriteLine(result);

                Console.WriteLine("SAP IN POC - Result Returned : " + result);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                result = ex.Message;
            }


            return result;
            //return "Hello";
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateConfirmation_OutService_InValid")]
        //[Consumes("application/soap+xml")]
        [Consumes("text/xml")]

        public void MeterReadingBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:MeterReadingBulk >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateConfirmation_Out");


            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = SaveDBTransaction(Data.ToString());
                    _logger.LogInformation("Debug:MeterReadingBulk >> Saved Into DB with inital Value 0 - Initiation");

                    #region serialization
                    XMLHelper objXML = new XMLHelper();
                    MeterReadingDocumentERPResultBulkCreateRequest_Self obj = objXML.Deserialize<MeterReadingDocumentERPResultBulkCreateRequest_Self>(Data.ToString());
                    #endregion

                    // Process the received data
                    Console.WriteLine($"Received data:MeterReadingBulk: {Data}");
                    //SoapFormatter formatter = new SoapFormatter();
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    //Update Success/Failure
                    updateDBTransaction(IDval, RetStatus);
                    _logger.LogInformation("Debug:MeterReadingBulk >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - "+ Data.Length.ToString());
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:MeterReadingBulk: {ex.Message}");
                    _logger.LogCritical("Debug:MeterReadingBulk >> Error occurred: " + ex.Message.ToString());

                    // Return error response
                    //return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }


        #region Private methods
        private void updateDBTransaction(long IDval, Boolean status)
        {
            try
            {
                var entity = _dbContext.ServiceCallLogs.FirstOrDefault(itm => itm.EntryId == IDval);

                // Validate entity is not null
                if (entity != null)
                {

                    using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //var std = new Student() { StudentName = "Steve" };
                            entity.IsSuccess = status;
                            entity.Remark = "OUT call for - " + key + " Completed.";

                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }



                }
            }
            catch (Exception ex)
            {

                throw;
            }
            //throw new NotImplementedException();
        }
        private long SaveDBTransaction(string txtXML)
        {
            long returnVal = 0;
            try
            {
                ServiceCallLog obj = new ServiceCallLog()
                {
                    IsSuccess = false,
                    Remark = "OUT call for - " + key + " initiated.",
                    ServiceId = 6,
                    ServiceName = key,
                    CallTimings = DateTime.Now,
                    ServiceParamJson = txtXML
                };

                //using (var context = new SchoolContext())
                //{
                //    var std = new Student() { StudentName = "Steve" };
                //    context.Add(std);
                //    context.SaveChanges();

                //    Console.Write(std.StudentID); // 1
                //}
                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //var std = new Student() { StudentName = "Steve" };
                        _dbContext.Add(obj);
                        _dbContext.SaveChanges();

                        returnVal = obj.EntryId;

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
                return returnVal;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
