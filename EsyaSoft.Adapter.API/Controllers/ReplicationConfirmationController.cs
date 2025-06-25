using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models.RegisterChange.Single;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;
using EsyaSoft.Adapter.API.DataProcessor;
using System.Text;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class ReplicationConfirmationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<ReplicationConfirmationController> _logger;

        private readonly string SectionHeader = "EndPoints";
        private readonly string key = "RegisterCreateConfirmation";



        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        public string xmlSOAP = string.Empty;
        private static readonly HttpClient httpClient = new HttpClient();

        //public WeatherForecastController(ILogger<WeatherForecastController> logger,
        //   AdapterContext dbContext)
        //{
        //    _logger = logger;
        //    _dbContext = dbContext;
        //}
        public ReplicationConfirmationController(ILogger<ReplicationConfirmationController> logger, AdapterContext dbContext,
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




        #region CONFIRMATION CALL FROM CREATE CONTROLLER
        private readonly string keyForBulkConfirmation = ServiceEnum.ReplicationDeviceBulkConfirmation.ToString();
        private readonly int ServiceEnumValueForBulkConfirmation = (int)ServiceEnum.ReplicationDeviceBulkConfirmation;
        public string SendReplicationConfirmationBulk(UtilitiesDeviceERPSmartMeterReplicationBulkRequestRoot ObjXML)
        {

            _logger.LogInformation("Debug : SendReplicationConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
            string? url = _configuration.GetSection(SectionHeader)[keyForBulkConfirmation];
            string resultSend = string.Empty;
            // EXIT Condition : Start
            if (ObjXML == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            // EXIT Condition : End

            GenericConfirmationBulk model = new();

            #region messageheader

            GenericConfirmationHeader MessageHeader = new GenericConfirmationHeader();
            MessageHeader.UUID = Guid.NewGuid();
            MessageHeader.ReferenceUUID = Guid.Parse(ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.MessageHeader.UUID); //Guid.NewGuid();
            MessageHeader.CreationDateTime = DateTime.Now;
            MessageHeader.RecipientBusinessSystemID = ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.MessageHeader.SenderBusinessSystemID; ; // "AEML";

            model.MessageHeader = MessageHeader;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of Child Object

            List<GenericConfirmationSingle> ObjLst = new();


            GenericConfirmationUtilitiesDevice UtilitiesDevice = null;
            GenericConfirmationLog Log = null;
            GenericItem Item = null;
            var allMessages = ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.UtilitiesDeviceERPSmartMeterReplicationRequestMessage;


            string JsonMsgList = string.Empty;
            if (!allMessages.ToString().Contains("["))
                JsonMsgList = "[" + allMessages.ToString() + "]";
            else
                JsonMsgList = allMessages.ToString();

            var objMsgList = JsonConvert.DeserializeObject<List<UtilitiesDeviceERPSmartMeterReplicationRequestMessage>>(JsonMsgList);

            foreach (var objReading in objMsgList)
            {

                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new GenericConfirmationHeader();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = Guid.Parse(objReading.MessageHeader.UUID);
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MessageHeader = MessageHeader;
                #endregion

                #region UtilitiesDevice
                UtilitiesDevice = new GenericConfirmationUtilitiesDevice();

                UtilitiesDevice.ID = objReading.UtilitiesDevice.ID;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
                #endregion

                #region Log
                Log = new GenericConfirmationLog();

                Log.BusinessDocumentProcessingResultCode = 3;
                Log.MaximumLogItemSeverityCode = 1;


                Item = new GenericItem();
                Item.TypeID = "00000";
                Item.SeverityCode = 1;
                Item.Note = "Request Processed Successfully";

                Log.Item = Item;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
                #endregion

                ObjLst.Add(new Domain.Models.GenericConfirmationSingle()
                {
                    MessageHeader = MessageHeader,
                    Log = Log,
                    UtilitiesDevice = UtilitiesDevice
                });

                //LstMeterReadingDocumentERPResultCreateConfirmationMessage.Add(MeterReadingDocumentERPResultCreateConfirmationMessage);
            }
            #endregion

            #region Log
            GenericConfirmationLog rootLog = new();

            rootLog.BusinessDocumentProcessingResultCode = 3;
            rootLog.MaximumLogItemSeverityCode = 1;

            GenericItem rootLogItem = new();
            rootLogItem.TypeID = "0000";
            rootLogItem.SeverityCode = 1;
            rootLogItem.Note = "Request Processed Successfully";

            rootLog.Item = rootLogItem;

            //model.Log = rootLog;
            #endregion

            model.ChildMessage = ObjLst;
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
            EnrichXML(ref xml);
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                resultSend = PostSOAPRequestAsync(url, xmlSOAP, keyForBulkConfirmation, ServiceEnumValueForBulkConfirmation);
                //Console.WriteLine(result);

                Console.WriteLine("SAP IN POC - Result Returned : " + resultSend);
                _logger.LogInformation("Debug >>SAP IN POC - Result Returned : " + resultSend);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                resultSend = ex.Message;
                _logger.LogCritical("Debug >> Error occurred in calling PostSOAPRequestAsync for IN service: " + ex.Message.ToString());

            }
            return resultSend;
        }


        private void EnrichXML(ref string xml)
        {
            //xml = xml.Replace("SmartMeterMeterReadingDocumentERPBulkCreateRequest ", "glob:SmartMeterMeterReadingDocumentERPBulkCreateRequest ");
            xml = xml.Replace("<ChildMessage>", "");
            xml = xml.Replace("</ChildMessage>", "");
            //xml.Remove(xml.IndexOf("<SmartMeterMeterReadingDocumentERPCreateConfirmationMessage>"), 60);
            //xml.Remove(xml.LastIndexOf("</SmartMeterMeterReadingDocumentERPCreateConfirmationMessage>"), 61);
            xml = xml.Replace("GenericConfirmationBulk", "glob:UtilitiesDeviceERPSmartMeterReplicationBulkConfirmation");
            xml = xml.Replace("GenericConfirmationSingle", "UtilitiesDeviceERPSmartMeterReplicationConfirmationMessage");
        }
        #endregion

        #region private Methods
        private string PostSOAPRequestAsync(string url, string text, string keyForBulkResult, int ServiceEnumValueForBulkResult)
        {
            _logger.LogInformation("Debug >> SAP IN POC - Within Post Asnyc Method ...");

            Console.WriteLine("SAP IN POC - Within Post Asnyc Method ...");
            string? AuthCred = _configuration.GetSection("SAPCredential")["BasicAuthKey"];
            string returnVal = string.Empty;
            long IDval = 0;
            bool RetStatusDB = false;
            bool RetStatus = false;
            // SaveTransaction to DB - Initiation
            IDval = ContextRepository.SaveDBTransaction(_dbContext, text, keyForBulkResult, ServiceEnumValueForBulkResult);
            if (IDval > 0)
            {
                RetStatusDB = true;
            }
            _logger.LogInformation("Debug >> Data Saved into DB for IN Service with Initial Value 0 ...");


            using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
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
            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatusDB, keyForBulkResult, ServiceEnumValueForBulkResult);
            _logger.LogInformation("Debug >> Data Updated into DB for IN Service with Initial Value 1 ...");


            return returnVal;
        }

        #endregion
    }
}
