using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace EsyaSoft.Adapter.API.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class DeviceCreateConfirmationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<DeviceCreateConfirmationController> _logger;

        private readonly string SectionHeader = "EndPoints";
        //private readonly string keyForSingleConfirmation = "DeviceCreateConfirmationSingle";
        private readonly string key = "DeviceCreateConfirmation";



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
                    <UtilitiesDeviceERPSmartMeterCreateConfirmation>
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
                        </UtilitiesDeviceERPSmartMeterCreateConfirmation>
                    </soapenv:Body>
                    </soapenv:Envelope>";
        
        public DeviceCreateConfirmationController(ILogger<DeviceCreateConfirmationController> logger, AdapterContext dbContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        //public IActionResult Index()
        //{
        //    UtilitiesDeviceERPSmartMeterCreateConfirmationModel model = new UtilitiesDeviceERPSmartMeterCreateConfirmationModel();

        //    MessageHeader messageHeader = new MessageHeader();
        //    messageHeader.UUID = Guid.NewGuid();
        //    messageHeader.ReferenceUUID = Guid.NewGuid();
        //    messageHeader.CreationDateTime = DateTime.Now;
        //    messageHeader.RecipientBusinessSystemID = "AEML";

        //    UtilitiesDevice utilitiesDevice  = new UtilitiesDevice();
        //    utilitiesDevice.ID = "46126725";

        //    Log log = new Log();

        //    log.BusinessDocumentProcessingResultCode = 3;
        //    log.MaximumLogItemSeverityCode = 1;
        //    log.Item = new Item(); ;

        //    log.Item.TypeID = "0000";
        //    log.Item.SeverityCode = 1;
        //    log.Item.Note = "Request Processed Successfully";

        //    model.messageHeader = messageHeader;
        //    model.utilitiesDevice = utilitiesDevice;
        //    model.log = log;


        //    return View();
        //}

        //[HttpPost("DeviceCreateConfirmation")]
        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterCreateConfirmation_InService_Old")]
        public string Get()
        {
            #region commented
            //UtilitiesDeviceERPSmartMeterCreateConfirmationModel model = new UtilitiesDeviceERPSmartMeterCreateConfirmationModel();

            //MessageHeader messageHeader = new MessageHeader();
            //messageHeader.UUID = Guid.NewGuid();
            //messageHeader.ReferenceUUID = Guid.NewGuid();
            //messageHeader.CreationDateTime = DateTime.Now;
            //messageHeader.RecipientBusinessSystemID = "AEML";

            //UtilitiesDevice utilitiesDevice = new UtilitiesDevice();
            //utilitiesDevice.ID = "46126725";

            //Log log = new Log();

            //log.BusinessDocumentProcessingResultCode = 3;
            //log.MaximumLogItemSeverityCode = 1;
            //log.Item = new Item(); ;

            //log.Item.TypeID = "0000";
            //log.Item.SeverityCode = 1;
            //log.Item.Note = "Request Processed Successfully";

            //model.messageHeader = messageHeader;
            //model.utilitiesDevice = utilitiesDevice;
            //model.log = log;


            ////XmlSerializer ser = new XmlSerializer(typeof(UtilitiesDeviceERPSmartMeterCreateConfirmationModel))

            ////var strObj = ser.Serialize(model);

            //var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            //var settings = new XmlWriterSettings()
            //{
            //    OmitXmlDeclaration = true,
            //};

            //using (var stream = new StringWriter())
            //using (var writer = XmlWriter.Create(stream, settings))
            //{
            //    var serializer = new XmlSerializer(model.GetType());
            //    serializer.Serialize(writer, model, emptyNamespaces);
            //    string xml = stream.ToString();

            //    SoapFormatter spFrmt = new SoapFormatter();

            //    //spFrmt.Serialize(xml,)

            //}
            #endregion
            
            string? url =  _configuration.GetSection(SectionHeader)[key];
            string result = string.Empty;
            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");

                result =  PostSOAPRequestAsync(url, xmlSOAP);
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
        private string PostSOAPRequestAsync(string url, string text)
        {
            _logger.LogInformation("Debug >> SAP IN POC - Within Post Asnyc Method ...");

            Console.WriteLine("SAP IN POC - Within Post Asnyc Method ..."); 
            string returnVal = string.Empty;
            string? AuthCred = _configuration.GetSection("SAPCredential")["BasicAuthKey"];

            long IDval = 0;
            Boolean RetStatus = false;
            // SaveTransaction to DB - Initiation
            IDval = SaveDBTransaction(text);
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
                        _logger.LogInformation("Debug >> SAP IN POC -Success Message StatusCode - Returned from API.."+ res.StatusCode.ToString());

                        //Console.WriteLine("SAP IN POC - Service returned with OK StatusCode ...");
                    }
                    else { 
                        RetStatus = false;
                        returnVal = res.StatusCode.ToString() + " : Failure Message StatusCode - Returned from API";
                        _logger.LogInformation("Debug >> SAP IN POC - Failure Message StatusCode - Returned from API.." + res.StatusCode.ToString());

                    }
                }
            }

            //Update Success/Failure
            updateDBTransaction(IDval, RetStatus);
            _logger.LogInformation("Debug >> Data Updated into DB for IN Service with Initial Value 1 ...");


            return returnVal;
        }

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
                            entity.Remark = "IN call for - " + key + " Completed.";

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
                    Remark = "IN call for - " + key + " intiated.",
                    ServiceId = 2,
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

        //[HttpPost("DeviceInputs")]
        [HttpPost("SAPAdapterWS/UtilitiesDeviceERPSmartMeterCreateConfirmation_In")]

        public string GetThroughInputs(Guid UUID, Guid ReferenceUUID, string RecipientBusinessSystemID, string DeviceId, int BusinessDocumentProcessingResultCode,
            int MaximumLogItemSeverityCode, string TypeID, int SeverityCode, string Note)
        {

            _logger.LogInformation("Debug >> Reached the controller - SAPAdapterWS/UtilitiesDeviceERPSmartMeterCreateConfirmation_InService");

            //UtilitiesDeviceERPSmartMeterCreateConfirmationModel model = new UtilitiesDeviceERPSmartMeterCreateConfirmationModel();
            UtilitiesDeviceERPSmartMeterCreateConfirmation model = new UtilitiesDeviceERPSmartMeterCreateConfirmation();
            
           ConfirmationHeader MessageHeader = new ConfirmationHeader();
           MessageHeader.UUID = UUID; // Guid.NewGuid();
           MessageHeader.ReferenceUUID = ReferenceUUID; //Guid.NewGuid();
           MessageHeader.CreationDateTime = DateTime.Now;
           MessageHeader.RecipientBusinessSystemID = RecipientBusinessSystemID; // "AEML";

            ConfirmationUtilitiesDevice UtilitiesDevice = new ConfirmationUtilitiesDevice();
            //utilitiesDevice.ID = "46126725";
            UtilitiesDevice.ID = DeviceId;

            ConfirmationLog log = new ConfirmationLog();

            //log.BusinessDocumentProcessingResultCode = 3;
            //log.MaximumLogItemSeverityCode = 1;
            //log.Item = new Item();

            log.BusinessDocumentProcessingResultCode = BusinessDocumentProcessingResultCode;
            log.MaximumLogItemSeverityCode = MaximumLogItemSeverityCode;
            log.Item = new Item();

            //log.Item.TypeID = "0000";
            //log.Item.SeverityCode = 1;
            //log.Item.Note = "Request Processed Successfully";

            log.Item.TypeID = TypeID;
            log.Item.SeverityCode = SeverityCode;
            log.Item.Note = Note;


            model.MessageHeader = MessageHeader;
            model.UtilitiesDevice = UtilitiesDevice;
            model.Log = log;

            _logger.LogInformation("Debug >> Completed Creating the Model");

            //XmlSerializer ser = new XmlSerializer(typeof(UtilitiesDeviceERPSmartMeterCreateConfirmationModel))

            //var strObj = ser.Serialize(model);

            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("glob", ""http://sap.com/xi/SAPGlobal20/Global\"");
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
            xml = xml.Replace("UtilitiesDeviceERPSmartMeterCreateConfirmation", "glob:UtilitiesDeviceERPSmartMeterCreateConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            /* Output XML
             
            <UtilitiesDeviceERPSmartMeterCreateConfirmationModel>
	            <messageHeader>
		            <UUID>15316ff0-d304-1eee-a9ab-f099e4508f04</UUID>
		            <ReferenceUUID>15216ff0-d304-1eee-a9ab-f099e4508f04</ReferenceUUID>
		            <CreationDateTime>2024-04-19T10:15:33.3278689+05:30</CreationDateTime>
		            <RecipientBusinessSystemID>ESFT</RecipientBusinessSystemID>
	            </messageHeader>
	            <utilitiesDevice>
		            <ID>46126725</ID>
	            </utilitiesDevice>
	            <log>
		            <BusinessDocumentProcessingResultCode>3</BusinessDocumentProcessingResultCode>
		            <MaximumLogItemSeverityCode>1</MaximumLogItemSeverityCode>
		            <Item>
			            <TypeID>0000</TypeID>
			            <SeverityCode>1</SeverityCode>
			            <Note>Request Processed Successfully</Note>
		            </Item>
	            </log>
            </UtilitiesDeviceERPSmartMeterCreateConfirmationModel>

            */

            string? url = _configuration.GetSection(SectionHeader)[key];
            string result = string.Empty;
            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP);
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
            //return "Hello";
        }


        #region CONFIRMATION CALL FROM CREATE CONTROLLER
        private readonly string keyForSingleConfirmation = ServiceEnum.DeviceCreateConfirmationSingle.ToString();
        private readonly int ServiceEnumValueForSingleConfirmation = (int)ServiceEnum.DeviceCreateConfirmationSingle;
        public string SendMeterCreateConfirmationSingle(UtilitiesDeviceERPSmartMeterCreateRequestRoot obj)
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

            #region REQUEST OBJECT CONVERSION TO CONFIRMATION OBJECT
            UtilitiesDeviceERPSmartMeterCreateConfirmation model = new UtilitiesDeviceERPSmartMeterCreateConfirmation();

            ConfirmationHeader MessageHeader = new ConfirmationHeader();
            MessageHeader.UUID = Guid.NewGuid();
            MessageHeader.ReferenceUUID = obj.UtilitiesDeviceERPSmartMeterCreateRequest.MessageHeader.UUID; //Guid.NewGuid();
            MessageHeader.CreationDateTime = DateTime.Now;
            MessageHeader.RecipientBusinessSystemID = obj.UtilitiesDeviceERPSmartMeterCreateRequest.MessageHeader.SenderBusinessSystemID; ; // "AEML";

            ConfirmationUtilitiesDevice UtilitiesDevice = new ConfirmationUtilitiesDevice();
            UtilitiesDevice.ID = obj.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.ID;

            #region Log
            ConfirmationLog log = new ConfirmationLog();

            log.BusinessDocumentProcessingResultCode = 3;
            log.MaximumLogItemSeverityCode = 1;
            log.Item = new Item();
            
            log.Item.TypeID = "00000";
            log.Item.SeverityCode = 1;
            log.Item.Note = "Request Processed Successfully";

            model.MessageHeader = MessageHeader;
            model.UtilitiesDevice = UtilitiesDevice;
            model.Log = log;

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
            xml = xml.Replace("UtilitiesDeviceERPSmartMeterCreateConfirmation", "glob:UtilitiesDeviceERPSmartMeterCreateConfirmation");
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
            
        }
        #endregion

    }
}
