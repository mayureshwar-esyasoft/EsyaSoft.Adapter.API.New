using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using EsyaSoft.Adapter.Domain.Models.ManualMeterRead;
using EsyaSoft.Adapter.Domain.Models.MeterReadCancelOUT;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class ManualMeterReadsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<ManualMeterReadsController> _logger;

        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        public string xmlSOAP = string.Empty;
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string SectionHeader = "EndPoints";

        public ManualMeterReadsController(ILogger<ManualMeterReadsController> logger, AdapterContext dbContext,
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

        #region Bulk - SAP OUT - Manual Meter Read

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForBulk = ServiceEnum.ManualMeterReadsCreateBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.ManualMeterReadsCreateBulk;

        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultBulkCreateRequest_OutService")]
        [Consumes("text/xml")]
        public void ManualMeterReadBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:ManualMeterReadBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultBulkCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:ManualMeterReadBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ManualMeterReadBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }


                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:ManualMeterReadBulk >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:ManualMeterReadBulk: {ex.Message}");
                    _logger.LogCritical("Debug:ManualMeterReadBulk >> Error occurred: " + ex.Message.ToString());
              }
            }
        }

        #endregion


        #region SINGLE - SAP OUT - Manual Meter Read

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.ManualMeterReadsCreateSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.ManualMeterReadsCreateSingle;

        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultCreateRequest_OutService")]
        [Consumes("text/xml")]

        public void ManualMeterReadSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:ManualMeterReadSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:ManualMeterReadSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ManualMeterReadSingle: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:ManualMeterReadSingle >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:ManualMeterReadSingle: {ex.Message}");
                    _logger.LogCritical("Debug:ManualMeterReadSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion



        #region SAP IN 
        #region SAP 46 IN - Manual Meter Reads Create Confirmation - BULK

        private readonly string keyForManualMeterReadsCreateConfirmationBulk = ServiceEnum.ManualMeterReadsCreateConfirmationBulk.ToString();
        private readonly int ServiceEnumValueForManualMeterReadsCreateConfirmationBulk = (int)ServiceEnum.ManualMeterReadsCreateConfirmationBulk;

        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultBulkCreateConfirmation_In")]
        [Consumes("text/xml")]
        public string ManualMeterReadsCreateConfirmationBULK([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : ManualMeterReadsCreateConfirmationBULK >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultBulkCreateConfirmation_In");
            SmartMeterMeterReadingDocumentERPResultBulkCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPResultBulkCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }


            string? url = _configuration.GetSection(SectionHeader)[keyForManualMeterReadsCreateConfirmationBulk];
            string result = string.Empty;


            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End

            SmartMeterMeterReadingDocumentERPResultBulkCreateConfirmation model = new();

            #region messageheader

            ManualMeterReads_BulkCreateConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPResultBulkCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPResultBulkCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument

            List<SmartMeterMeterReadingDocumentERPResultCreateConfirmationMessage> ObjLst = new();

            ManualMeterReads_BulkCreateConfirmationMessageHeader MessageHeader = null;

            ManualMeterReads_MeterReadingDocumentConfirmation MeterReadingDocument = null;

            ManualMeterReads_BulkConfirmationLog ChildLog = null;
            ManualMeterReads_BulkConfirmationItem ChildLogItem = null;

            foreach (var objReading in obj.SmartMeterMeterReadingDocumentERPResultBulkCreateRequest.SmartMeterMeterReadingDocumentERPResultCreateRequestMessage)
            {
                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = objReading.MessageHeader.UUID;
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;
                #endregion

                #region MeterReadingDocument
                MeterReadingDocument = new();

                MeterReadingDocument.ID = objReading.MeterReadingDocument.ID;

                #endregion

                #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB

                ChildLog = new();

                ChildLog.BusinessDocumentProcessingResultCode = 3;
                ChildLog.MaximumLogItemSeverityCode = 1;

                ChildLogItem = new();

                ChildLogItem.Note = "Request already processed";
                ChildLogItem.TypeID = "00000";
                ChildLogItem.SeverityCode = 1;

                ChildLog.Item = ChildLogItem;



                #endregion

                ObjLst.Add(new SmartMeterMeterReadingDocumentERPResultCreateConfirmationMessage()
                {
                    MessageHeader = MessageHeader,
                    MeterReadingDocument = MeterReadingDocument,
                    Log = ChildLog
                });

            }

            #endregion

            #region Log
            ManualMeterReads_BulkConfirmationLog rootLog = new();

            rootLog.BusinessDocumentProcessingResultCode = 3;
            rootLog.MaximumLogItemSeverityCode = 1;

            ManualMeterReads_BulkConfirmationItem rootLogItem = new();
            rootLogItem.TypeID = "0000";
            rootLogItem.SeverityCode = 1;
            rootLogItem.Note = "Request Processed Successfully";

            rootLog.Item = rootLogItem;

            #endregion

            model.MessageHeader = rootHeader;
            model.LstManualMeterReadBulkConfirmationMessage = ObjLst;
            model.Log = rootLog;

            _logger.LogInformation("Debug >> Completed Creating the Model");

            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
            };
            string xml = string.Empty;
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                var serializer = new XmlSerializer(model.GetType());
                serializer.Serialize(writer, model, emptyNamespaces);
                xml = stream.ToString();
            }
            EnrichMeterReadCancellationConfirmationXML(ref xml);
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForManualMeterReadsCreateConfirmationBulk, ServiceEnumValueForManualMeterReadsCreateConfirmationBulk);
                //Console.WriteLine(result);

                Console.WriteLine("SAP IN POC - Result Returned : " + result);
                _logger.LogInformation("Debug >>SAP IN POC - Result Returned : " + result);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                result = ex.Message;
                _logger.LogCritical("Debug >> Error occurred in calling PostSOAPRequestAsync for IN service: " + ex.Message.ToString());

            }

            return result;
        }
        private void EnrichMeterReadCancellationConfirmationXML(ref string xml)
        {
            xml = xml.Replace("<LstManualMeterReadBulkConfirmationMessage>", "");
            xml = xml.Replace("</LstManualMeterReadBulkConfirmationMessage>", "");
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPResultBulkCreateConfirmation", "glob:SmartMeterMeterReadingDocumentERPResultBulkCreateConfirmation");
        }


        #endregion

        #region SAP 47 IN - Manual Meter Reads Create Confirmation - SINGLE
        private readonly string keyForManualMeterReadsCreateConfirmationSingle = ServiceEnum.ManualMeterReadsCreateConfirmationSingle.ToString();
        private readonly int ServiceEnumValueForManualMeterReadsCreateConfirmationSingle = (int)ServiceEnum.ManualMeterReadsCreateConfirmationSingle;

        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultCreateConfirmation_In")]
        [Consumes("text/xml")]
        public string ManualMeterReadsCreateConfirmationSINGLE([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : ManualMeterReadsCreateConfirmationSINGLE >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPResultCreateConfirmation_In");
            SmartMeterMeterReadingDocumentERPResultCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPResultCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }


            string? url = _configuration.GetSection(SectionHeader)[keyForManualMeterReadsCreateConfirmationSingle];
            string result = string.Empty;


            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End


            SmartMeterMeterReadingDocumentERPResultCreateConfirmation model = new();

            #region messageheader

            ManualMeterReads_BulkCreateConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPResultCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPResultCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region MeterDocument
            ManualMeterReads_MeterReadingDocumentConfirmation MeterReadingDocument = new();

            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPResultCreateRequest.MeterReadingDocument.ID;

            #endregion

            #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB
            ManualMeterReads_BulkConfirmationLog ChildLog = new();

            ChildLog.BusinessDocumentProcessingResultCode = 3;
            ChildLog.MaximumLogItemSeverityCode = 1;

            ManualMeterReads_BulkConfirmationItem ChildLogItem = new();
            ChildLogItem.Note = "Request already processed";
            ChildLogItem.TypeID = "00000";
            ChildLogItem.SeverityCode = 1;

            ChildLog.Item = ChildLogItem;

            #endregion

            model.MessageHeader = rootHeader;
            model.MeterReadingDocument = MeterReadingDocument;
            model.Log = ChildLog;

            _logger.LogInformation("Debug >> Completed Creating the Model");

            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
            };
            string xml = string.Empty;
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                var serializer = new XmlSerializer(model.GetType());
                serializer.Serialize(writer, model, emptyNamespaces);
                xml = stream.ToString();
            }
            //EnrichMeterReadCancellationConfirmationXML(ref xml);
            //Change XML Namespace
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPResultCreateConfirmation", "glob:SmartMeterMeterReadingDocumentERPResultCreateConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;

            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForManualMeterReadsCreateConfirmationSingle, ServiceEnumValueForManualMeterReadsCreateConfirmationSingle);
                //Console.WriteLine(result);

                Console.WriteLine("SAP IN POC - Result Returned : " + result);
                _logger.LogInformation("Debug >>SAP IN POC - Result Returned : " + result);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                result = ex.Message;
                _logger.LogCritical("Debug >> Error occurred in calling PostSOAPRequestAsync for IN service: " + ex.Message.ToString());

            }

            return result;

        }
        #endregion

        #endregion

        #region private Methods
        private string PostSOAPRequestAsync(string url, string text, string key, int ServiceEnumValue)
        {
            _logger.LogInformation("Debug >> SAP IN POC - Within Post Asnyc Method ...");

            Console.WriteLine("SAP IN POC - Within Post Asnyc Method ...");
            string returnVal = string.Empty;
            string? AuthCred = _configuration.GetSection("SAPCredential")["BasicAuthKey"];

            long IDval = 0;
            bool RetStatusDB = false;
            bool RetStatus = false;
            // SaveTransaction to DB - Initiation
            //IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);

            IDval = ContextRepository.SaveDBTransaction(_dbContext, text, key, ServiceEnumValue);
            if (IDval > 0)
            {
                RetStatusDB = true;
            }
            _logger.LogInformation("Debug >> Data Saved into DB for IN Service with Initial Value 0 ...");

            using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                //request.Headers.Add("SOAPAction", "");
                //request.Headers.Add("Authorization", "Basic dG1waV9kZXY6Z3RkQDEyMw==");
                request.Headers.Add("Authorization", AuthCred);
                request.Content = content;
                using (HttpResponseMessage response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    Console.WriteLine("SAP IN POC - Connection with Remote Host established...");
                    _logger.LogInformation("Debug >> SAP IN POC - Connection with Remote Host established...");


                    var res = response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
                    if (res.StatusCode.ToString() == "OK")
                    {
                        RetStatus = true;
                        returnVal = res.StatusCode.ToString() + " : Success Message StatusCode - Returned from API";
                        _logger.LogInformation("Debug >> SAP IN POC -Success Message StatusCode - Returned from API.." + res.StatusCode.ToString());

                        //Console.WriteLine("SAP IN POC - Service returned with OK StatusCode ...");
                    }
                    else
                    {
                        RetStatus = false;
                        returnVal = res.StatusCode.ToString() + " : Failure Message StatusCode - Returned from API";
                        _logger.LogInformation("Debug >> SAP IN POC - Failure Message StatusCode - Returned from API.." + res.StatusCode.ToString());

                    }
                }
            }

            //Update Success/Failure
            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatusDB, key, ServiceEnumValue);
            _logger.LogInformation("Debug >> Data Updated into DB for IN Service with Initial Value 1 ...");


            return returnVal;
        }

        #endregion
    }
}
