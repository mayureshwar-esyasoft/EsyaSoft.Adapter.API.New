using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;
using System.Xml;
using EsyaSoft.Adapter.API.DataProcessor;
using System.Text;
using Newtonsoft.Json;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class DeviceChangeConfirmationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<DeviceChangeConfirmationController> _logger;

        private readonly string SectionHeader = "EndPoints";
        
        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv =""http://schemas.xmlsoap.org/soap/envelope/"" 
                    xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                    <soapenv:Header/>
                    <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        private static readonly HttpClient httpClient = new HttpClient();
        public string xmlSOAP = @" < soapenv:Envelope xmlns:soapenv =""http://schemas.xmlsoap.org/soap/envelope/"" 
                    xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                    <soapenv:Header/>
                    <soapenv:Body> 
                    <UtilitiesDeviceERPSmartMeterChangeConfirmation>
                            <MessageHeader>
                                    <UUID>15316ff0-d304-1eee-a9ab-f099e4508f04</UUID>
                                    <ReferenceUUID>25316ff0-d304-1eee-a9ab-f099e4508f02</ReferenceUUID>
                                    <CreationDateTime></CreationDateTime>
                                    <RecipientBusinessSystemID>AEML</RecipientBusinessSystemID>
                            </MessageHeader>
                            <UtilitiesDevice>
                                    <ID>46126725</ID>
                            </UtilitiesDevice>
                            <Log> 
                                <BusinessDocumentProcessingResultCode>3</BusinessDocumentProcessingResultCode>
                                <MaximumLogItemSeverityCode>1</MaximumLogItemSeverityCode>
                                <Item>
                                    <TypeID>00000</TypeID>
                                    <SeverityCode>1</SeverityCode>
                                    <Note>Request Processed Successfully</Note>
                                </Item>
                            </Log>
                        </UtilitiesDeviceERPSmartMeterChangeConfirmation>
                    </soapenv:Body>
                    </soapenv:Envelope>";

        public DeviceChangeConfirmationController(ILogger<DeviceChangeConfirmationController> logger, AdapterContext dbContext,
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

        #region CONFIRMATION CALL FROM CREATE CONTROLLER - SINGLE 
        private readonly string keyForSingleConfirmation = ServiceEnum.DeviceChangeConfirmationSingle.ToString();
        private readonly int ServiceEnumValueForSingleConfirmation = (int)ServiceEnum.DeviceChangeConfirmationSingle;
        public string SendMeterChangeConfirmationSingle(UtilitiesDeviceERPSmartMeterChangeRequestRoot obj)
        {
            _logger.LogInformation("Debug : SendMeterCreateConfirmationSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleConfirmation];
            string resultSend = string.Empty;
            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            /*
             * public MeterChangeConfirmationMessageHeaderSingle MessageHeader { get; set; }
        public MeterChangeConfirmationUtilitiesDeviceSingle UtilitiesDevice { get; set; }
        public MeterChangeConfirmationLogSingle Log { get; set; }
             */
            #region REQUEST OBJECT CONVERSION TO CONFIRMATION OBJECT
            UtilitiesDeviceERPSmartMeterChangeConfirmation model = new UtilitiesDeviceERPSmartMeterChangeConfirmation();

            MeterChangeConfirmationMessageHeaderSingle MessageHeader = new MeterChangeConfirmationMessageHeaderSingle();
            MessageHeader.UUID = Guid.NewGuid();
            MessageHeader.ReferenceUUID = obj.UtilitiesDeviceERPSmartMeterChangeRequest.MessageHeader.UUID; //Guid.NewGuid();
            MessageHeader.CreationDateTime = DateTime.Now;
            MessageHeader.RecipientBusinessSystemID = obj.UtilitiesDeviceERPSmartMeterChangeRequest.MessageHeader.SenderBusinessSystemID; ; // "AEML";

            MeterChangeConfirmationUtilitiesDeviceSingle UtilitiesDevice = new MeterChangeConfirmationUtilitiesDeviceSingle();
            UtilitiesDevice.ID = obj.UtilitiesDeviceERPSmartMeterChangeRequest.UtilitiesDevice.ID;

            #region Log
            MeterChangeConfirmationLogSingle log = new MeterChangeConfirmationLogSingle();

            log.BusinessDocumentProcessingResultCode = 3;
            log.MaximumLogItemSeverityCode = 1;
            log.Item = new MeterChangeConfirmationLogItemSingle();

            log.Item.TypeID = "00000";
            log.Item.SeverityCode = 1;
            log.Item.Note = "Request Processed Successfully";

            model.MessageHeader = MessageHeader;
            model.UtilitiesDevice = UtilitiesDevice;
            model.Log = log;

            //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
            #endregion

            #endregion
            //==============


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
            xml = xml.Replace("UtilitiesDeviceERPSmartMeterChangeConfirmation", "glob:UtilitiesDeviceERPSmartMeterChangeConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                resultSend = PostSOAPRequestAsync(url, xmlSOAP, keyForSingleConfirmation, ServiceEnumValueForSingleConfirmation);
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
            //XmlSerializer ser = new XmlSerializer(typeof(UtilitiesDeviceERPSmartMeterCreateConfirmationModel))

        }
        #endregion

        #region CONFIRMATION CALL FROM CREATE CONTROLLER - BULK 
        private readonly string keyForBulkConfirmation = ServiceEnum.DeviceChangeConfirmationBulk.ToString();
        private readonly int ServiceEnumValueForBulkConfirmation = (int)ServiceEnum.DeviceChangeConfirmationBulk;
        public string SendMeterChangeConfirmationBULK(UtilitiesDeviceERPSmartMeterBulkChangeRequestRoot ObjXML)
        {
            _logger.LogInformation("Debug : SendMeterCreateConfirmationSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");
            string? url = _configuration.GetSection(SectionHeader)[keyForBulkConfirmation];
            string resultSend = string.Empty;
            // EXIT Condition : Start
            if (ObjXML == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            //==============
            GenericConfirmationBulk model = new();

            //--->>> Root messageheader
            GenericConfirmationHeader MessageHeader = new GenericConfirmationHeader();
            MessageHeader.UUID = Guid.NewGuid();
            MessageHeader.ReferenceUUID = (ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.MessageHeader.UUID); //Guid.NewGuid();
            MessageHeader.CreationDateTime = DateTime.Now;
            MessageHeader.RecipientBusinessSystemID = ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.MessageHeader.SenderBusinessSystemID; ; // "AEML";

            model.MessageHeader = MessageHeader;
            #region LIST of Child Object
            List<GenericConfirmationSingle> ObjLst = new();

            GenericConfirmationUtilitiesDevice UtilitiesDevice = null;
            GenericConfirmationLog Log = null;
            GenericItem Item = null;
            var allMessages = ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.UtilitiesDeviceERPSmartMeterChangeRequestMessage;

            foreach (var objReading in allMessages)
            {
                #region messageHeader
                MessageHeader = new GenericConfirmationHeader();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = objReading.MessageHeader.UUID;
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;

                #endregion

                #region UtilitiesDevice
                UtilitiesDevice = new GenericConfirmationUtilitiesDevice();

                UtilitiesDevice.ID = objReading.UtilitiesDevice.ID;

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

                #endregion

                ObjLst.Add(new Domain.Models.GenericConfirmationSingle()
                {
                    MessageHeader = MessageHeader,
                    Log = Log,
                    UtilitiesDevice = UtilitiesDevice
                });
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
            //xml = xml.Replace("UtilitiesDeviceERPSmartMeterChangeConfirmation", "glob:UtilitiesDeviceERPSmartMeterChangeConfirmation");
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
            //XmlSerializer ser = new XmlSerializer(typeof(UtilitiesDeviceERPSmartMeterCreateConfirmationModel))

        }

        private void EnrichXML(ref string xml)
        {
            //xml = xml.Replace("SmartMeterMeterReadingDocumentERPBulkCreateRequest ", "glob:SmartMeterMeterReadingDocumentERPBulkCreateRequest ");
            xml = xml.Replace("<ChildMessage>", "");
            xml = xml.Replace("</ChildMessage>", "");
            xml = xml.Replace("GenericConfirmationBulk", "glob:UtilitiesDeviceERPSmartMeterBulkChangeConfirmation");
            xml = xml.Replace("GenericConfirmationSingle", "UtilitiesDeviceERPSmartMeterChangeRequestMessage");
        }
        #endregion

        #region private methods
        private string PostSOAPRequestAsync(string url, string text, string key, int ServiceEnumValue)
        {
            _logger.LogInformation("Debug >> SAP IN POC - Within Post Asnyc Method ...");

            Console.WriteLine("SAP IN POC - Within Post Asnyc Method ...");
            string? AuthCred = _configuration.GetSection("SAPCredential")["BasicAuthKey"];
            string returnVal = string.Empty;
            long IDval = 0;
            bool RetStatusDB = false;
            bool RetStatus = false;
            // SaveTransaction to DB - Initiation
            IDval = ContextRepository.SaveDBTransaction(_dbContext, text, key, ServiceEnumValue);
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
            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatusDB, key, ServiceEnumValue);
            _logger.LogInformation("Debug >> Data Updated into DB for IN Service with Initial Value 1 ...");

            return returnVal;
        }
        #endregion

    }
}
