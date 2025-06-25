using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using Serilog;
using System;
using System.Text;
using ExtensionMethod;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Security.Policy;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;

namespace EsyaSoft.Adapter.API.Controllers
{
    //SAP IN
    public class MeterReadRequestConfirmationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<MeterReadRequestConfirmationController> _logger;

        private static string urlSTATIC = string.Empty;

        private static string xmlSOAPPrefixForStaticMethods = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private static string xmlSOAPSuffixForStaticMethods = "</soapenv:Body></soapenv:Envelope>";
        private static string xmlSOAPSTATIC = string.Empty;

        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        public string xmlSOAP = string.Empty;
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly string SectionHeader = "EndPoints";

        public MeterReadRequestConfirmationController(ILogger<MeterReadRequestConfirmationController> logger, AdapterContext dbContext,
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

        #region BULK

        #region  SAP 35 IN - REQUEST Confirmation Object

        private readonly string keyForBulkConfirmation = ServiceEnum.MeterReadBulkConfirmation.ToString();
        private readonly int ServiceEnumValueForBulkConfirmation = (int)ServiceEnum.MeterReadBulkConfirmation;


        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In")]
        [Consumes("text/xml")]
        public string MeterReadRequestConfirmationBulk([FromBody] XElement Data)
        {

            _logger.LogInformation("Debug : MeterReadRequestConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
            SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            string? url = _configuration.GetSection(SectionHeader)[keyForBulkConfirmation];
            string resultSend = string.Empty;

            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            // EXIT Condition : End

            SmartMeterMeterReadingDocumentERPBulkCreateConfirmation model = new();

            #region messageheader

            MeterCreateConfirmationBulkMessageHeader rootHeader = new MeterCreateConfirmationBulkMessageHeader();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = Guid.Parse(obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.UUID);
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument

            List<SmartMeterMeterReadingDocumentERPCreateConfirmationMessage> ObjLst
                = new();

            //ClsMeterReadingDocumentERPResultCreateConfirmationMessage MeterReadingDocumentERPResultCreateConfirmationMessage = null;

            MeterCreateConfirmationBulkMessageHeader MessageHeader = null;

            MeterReadingDocumentConfirmation MeterReadingDocument = null;

            MeterCreateBulkConfirmationLog Log = null;
            MeterCreateBulkConfirmationItem Item = null;

            foreach (var objReading in obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.SmartMeterMeterReadingDocumentERPCreateRequestMessage)
            {
                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new MeterCreateConfirmationBulkMessageHeader();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = Guid.Parse(objReading.MessageHeader.UUID);
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MessageHeader = MessageHeader;
                #endregion

                #region MeterReadingDocument
                MeterReadingDocument = new MeterReadingDocumentConfirmation();

                MeterReadingDocument.ID = objReading.MeterReadingDocument.ID;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
                #endregion

                #region Log
                Log = new MeterCreateBulkConfirmationLog();

                Log.BusinessDocumentProcessingResultCode = 3;
                Log.MaximumLogItemSeverityCode = 1;


                Item = new MeterCreateBulkConfirmationItem();
                Item.TypeID = "00000";
                Item.SeverityCode = 1;
                Item.Note = "Request Processed Successfully";

                Log.Item = Item;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
                #endregion

                ObjLst.Add(new Domain.Models.SmartMeterMeterReadingDocumentERPCreateConfirmationMessage()
                {
                    MessageHeader = MessageHeader,
                    Log = Log,
                    MeterReadingDocument = MeterReadingDocument
                });

                //LstMeterReadingDocumentERPResultCreateConfirmationMessage.Add(MeterReadingDocumentERPResultCreateConfirmationMessage);
            }
            #endregion

            #region Log
            MeterCreateBulkConfirmationLog rootLog = new MeterCreateBulkConfirmationLog();

            rootLog.BusinessDocumentProcessingResultCode = 3;
            rootLog.MaximumLogItemSeverityCode = 1;

            MeterCreateBulkConfirmationItem rootLogItem = new MeterCreateBulkConfirmationItem();
            rootLogItem.TypeID = "0000";
            rootLogItem.SeverityCode = 1;
            rootLogItem.Note = "Request Processed Successfully";

            rootLog.Item = rootLogItem;

            //model.Log = rootLog;
            #endregion

            model.MessageHeader = rootHeader;
            model.LstRegisterRequest = ObjLst;
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
            //return "Hello";
        }

        /// <summary>
        /// On The Fly Response for Each Request
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MeterReadRequestConfirmationBulk(SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj)
        {

            _logger.LogInformation("Debug : MeterReadRequestConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
            string? url = _configuration.GetSection(SectionHeader)[keyForBulkConfirmation];
            string resultSend = string.Empty;
            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            // EXIT Condition : End

            SmartMeterMeterReadingDocumentERPBulkCreateConfirmation model = new();

            #region messageheader

            MeterCreateConfirmationBulkMessageHeader rootHeader = new MeterCreateConfirmationBulkMessageHeader();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = Guid.Parse(obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.UUID);
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument

            List<SmartMeterMeterReadingDocumentERPCreateConfirmationMessage> ObjLst
                = new();

            //ClsMeterReadingDocumentERPResultCreateConfirmationMessage MeterReadingDocumentERPResultCreateConfirmationMessage = null;

            MeterCreateConfirmationBulkMessageHeader MessageHeader = null;

            MeterReadingDocumentConfirmation MeterReadingDocument = null;

            MeterCreateBulkConfirmationLog Log = null;
            MeterCreateBulkConfirmationItem Item = null;

            foreach (var objReading in obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.SmartMeterMeterReadingDocumentERPCreateRequestMessage)
            {
                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new MeterCreateConfirmationBulkMessageHeader();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = Guid.Parse(objReading.MessageHeader.UUID);
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MessageHeader = MessageHeader;
                #endregion

                #region MeterReadingDocument
                MeterReadingDocument = new MeterReadingDocumentConfirmation();

                MeterReadingDocument.ID = objReading.MeterReadingDocument.ID;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
                #endregion

                #region Log
                Log = new MeterCreateBulkConfirmationLog();

                Log.BusinessDocumentProcessingResultCode = 3;
                Log.MaximumLogItemSeverityCode = 1;


                Item = new MeterCreateBulkConfirmationItem();
                Item.TypeID = "00000";
                Item.SeverityCode = 1;
                Item.Note = "Request Processed Successfully";

                Log.Item = Item;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
                #endregion

                ObjLst.Add(new Domain.Models.SmartMeterMeterReadingDocumentERPCreateConfirmationMessage()
                {
                    MessageHeader = MessageHeader,
                    Log = Log,
                    MeterReadingDocument = MeterReadingDocument
                });

                //LstMeterReadingDocumentERPResultCreateConfirmationMessage.Add(MeterReadingDocumentERPResultCreateConfirmationMessage);
            }
            #endregion

            #region Log
            MeterCreateBulkConfirmationLog rootLog = new MeterCreateBulkConfirmationLog();

            rootLog.BusinessDocumentProcessingResultCode = 3;
            rootLog.MaximumLogItemSeverityCode = 1;

            MeterCreateBulkConfirmationItem rootLogItem = new MeterCreateBulkConfirmationItem();
            rootLogItem.TypeID = "0000";
            rootLogItem.SeverityCode = 1;
            rootLogItem.Note = "Request Processed Successfully";

            rootLog.Item = rootLogItem;

            //model.Log = rootLog;
            #endregion

            model.MessageHeader = rootHeader;
            model.LstRegisterRequest = ObjLst;
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
            xml = xml.Replace("<LstRegisterRequest>", "");
            xml = xml.Replace("</LstRegisterRequest>", "");
            //xml.Remove(xml.IndexOf("<SmartMeterMeterReadingDocumentERPCreateConfirmationMessage>"), 60);
            //xml.Remove(xml.LastIndexOf("</SmartMeterMeterReadingDocumentERPCreateConfirmationMessage>"), 61);
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPBulkCreateConfirmation", "glob:SmartMeterMeterReadingDocumentERPBulkCreateConfirmation");
        }

        #endregion

        #region SAP 37 BULK - IN - SENDING RESULT

        private readonly string keyForBulkResult = ServiceEnum.MeterReadBulkResult.ToString();
        private readonly int ServiceEnumValueForBulkResult = (int)ServiceEnum.MeterReadBulkResult;

        #region Parsing Result on the FLY
        //[HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateRequest_In")]
        //[Consumes("text/xml")]
        public string MeterReadBULKRESULTConfirmation([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : MeterReadRequestConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
            SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            Dictionary<string, decimal> staticReading = new();

            staticReading.Add("KWH_TOTAL", 2271.01m);
            staticReading.Add("MD_0622", 1.296m);
            staticReading.Add("KVARH_LAG", 439.24m);
            staticReading.Add("KWH18-22", 363.32m);
            staticReading.Add("MD_2206", 0.432m);
            staticReading.Add("KW_24_HRS", 1.286m);
            staticReading.Add("MD_24_HRS", 1.296m);
            staticReading.Add("KWH_0609", 246.98m);
            staticReading.Add("KWH_0912", 328.59m);
            staticReading.Add("KWH_1218", 700.32m);
            staticReading.Add("KWH_2206", 631.79m);
            staticReading.Add("KVARH_LEAD", 12.84m);

            string? url = _configuration.GetSection(SectionHeader)[keyForBulkResult];
            string result = string.Empty;


            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End

            MeterReadingDocumentERPResultBulkCreateRequest model = new();
            #region messageheader

            MeterReadResultConfirmationBulkMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = Guid.Parse(obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.UUID);
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;//.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK");

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument

            List<MeterReadingDocumentERPResultCreateRequestMessage> ObjLst
                = new();


            MeterReadResultConfirmationBulkMessageHeader MessageHeader = null;

            MeterReadingDocumentResult MeterReadingDocument = null;
            UtiltiesMeasurementTaskResult UtiltiesMeasurementTask = null;
            UtiltiesDeviceResult UtiltiesDevice = null;

            SentResult Result = null;

            foreach (var objReading in obj.SmartMeterMeterReadingDocumentERPBulkCreateRequest.SmartMeterMeterReadingDocumentERPCreateRequestMessage)
            {
                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = Guid.Parse(objReading.MessageHeader.UUID);
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;//.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK");

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MessageHeader = MessageHeader;
                #endregion


                #region MeterReadingDocument
                MeterReadingDocument = new();

                MeterReadingDocument.ID = objReading.MeterReadingDocument.ID;
                MeterReadingDocument.MeterReadingReasonCode = 1;
                MeterReadingDocument.ScheduledMeterReadingDate = DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");

                UtiltiesMeasurementTask = new();
                UtiltiesMeasurementTask.UtilitiesMeasurementTaskID = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesMeasurementTaskID.ToString();
                var Register = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText.ToString();
                UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText = Register.ToString();

                UtiltiesDevice = new();
                UtiltiesDevice.UtilitiesDeviceID
                   = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtiltiesDevice.UtilitiesDeviceID.ToString();

                UtiltiesMeasurementTask.UtiltiesDevice = UtiltiesDevice;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
                #endregion

                #region Result

                var registerValueCanned = staticReading[Register.ToString().Trim()];

                Result = new();
                Result.ActualMeterReadingDate = DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");
                Result.ActualMeterReadingTime = DateTime.Now.ToTimeOnly().ToString(format: "HH:mm:ss");
                Result.MeterReadingTypeCode = "1";
                Result.MeterReadingResultValue = registerValueCanned.ToString();

                #endregion

                MeterReadingDocument.UtiltiesMeasurementTask = UtiltiesMeasurementTask;
                MeterReadingDocument.Result = Result;


                ObjLst.Add(new Domain.Models.MeterReadingDocumentERPResultCreateRequestMessage()
                {
                    MessageHeader = MessageHeader,
                    MeterReadingDocument = MeterReadingDocument,
                });

            }
            #endregion

            model.MessageHeader = rootHeader;
            model.LstMeterDocumentResult = ObjLst;

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
            EnrichResultXML(ref xml);
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForBulkResult, ServiceEnumValueForBulkResult);
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

        #region Parsing Result from DB

        [HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultBulkCreateRequest_In")]
        [Consumes("text/xml")]

        public async Task MeterReadBULKRESULTConfirmation()
        {
            await MeterReadBULKRESULTConfirmationTask(); // ✅ Works
        }
        public async Task<string> MeterReadBULKRESULTConfirmationTask()
        {
            _logger.LogInformation("Debug : MeterReadRequestConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
            MdrresultHeader ObjResultHdr = null;
            List<MdrresultDetail> LstObjResultDtl = null;

            string? url = _configuration.GetSection(SectionHeader)[keyForBulkResult];
            int SplitSize = Convert.ToInt16(_configuration.GetSection("MROSplit")["SplitSize"]);
            string result = string.Empty;

            SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = null;
            try
            {
                var resultRoot = GetResultValuesForMultipleMROSend("BULK");
                if (resultRoot != null)
                {
                    _logger.LogInformation("Split Payload Debug : Total Headers: resultRoot ->" + resultRoot.Count.ToString());

                    foreach (var objMroPostInterim in resultRoot)
                    {

                        /*
                        objMroPost = MROPost [public MdrresultHeader HeaderGuid { get; set; } + public List<MdrresultDetail> DetailGuidList { get; set; }]
                        Check if DetailGuidList > Config value
                        if Yes >>> [ SPLIT] ELSE >>> go AS IS

                        WHEN SPLIT
                            Group by >> public string? MdrresultDetailUtilitiesUtilitiesDeviceId { get; set; } of  public List<MdrresultDetail> DetailGuidList { get; set; }
                            
                        Later:
                            Check Size of Each each Split Group
                            Keep on Adding each groups number & check if exceeds CONFIG VALUE
                        */
                        var SplitResultHdr = objMroPostInterim.HeaderGuid;
                        var SplitResultDtlList = objMroPostInterim.DetailGuidList;

                        _logger.LogInformation("Split Payload Debug : Total Details list of resultRoot | objMroPostInterim.DetailGuidList ->" + SplitResultDtlList.Count.ToString());

                        if (SplitResultDtlList.Count > (SplitSize - 1))
                        {

                            var SplitResultDtlListGroupByDevice =
                                       SplitResultDtlList.GroupBy(u => u.MdrresultDetailUtilitiesUtilitiesDeviceId)
                                       .Select(grp => grp.ToList())
                                       .ToList();
                            //List<List<MdrresultDetail>> aa;
                            _logger.LogInformation("Split Payload Debug : Total Device Count ->" + SplitResultDtlListGroupByDevice.Count.ToString());

                            int runningCount = 0;
                            List<MdrresultDetail> runningDtl = new List<MdrresultDetail>();
                            foreach (var SplitResultDtlGroupByDevice in SplitResultDtlListGroupByDevice)
                            {
                                runningCount = runningCount + SplitResultDtlGroupByDevice.Count;
                                runningDtl.AddRange(SplitResultDtlGroupByDevice);
                                if (runningCount > (SplitSize - 1))
                                {
                                    _logger.LogInformation("Split Payload Debug : Within 2nd Foreach runningCount | SplitSize ->" + runningCount.ToString());

                                    //MROPost mROPost = new MROPost() { HeaderGuid = SplitResultHdr, DetailGuidList = SplitResultDtlGroupByDevice };
                                    MROPost mROPost = new MROPost() { HeaderGuid = SplitResultHdr, DetailGuidList = runningDtl };
                                    SendMROResultToSAP(mROPost, url);

                                    //reset all params
                                    runningCount = 0;
                                    runningDtl.Clear();
                                }
                            }
                            //At the end - Create Payload for remaining Running Details Items
                            if (runningDtl.Count > 0)
                            {
                                _logger.LogInformation("Split Payload Debug : Outside 2nd Foreach runningCount | SplitSize ->" + runningCount.ToString());

                                MROPost mROPost = new MROPost() { HeaderGuid = SplitResultHdr, DetailGuidList = runningDtl };
                                SendMROResultToSAP(mROPost, url);

                                //reset all params
                                runningCount = 0;
                                runningDtl.Clear();
                            }
                        }
                        else
                        {
                            //When Number of Payload Less than SplitSize [Spliting is not required]
                            _logger.LogInformation("Split Payload Debug : No Split Required ->" + SplitResultDtlList.Count.ToString());
                            SendMROResultToSAP(objMroPostInterim, url);
                        }

                        //Replaced by Logic Above==SendMROResultToSAP(objMroPostInterim, url);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }



            // EXIT Condition : Start
            if (ObjResultHdr == null)
            {
                result = "No Data Retrived - Contact Administrator";
                return result;
            }
            if (LstObjResultDtl == null)
            {
                result = "No Data Retrived - Contact Administrator";
                return result;
            }
            // EXIT Condition : End


            return result;
        }


       

        #endregion

        private void SendMROResultToSAP(MROPost objPost, string url)
        {
            //******* >> required for Sending
            string result = string.Empty;

            //*********** >> Required for Update Flag
            Guid HeaderAltId = Guid.Empty;
            List<Guid> DetilAltIds = new List<Guid>();


            MdrresultHeader ObjResultHdr = null;
            List<MdrresultDetail> LstObjResultDtl = null;
            ObjResultHdr = objPost.HeaderGuid;
            LstObjResultDtl = objPost.DetailGuidList;

            MeterReadingDocumentERPResultBulkCreateRequest model = new();
            #region messageheader
            HeaderAltId = ObjResultHdr.MdrresultHeaderAltId;  //Required for Update Flag


            MeterReadResultConfirmationBulkMessageHeader rootHeader = new();

            rootHeader.UUID = ObjResultHdr.MdrresultHeaderUuid;
            rootHeader.ReferenceUUID = ObjResultHdr.MdrresultHeaderRefUuid;
            rootHeader.RecipientBusinessSystemID = ObjResultHdr.MdrresultHeaderRecipientSystemBusinessId;
            rootHeader.CreationDateTime = DateTime.SpecifyKind(ObjResultHdr.MdrresultHeaderCreationDatetime ?? DateTime.Now, DateTimeKind.Local);

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument



            List<MeterReadingDocumentERPResultCreateRequestMessage> ObjLst
                = new();

            for (int i = 0; i < LstObjResultDtl.Count; i++)
            {
                var objResultDtl = LstObjResultDtl[i];
                DetilAltIds.Add(objResultDtl.MdrresultDetailAltId); //Required for Update Flag

                var PopulateDetail = PopuplateResultDetail(objResultDtl);

                ObjLst.Add(new Domain.Models.MeterReadingDocumentERPResultCreateRequestMessage()
                {
                    MessageHeader = PopulateDetail.Item1,// MessageHeader,
                    MeterReadingDocument = PopulateDetail.Item2, //MeterReadingDocument,
                });
            }
            #endregion

            model.MessageHeader = rootHeader;
            model.LstMeterDocumentResult = ObjLst;

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
            EnrichResultXML(ref xml);
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForBulkResult, ServiceEnumValueForBulkResult);
                //Console.WriteLine(result);
                #region Update IsResultSent Flag in ResultHeader and ResultDetail
                if (!string.IsNullOrEmpty(result) && result.Trim().ToUpper().Contains("OK"))
                {
                    ContextRepository.UpdateIsResultSentForResult(_dbContext, DetilAltIds, HeaderAltId, "BULK");
                }
                #endregion
                Console.WriteLine("SAP IN POC - Result Returned : " + result);
                _logger.LogInformation("Debug >>SAP IN POC - Result Returned : " + result);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                result = ex.Message;
                _logger.LogCritical("Debug >> Error occurred in calling PostSOAPRequestAsync for IN service: " + ex.Message.ToString());

            }
        }
        private void EnrichResultXML(ref string xml)
        {
            xml = xml.Replace("<LstMeterDocumentResult>", "");
            xml = xml.Replace("</LstMeterDocumentResult>", "");
            xml = xml.Replace("MeterReadingDocumentERPResultBulkCreateRequest", "glob:MeterReadingDocumentERPResultBulkCreateRequest");
        }

        #endregion

        #endregion

        #region SINGLE

        #region  SAP 34 IN - REQUEST Confirmation Object

        private readonly string keyForSingleConfirmation = ServiceEnum.MeterReadSingleConfirmation.ToString();
        private readonly int ServiceEnumValueForSingleConfirmation = (int)ServiceEnum.MeterReadSingleConfirmation;

        #region - Parsing Confirmation 34 on the FLY
        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In")]
        [Consumes("text/xml")]
        public string MeterReadSingleConfirmation([FromBody] XElement Data)
        {

            _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");
            SmartMeterMeterReadingDocumentERPCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleConfirmation];
            string resultSend = string.Empty;

            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            // EXIT Condition : End

            #region REQUEST OBJECT CONVERSION TO CONFIRMATION OBJECT

            SmartMeterMeterReadingDocumentERPCreateConfirmation model = new();

            #region messageheader

            SingleMeterReadingConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region MeterReadingDocument
            SingleMeterReadingDocumentConfirmationMeterReadingDocument MeterReadingDocument = new();
            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.ID;
            #endregion

            #region Log
            SingleMeterReadingConfirmationLog Log = new();

            Log.BusinessDocumentProcessingResultCode = 3;
            Log.MaximumLogItemSeverityCode = 1;


            SingleMeterReadingConfirmationItem Item = new();
            Item.TypeID = "00000";
            Item.SeverityCode = 1;
            Item.Note = "Request Processed Successfully";

            Log.Item = Item;

            //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
            #endregion


            model.MessageHeader = rootHeader;
            model.MeterReadingDocument = MeterReadingDocument;
            model.Log = Log;

            #endregion

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
            //EnrichSingleXML(ref xml);
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPCreateConfirmation", "glob:SmartMeterMeterReadingDocumentERPCreateConfirmation");

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

        public string MeterReadSingleConfirmationFromReadController(SmartMeterMeterReadingDocumentERPCreateRequestRoot obj)
        {

            _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleConfirmation];
            string resultSend = string.Empty;
            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            // EXIT Condition : End

            #region REQUEST OBJECT CONVERSION TO CONFIRMATION OBJECT

            SmartMeterMeterReadingDocumentERPCreateConfirmation model = new();

            #region messageheader

            SingleMeterReadingConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region MeterReadingDocument
            SingleMeterReadingDocumentConfirmationMeterReadingDocument MeterReadingDocument = new();
            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.ID;
            #endregion

            #region Log
            SingleMeterReadingConfirmationLog Log = new();

            Log.BusinessDocumentProcessingResultCode = 3;
            Log.MaximumLogItemSeverityCode = 1;


            SingleMeterReadingConfirmationItem Item = new();
            Item.TypeID = "00000";
            Item.SeverityCode = 1;
            Item.Note = "Request Processed Successfully";

            Log.Item = Item;

            //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
            #endregion


            model.MessageHeader = rootHeader;
            model.MeterReadingDocument = MeterReadingDocument;
            model.Log = Log;

            #endregion

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
            //EnrichSingleXML(ref xml);
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPCreateConfirmation", "glob:SmartMeterMeterReadingDocumentERPCreateConfirmation");

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
            //_logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");
        }
        #endregion

        #region - Parsing Confirmation 34 From DB
        //[HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In")]
        //[Consumes("text/xml")]
        public string MeterReadSingleConfirmation()
        {

            _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");
            SmartMeterMeterReadingDocumentERPCreateRequestRoot obj = null;
            int ServiceCallLogId = 0;
            string ServiceParamJSON = string.Empty;
            try
            {

                var resultRoot = GetConfirmationValues((int)ServiceEnum.MeterReadSingleRequest);
                ServiceCallLogId = resultRoot.Item1;
                ServiceParamJSON = resultRoot.Item2;

                if (ServiceParamJSON != string.Empty)
                {
                    XMLHelper objXML = new XMLHelper();
                    XElement Data = XElement.Parse(ServiceParamJSON);
                    obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCreateRequestRoot>(Data.ToString());

                }
            }
            catch (Exception)
            {

                throw;
            }
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleConfirmation];
            string resultSend = string.Empty;

            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            else
            {

            }
            // EXIT Condition : End

            #region REQUEST OBJECT CONVERSION TO CONFIRMATION OBJECT

            SmartMeterMeterReadingDocumentERPCreateConfirmation model = new();

            #region messageheader

            SingleMeterReadingConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region MeterReadingDocument
            SingleMeterReadingDocumentConfirmationMeterReadingDocument MeterReadingDocument = new();
            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.ID;
            #endregion

            #region Log
            SingleMeterReadingConfirmationLog Log = new();

            Log.BusinessDocumentProcessingResultCode = 3;
            Log.MaximumLogItemSeverityCode = 1;


            SingleMeterReadingConfirmationItem Item = new();
            Item.TypeID = "00000";
            Item.SeverityCode = 1;
            Item.Note = "Request Processed Successfully";

            Log.Item = Item;

            //MeterReadingDocumentERPResultCreateConfirmationMessage.Log = Log;
            #endregion


            model.MessageHeader = rootHeader;
            model.MeterReadingDocument = MeterReadingDocument;
            model.Log = Log;

            #endregion

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
            //EnrichSingleXML(ref xml);
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPCreateConfirmation", "glob:SmartMeterMeterReadingDocumentERPCreateConfirmation");

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


        #endregion

        #region SAP 36 IN - SENDING RESULT SINGLE

        private readonly string keyForSingleResult = ServiceEnum.MeterReadSingleResult.ToString();
        private readonly int ServiceEnumValueForSingleResult = (int)ServiceEnum.MeterReadSingleResult;



        #region PARSING SINGLE -RESULT- ON THE FLY
        //[HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultCreateRequest_In")]
        //[Consumes("text/xml")]
        public string MeterReadSINGLERESULTConfirmation([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : MeterReadSINGLERESULTConfirmation >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultCreateRequest_In");
            SmartMeterMeterReadingDocumentERPCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleResult];
            string result = string.Empty;

            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }
            // EXIT Condition : End



            #region CANNED RESULT
            Dictionary<string, int> staticReading = new();

            staticReading.Add("KWH_TOTAL", 2271);
            staticReading.Add("MD_0622", 1);
            staticReading.Add("KVARH_LAG", 439);
            staticReading.Add("KWH18-22", 363);
            staticReading.Add("MD_2206", 1);
            staticReading.Add("KW_24_HRS", 1);
            staticReading.Add("MD_24_HRS", 1);
            staticReading.Add("KWH_0609", 246);
            staticReading.Add("KWH_0912", 328);
            staticReading.Add("KWH_1218", 700);
            staticReading.Add("KWH_2206", 631);
            staticReading.Add("KVARH_LEAD", 12);
            #endregion

            #region OBJECT CONVERSION > Read Requst TO Result
            MeterReadingDocumentERPResultCreateRequest model = new();
            #region messageheader

            MeterReadResultConfirmationBulkMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;//.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK");

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument


            MeterReadingDocumentResult MeterReadingDocument = null;
            UtiltiesMeasurementTaskResult UtiltiesMeasurementTask = null;
            UtiltiesDeviceResult UtiltiesDevice = null;

            SentResult Result = null;


            #region MeterReadingDocument
            MeterReadingDocument = new();

            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.ID;
            MeterReadingDocument.MeterReadingReasonCode = 1;
            MeterReadingDocument.ScheduledMeterReadingDate = DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");

            UtiltiesMeasurementTask = new();
            UtiltiesMeasurementTask.UtilitiesMeasurementTaskID = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesMeasurementTaskID.ToString();
            var Register = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText.ToString();
            UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText = Register.ToString();

            UtiltiesDevice = new();
            UtiltiesDevice.UtilitiesDeviceID
               = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtiltiesDevice.UtilitiesDeviceID.ToString();

            UtiltiesMeasurementTask.UtiltiesDevice = UtiltiesDevice;

            //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
            #endregion

            #region Result

            var registerValueCanned = staticReading[Register.ToString().Trim()];

            Result = new();
            Result.ActualMeterReadingDate = DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");
            Result.ActualMeterReadingTime = DateTime.Now.ToTimeOnly().ToString(format: "HH:mm:ss");
            Result.MeterReadingTypeCode = "1";
            Result.MeterReadingResultValue = registerValueCanned.ToString();

            #endregion

            MeterReadingDocument.UtiltiesMeasurementTask = UtiltiesMeasurementTask;
            MeterReadingDocument.Result = Result;


            #endregion

            model.MessageHeader = rootHeader;
            model.MeterReadingDocument = MeterReadingDocument;
            #endregion


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
            //EnrichResultXML(ref xml);
            xml = xml.Replace("MeterReadingDocumentERPResultCreateRequest", "glob:MeterReadingDocumentERPResultCreateRequest");

            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForSingleResult, ServiceEnumValueForSingleResult);
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

        #region PARSING SINGLE FROM DB
        [HttpPost("SAPAdapterWS/MeterReadingDocumentERPResultCreateRequest_In")]
        [Consumes("text/xml")]
        public string MeterReadSINGLERESULTConfirmation()
        {
            _logger.LogInformation("Debug : MeterReadSINGLERESULTConfirmation >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultCreateRequest_In");
            MdrresultHeader ObjResultHdr = null;
            List<MdrresultDetail> LstObjResultDtl = null;

            //*********** >> Required for Update Flag
            Guid HeaderAltId = Guid.Empty;
            List<Guid> DetilAltIds = new List<Guid>();

            //return "Sent back from Scheduled Job :" + DateTime.Now.ToString();
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleResult];
            string result = string.Empty;

            MeterReadingDocumentERPResultCreateRequest model = null;

            SmartMeterMeterReadingDocumentERPCreateRequestRoot obj = null;

            try
            {
                var resultRoot = GetResultValuesForMultipleMROSend("SINGLE");
                if (resultRoot != null)
                {
                    foreach (var objMroPost in resultRoot)
                    {
                        //Get a HeaderID and Respective Detail ID From objMroPostInterim
                        
                        var objResultHdr = objMroPost.HeaderGuid;
                        var objResultDtlList = objMroPost.DetailGuidList;
                        if (objResultDtlList == null || objResultDtlList.Count > 2)
                        {
                            result = "No Data Retrived - Contact Administrator";
                            continue; // This is an erronious record, Continue to Next >> Revisit.
                            //return result;
                        }
                        var PopulateDetail = PopuplateResultDetail(objResultDtlList[0]);
                        
                        model = new();
                        model.MessageHeader = PopulateDetail.Item1; // rootHeader;
                        model.MeterReadingDocument = PopulateDetail.Item2;// MeterReadingDocument;

                        //*********** >> Required for Update Flag
                        HeaderAltId = objResultHdr.MdrresultHeaderAltId;
                        DetilAltIds.Add(objResultDtlList[0].MdrresultDetailAltId);

                        _logger.LogInformation("Debug >> Completed Creating the Model");

                        #region Build the Response Payload
                       
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
                        //EnrichResultXML(ref xml);
                        xml = xml.Replace("MeterReadingDocumentERPResultCreateRequest", "glob:MeterReadingDocumentERPResultCreateRequest");

                        xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
                        _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

                        #endregion

                        try
                        {
                            Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                            _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                            result = PostSOAPRequestAsync(url, xmlSOAP, keyForSingleResult, ServiceEnumValueForSingleResult);
                            //Console.WriteLine(result);

                            #region Update IsResultSent Flag in ResultHeader and ResultDetail
                            if (!string.IsNullOrEmpty(result) && result.Trim().ToUpper().Contains("OK"))
                            {
                                ContextRepository.UpdateIsResultSentForResult(_dbContext, DetilAltIds, HeaderAltId, "SINGLE");
                            }
                            #endregion
                            Console.WriteLine("SAP IN POC - Result Returned : " + result);

                            _logger.LogInformation("Debug >>SAP IN POC - Result Returned : " + result);
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex.Message);
                            result = ex.Message;
                            _logger.LogCritical("Debug >> Error occurred in calling PostSOAPRequestAsync for IN service: " + ex.Message.ToString());

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return result;
        }


        /// <summary>
        /// Depricated >> Hangfire Job >> For Single Result
        /// </summary>
        /// <returns></returns>
        public string MeterReadSINGLERESULTConfirmationOriginal()
        {
            _logger.LogInformation("Debug : MeterReadSINGLERESULTConfirmation >> Reached the controller - SAPAdapterWS/MeterReadingDocumentERPResultCreateRequest_In");
            MdrresultHeader ObjResultHdr = null;
            List<MdrresultDetail> LstObjResultDtl = null;

            //*********** >> Required for Update Flag
            Guid HeaderAltId = Guid.Empty;
            List<Guid> DetilAltIds = new List<Guid>();


            //return "Sent back from Scheduled Job :" + DateTime.Now.ToString();

            SmartMeterMeterReadingDocumentERPCreateRequestRoot obj = null;
            try
            {
                //XMLHelper objXML = new XMLHelper();
                //obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCreateRequestRoot>(Data.ToString());
                //var author2 = tupleAuthor;
                //Console.WriteLine("Author:{0}, Title:{1}, Year:{2}.", author2.Item1, author2.Item2, author2.Item3);
                var resultRoot = GetResultValues("SINGLE");

                if (resultRoot != null)
                {
                    ObjResultHdr = new();
                    LstObjResultDtl = new();
                    ObjResultHdr = resultRoot.Item1;
                    LstObjResultDtl = resultRoot.Item2;
                }

            }
            catch (Exception)
            {
                throw;
            }
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleResult];
            string result = string.Empty;

            // EXIT Condition : Start
            if (ObjResultHdr == null)
            {
                result = "No Data Retrived - Contact Administrator";
                return result;
            }
            // EXIT Condition : End

            #region OBJECT CONVERSION > Read Requst TO Result
            MeterReadingDocumentERPResultCreateRequest model = new();
            if (LstObjResultDtl == null || LstObjResultDtl.Count > 2)
            {
                result = "No Data Retrived - Contact Administrator";
                return result;
            }

            var PopulateDetail = PopuplateResultDetail(LstObjResultDtl[0]);

            model.MessageHeader = PopulateDetail.Item1; // rootHeader;
            model.MeterReadingDocument = PopulateDetail.Item2;// MeterReadingDocument;


            //*********** >> Required for Update Flag
            HeaderAltId = ObjResultHdr.MdrresultHeaderAltId;
            DetilAltIds.Add(LstObjResultDtl[0].MdrresultDetailAltId);
            #endregion


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
            //EnrichResultXML(ref xml);
            xml = xml.Replace("MeterReadingDocumentERPResultCreateRequest", "glob:MeterReadingDocumentERPResultCreateRequest");

            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;
            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForSingleResult, ServiceEnumValueForSingleResult);
                //Console.WriteLine(result);

                #region Update IsResultSent Flag in ResultHeader and ResultDetail
                if (!string.IsNullOrEmpty(result) && result.Trim().ToUpper().Contains("OK"))
                {
                    ContextRepository.UpdateIsResultSentForResult(_dbContext, DetilAltIds, HeaderAltId, "SINGLE");
                }
                #endregion
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

        #region Common methods for DB Parsing - SINGLE and BULK Both - RESULT Related
        private List<MROPost> GetResultValuesForMultipleMROSendForBulk_Original(string strPayLoadType)
        {
            //MdrresultHeader hdr = null;
            //List<MdrresultDetail> dtl = null;
            List<MROPost> MROPostList = null;

            //var entity = _dbContext.Mdrheaders.FirstOrDefault(itm => itm.EntryId == IDval);
            //var row = _dbContext.Mdrheaders.FirstOrDefault(r => r.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper() && r.IsMdminvoked == true && r.IsProcessCompleted == false);
            //var hdrAltId = row != null ? row.MdrheaderAltId : Guid.Empty;


            //Select FIFO ResultHeader..Where isResultRecevied 1 and isResultSent 0 order by 1 asc
            //For Each Header Check if there are any Detail with isResultRecevied 1 and IsResultSent 0
            //if YES - Tht Result Header is your Candidate

            Guid rsltHdrAltId = Guid.Empty;
            List<MdrresultDetail> objDtls = null;

            var objRsltHdrLst = _dbContext.MdrresultHeaders.Where(r => r.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper()
                                    //&& r.IsResultReceived == true & r.IsResultSent == false).OrderBy(r => r.MdrresultHeaderId).ToList();
                                    && r.IsResultSent == false).OrderBy(r => r.MdrresultHeaderId).ToList();


            List<Guid> CandidateHdrList = new();
            if (objRsltHdrLst != null || objRsltHdrLst.Any())
            {
                foreach (var objHdr in objRsltHdrLst)
                {
                    var objRsltDtlLst = _dbContext.MdrresultDetails.Where(r => r.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper()
                                        && r.MdrresultHeaderAltId == objHdr.MdrresultHeaderAltId && r.IsResultReceived == true && r.IsResultSent == false).OrderBy(r => r.MdrresultDetailId).ToList();

                    if (objRsltDtlLst.Any())
                    {
                        //rsltHdrAltId = objHdr.MdrresultHeaderAltId;
                        //break;
                        CandidateHdrList.Add(objHdr.MdrresultHeaderAltId);
                    }
                }
            }

            // ********** GET RESULT HEADER DETAILS ****************

            if (CandidateHdrList != null)
            {
                if (CandidateHdrList.Count > 0)
                {
                    MROPostList = new List<MROPost>();
                    foreach (var CandidateHdr in CandidateHdrList)
                    {
                        var RsltHdr = _dbContext.MdrresultHeaders
                        .Where(g => g.MdrresultHeaderAltId == CandidateHdr)
                        .Select(g => new MdrresultHeader()
                        {
                            MdrresultHeaderId = g.MdrresultHeaderId,
                            MdrresultHeaderAltId = g.MdrresultHeaderAltId,
                            MdrheaderId = g.MdrheaderId,
                            MdrheaderAltId = g.MdrheaderAltId,
                            ServiceCallLogId = g.ServiceCallLogId,
                            PayloadType = g.PayloadType,
                            MdrresultHeaderUuid = g.MdrresultHeaderUuid,
                            MdrresultHeaderRefUuid = g.MdrresultHeaderRefUuid,
                            MdrresultHeaderCreationDatetime = g.MdrresultHeaderCreationDatetime,
                            MdrresultHeaderRecipientSystemBusinessId = g.MdrresultHeaderRecipientSystemBusinessId,
                            IsResultSent = g.IsResultSent,
                            IsResultReceived = g.IsResultReceived,
                            CreatedOn = g.CreatedOn,
                            UpdatedOn = g.UpdatedOn,
                            CreatedBy = g.CreatedBy,
                            UpdatedBy = g.UpdatedBy
                        }).FirstOrDefault();

                        if (RsltHdr != null)
                        {
                            rsltHdrAltId = RsltHdr.MdrresultHeaderAltId;

                            // ********** GET RESULT DETAIL TABLE DETAILS ****************
                            if (rsltHdrAltId != Guid.Empty)
                            {
                                objDtls = new List<MdrresultDetail>();

                                var RsltDtl = _dbContext.MdrresultDetails
                                .Where(g => g.MdrresultHeaderAltId == rsltHdrAltId
                                  && g.IsResultReceived == true & g.IsResultSent == false).OrderBy(g => g.MdrresultDetailId)
                                .Select(g => new MdrresultDetail()
                                {
                                    MdrresultDetailId = g.MdrresultDetailId,
                                    MdrresultDetailAltId = g.MdrresultDetailAltId,
                                    MdrresultHeaderId = g.MdrresultHeaderId,
                                    MdrresultHeaderAltId = g.MdrresultHeaderAltId,
                                    ServiceCallLogId = g.ServiceCallLogId,
                                    PayloadType = g.PayloadType,
                                    MdrresultDetailUuid = g.MdrresultDetailUuid,
                                    MdrresultDetailRefUuid = g.MdrresultDetailRefUuid,
                                    MdrresultDetailRecipientSystemBusinessId = g.MdrresultDetailRecipientSystemBusinessId,
                                    MdrresultDetailCreationDatetime = g.MdrresultDetailCreationDatetime,
                                    MdrresultDetailMeterReadingDocumentId = g.MdrresultDetailMeterReadingDocumentId,
                                    MdrresultDetailMeterReadingReasonCode = g.MdrresultDetailMeterReadingReasonCode,
                                    MdrresultDetailScheduledMeterReadingDate = g.MdrresultDetailScheduledMeterReadingDate,
                                    MdrresultDetailUtilitiesMeasurementTaskId = g.MdrresultDetailUtilitiesMeasurementTaskId,
                                    MdrresultDetailUtilitiesObjectIdentificationSystemCodeText = g.MdrresultDetailUtilitiesObjectIdentificationSystemCodeText,
                                    MdrresultDetailUtilitiesUtilitiesDeviceId = g.MdrresultDetailUtilitiesUtilitiesDeviceId,
                                    MdrresultDetailActualMeterReadingDate = g.MdrresultDetailActualMeterReadingDate,
                                    MdrresultDetailActualMeterReadingTime = g.MdrresultDetailActualMeterReadingTime,
                                    MdrresultDetailMeterReadingTypeCode = g.MdrresultDetailMeterReadingTypeCode,
                                    MdrresultDetailMeterReadingResultValue = g.MdrresultDetailMeterReadingResultValue,
                                    IsResultSent = g.IsResultSent,
                                    IsResultReceived = g.IsResultReceived,
                                    CreatedOn = g.CreatedOn,
                                    UpdatedOn = g.UpdatedOn,
                                    CreatedBy = g.CreatedBy,
                                    UpdatedBy = g.UpdatedBy
                                }).ToList();

                                if (RsltDtl != null)
                                {
                                    foreach (var objDtl in RsltDtl)
                                    {
                                        var isCancel = _dbContext.Mdrdetails
                                       .Where(g => g.MeterReadingDocumentId == Convert.ToString(objDtl.MdrresultDetailMeterReadingDocumentId)
                                        && g.IsCancelled == false);
                                        if (isCancel != null && isCancel.Count() > 0)
                                        {
                                            objDtls.Add(objDtl);
                                        }
                                    }
                                }

                                //Here Fill the MROPost Object
                                MROPostList.Add(new MROPost()
                                {
                                    HeaderGuid = RsltHdr,
                                    DetailGuidList = objDtls
                                });
                            }
                            // ********** GET RESULT DETAIL TABLE DETAILS ****************
                        }

                    } //Foreach CandidteHeadr
                } //END OF MASTER IF - 2

            } //END OF MASTER IF - 1

            //*** When All Good - Return ResultHeader & List<ResultDetail> Object
            //return Tuple.Create(RsltHdr ?? null, RsltDtl ?? null); ;
            //return Tuple.Create(RsltHdr ?? null, objDtls ?? null); ;
            return MROPostList;
        }


        private List<MROPost> GetResultValuesForMultipleMROSend(string strPayLoadType)
        {
            List<MROPost> MROPostList = null;
            int MROHeaderFetchSizeBulk = Convert.ToInt16(_configuration.GetSection("MROSplit")["BulkMROFetchSizeDetailTable"]);
            int MROHeaderFetchSizeSingle = Convert.ToInt16(_configuration.GetSection("MROSplit")["SingleMROFetchSizeDetailTable"]);
            int MROHeaderFetchSize = 100;
            
            if (strPayLoadType.Trim().ToUpper() == "BULK")
                MROHeaderFetchSize = MROHeaderFetchSizeBulk;
            else if(strPayLoadType.Trim().ToUpper() == "SINGLE")
                MROHeaderFetchSize = MROHeaderFetchSizeSingle;



            string? ProcessingDateConfig = _configuration.GetSection("MROSplit")["BulkMROProcessingDate"];
            DateTime ProcessingDateLINQ = DateTime.Now.AddDays(-1);
            DateTime ProcessingDateTodayLINQ = DateTime.Now;
            if (ProcessingDateConfig != "")
            {
                ProcessingDateLINQ = Convert.ToDateTime(ProcessingDateConfig);
            }

            //Select FIFO ResultHeader..Where isResultRecevied 1 and isResultSent 0 order by 1 asc
            //For Each Header Check if there are any Detail with isResultRecevied 1 and IsResultSent 0
            //if YES - Tht Result Header is your Candidate

            Guid rsltHdrAltId = Guid.Empty;
            List<MdrresultDetail> objDtls = null;

            // 1. From MDRRESULTDETAIL Get List of isResultSent = 0 for a date [Configurable] >> If Config Value TODAY, Then Process for GetDate()-1 Else for that Date in [YYYY-MM-DD] Format
            var mdrResultPopulation = _dbContext.MdrresultDetails
                        .Where(x => x.MdrresultDetailScheduledMeterReadingDate.HasValue 
                        && x.MdrresultDetailScheduledMeterReadingDate.Value.Date == ProcessingDateLINQ.Date
                        && x.IsResultSent == false && x.IsPickedByAdapter == false &&
                        x.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper())
                        .OrderBy(x => x.MdrresultDetailId)
                        .AsNoTracking()
                        .Take(MROHeaderFetchSize)
                        .ToList();

            if (strPayLoadType.Trim().ToUpper() == "SINGLE")
            {
                //This is for Instantaneous Load  >> After CDC helps to send ODR
                var mdrResultInstantaneous = _dbContext.MdrresultDetails
                        .Where(x => x.MdrresultDetailScheduledMeterReadingDate.HasValue
                        && x.MdrresultDetailScheduledMeterReadingDate.Value.Date == ProcessingDateTodayLINQ.Date
                        && (new[] { 13, 18 }).Contains(x.MdrresultDetailMeterReadingReasonCode)
                        && x.IsResultSent == false && x.IsPickedByAdapter == false &&
                        x.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper())
                        .OrderBy(x => x.MdrresultDetailId)
                        .AsNoTracking()
                        .Take(MROHeaderFetchSize)
                        .ToList();

                if (mdrResultInstantaneous != null && mdrResultInstantaneous.Count() > 0)
                {
                    mdrResultPopulation.AddRange(mdrResultInstantaneous);
                }
            }


            // 3. Get Distinct HeaderAltId >>> Select from Details Those Details AltId
            List<Guid> CandidateHdrList = new();
            CandidateHdrList = mdrResultPopulation.DistinctBy(u => u.MdrresultHeaderAltId).Select(x => x.MdrresultHeaderAltId).ToList();

            if (CandidateHdrList != null)
            {
                // 2. Update isPicked by Fetching MdrresultDetailsId
                var mdrMarkIsPicked = _dbContext.MdrresultDetails
                                .Where(u => CandidateHdrList.Contains(u.MdrresultHeaderAltId))
                                .ExecuteUpdateAsync(setters => setters
                                    .SetProperty(u => u.IsPickedByAdapter, u => true)
                                );
                var rowsAffected = mdrMarkIsPicked;
            }
            /* Mismatch in MdrresultDetailId vs MdrresultHeaderAltId >> Commneted
            // 2. Update isPicked by Fetching MdrresultDetailsId
            //var mdrPopulationResultIds = mdrResultPopulation.Select(u => u.MdrresultDetailId).ToList();

            var mdrMarkIsPicked = _dbContext.MdrresultDetails
                            .Where(u => mdrPopulationResultIds.Contains(u.MdrresultDetailId))
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(u => u.IsPickedByAdapter, u => true)
                            );
            var rowsAffected = mdrMarkIsPicked;
            */

            // ********** GET RESULT HEADER DETAILS ****************

            if (CandidateHdrList != null)
            {
                if (CandidateHdrList.Count > 0)
                {
                    MROPostList = new List<MROPost>();
                    foreach (var CandidateHdr in CandidateHdrList)
                    {
                        var RsltHdr = _dbContext.MdrresultHeaders
                        .Where(g => g.MdrresultHeaderAltId == CandidateHdr)
                        .AsNoTracking()
                        .Select(g => new MdrresultHeader()
                        {
                            MdrresultHeaderId = g.MdrresultHeaderId,
                            MdrresultHeaderAltId = g.MdrresultHeaderAltId,
                            MdrheaderId = g.MdrheaderId,
                            MdrheaderAltId = g.MdrheaderAltId,
                            ServiceCallLogId = g.ServiceCallLogId,
                            PayloadType = g.PayloadType,
                            MdrresultHeaderUuid = g.MdrresultHeaderUuid,
                            MdrresultHeaderRefUuid = g.MdrresultHeaderRefUuid,
                            MdrresultHeaderCreationDatetime = g.MdrresultHeaderCreationDatetime,
                            MdrresultHeaderRecipientSystemBusinessId = g.MdrresultHeaderRecipientSystemBusinessId,
                            IsResultSent = g.IsResultSent,
                            IsResultReceived = g.IsResultReceived,
                            CreatedOn = g.CreatedOn,
                            UpdatedOn = g.UpdatedOn,
                            CreatedBy = g.CreatedBy,
                            UpdatedBy = g.UpdatedBy
                        }).FirstOrDefault();

                        if (RsltHdr != null)
                        {
                            rsltHdrAltId = RsltHdr.MdrresultHeaderAltId;

                            // ********** GET RESULT DETAIL TABLE DETAILS ****************
                            if (rsltHdrAltId != Guid.Empty)
                            {
                                objDtls = new List<MdrresultDetail>();

                                var RsltDtl = _dbContext.MdrresultDetails
                                    .Where(g => g.MdrresultHeaderAltId == rsltHdrAltId
                                    && g.IsResultReceived == true & g.IsResultSent == false).OrderBy(g => g.MdrresultDetailId)
                                    .AsNoTracking()
                                    .Select(g => new MdrresultDetail()
                                    {
                                        MdrresultDetailId = g.MdrresultDetailId,
                                        MdrresultDetailAltId = g.MdrresultDetailAltId,
                                        MdrresultHeaderId = g.MdrresultHeaderId,
                                        MdrresultHeaderAltId = g.MdrresultHeaderAltId,
                                        ServiceCallLogId = g.ServiceCallLogId,
                                        PayloadType = g.PayloadType,
                                        MdrresultDetailUuid = g.MdrresultDetailUuid,
                                        MdrresultDetailRefUuid = g.MdrresultDetailRefUuid,
                                        MdrresultDetailRecipientSystemBusinessId = g.MdrresultDetailRecipientSystemBusinessId,
                                        MdrresultDetailCreationDatetime = g.MdrresultDetailCreationDatetime,
                                        MdrresultDetailMeterReadingDocumentId = g.MdrresultDetailMeterReadingDocumentId,
                                        MdrresultDetailMeterReadingReasonCode = g.MdrresultDetailMeterReadingReasonCode,
                                        MdrresultDetailScheduledMeterReadingDate = g.MdrresultDetailScheduledMeterReadingDate,
                                        MdrresultDetailUtilitiesMeasurementTaskId = g.MdrresultDetailUtilitiesMeasurementTaskId,
                                        MdrresultDetailUtilitiesObjectIdentificationSystemCodeText = g.MdrresultDetailUtilitiesObjectIdentificationSystemCodeText,
                                        MdrresultDetailUtilitiesUtilitiesDeviceId = g.MdrresultDetailUtilitiesUtilitiesDeviceId,
                                        MdrresultDetailActualMeterReadingDate = g.MdrresultDetailActualMeterReadingDate,
                                        MdrresultDetailActualMeterReadingTime = g.MdrresultDetailActualMeterReadingTime,
                                        MdrresultDetailMeterReadingTypeCode = g.MdrresultDetailMeterReadingTypeCode,
                                        MdrresultDetailMeterReadingResultValue = g.MdrresultDetailMeterReadingResultValue,
                                        IsResultSent = g.IsResultSent,
                                        IsResultReceived = g.IsResultReceived,
                                        CreatedOn = g.CreatedOn,
                                        UpdatedOn = g.UpdatedOn,
                                        CreatedBy = g.CreatedBy,
                                        UpdatedBy = g.UpdatedBy
                                    }).ToList();

                                if (RsltDtl != null)
                                {
                                    foreach (var objDtl in RsltDtl)
                                    {
                                        var isCancel = _dbContext.Mdrdetails
                                       .Where(g => g.MeterReadingDocumentId == Convert.ToString(objDtl.MdrresultDetailMeterReadingDocumentId)
                                        && g.IsCancelled == false)
                                       .AsNoTracking();
                                        if (isCancel != null && isCancel.Count() > 0)
                                        {
                                            objDtls.Add(objDtl);
                                        }
                                    }
                                }

                                //Here Fill the MROPost Object
                                MROPostList.Add(new MROPost()
                                {
                                    HeaderGuid = RsltHdr,
                                    DetailGuidList = objDtls
                                });
                            }
                            // ********** GET RESULT DETAIL TABLE DETAILS ****************
                        }

                    } //Foreach CandidteHeadr
                } //END OF MASTER IF - 2

            } //END OF MASTER IF - 1

            //*** When All Good - Return ResultHeader & List<ResultDetail> Object
            return MROPostList;
        }

        
        private Tuple<MdrresultHeader?, List<MdrresultDetail>?> GetResultValues(string strPayLoadType)
        {
            MdrresultHeader hdr = null;
            List<MdrresultDetail> dtl = null;

            //var entity = _dbContext.Mdrheaders.FirstOrDefault(itm => itm.EntryId == IDval);
            //var row = _dbContext.Mdrheaders.FirstOrDefault(r => r.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper() && r.IsMdminvoked == true && r.IsProcessCompleted == false);
            //var hdrAltId = row != null ? row.MdrheaderAltId : Guid.Empty;


            //Select FIFO ResultHeader..Where isResultRecevied 1 and isResultSent 0 order by 1 asc
            //For Each Header Check if there are any Detail with isResultRecevied 1 and IsResultSent 0
            //if YES - Tht Result Header is your Candidate
            Guid rsltHdrAltId = Guid.Empty;
            var objRsltHdrLst = _dbContext.MdrresultHeaders.Where(r => r.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper()
                                    //&& r.IsResultReceived == true & r.IsResultSent == false).OrderBy(r => r.MdrresultHeaderId).ToList();
                                    && r.IsResultSent == false).OrderBy(r => r.MdrresultHeaderId).ToList();

            if (objRsltHdrLst != null || objRsltHdrLst.Any())
            {
                foreach (var objHdr in objRsltHdrLst)
                {
                    var objRsltDtlLst = _dbContext.MdrresultDetails.Where(r => r.PayloadType.Trim().ToUpper() == strPayLoadType.Trim().ToUpper()
                                        && r.MdrresultHeaderAltId == objHdr.MdrresultHeaderAltId && r.IsResultReceived == true && r.IsResultSent == false).OrderBy(r => r.MdrresultDetailId).ToList();

                    if (objRsltDtlLst.Any())
                    {
                        rsltHdrAltId = objHdr.MdrresultHeaderAltId;
                        break;
                    }
                }
            }
            else { return null; }


            // ********** GET RESULT HEADER DETAILS ****************

            var RsltHdr = _dbContext.MdrresultHeaders
            .Where(g => g.MdrresultHeaderAltId == rsltHdrAltId)
            .Select(g => new MdrresultHeader()
            {
                MdrresultHeaderId = g.MdrresultHeaderId,
                MdrresultHeaderAltId = g.MdrresultHeaderAltId,
                MdrheaderId = g.MdrheaderId,
                MdrheaderAltId = g.MdrheaderAltId,
                ServiceCallLogId = g.ServiceCallLogId,
                PayloadType = g.PayloadType,
                MdrresultHeaderUuid = g.MdrresultHeaderUuid,
                MdrresultHeaderRefUuid = g.MdrresultHeaderRefUuid,
                MdrresultHeaderCreationDatetime = g.MdrresultHeaderCreationDatetime,
                MdrresultHeaderRecipientSystemBusinessId = g.MdrresultHeaderRecipientSystemBusinessId,
                IsResultSent = g.IsResultSent,
                IsResultReceived = g.IsResultReceived,
                CreatedOn = g.CreatedOn,
                UpdatedOn = g.UpdatedOn,
                CreatedBy = g.CreatedBy,
                UpdatedBy = g.UpdatedBy
            }).FirstOrDefault();

            if (RsltHdr != null)
            {
                rsltHdrAltId = RsltHdr.MdrresultHeaderAltId;
            }

            // ********** GET RESULT DETAIL TABLE DETAILS ****************
            if (rsltHdrAltId != Guid.Empty)
            {
                List<MdrresultDetail> objDtls = new List<MdrresultDetail>();

                var RsltDtl = _dbContext.MdrresultDetails
                .Where(g => g.MdrresultHeaderAltId == rsltHdrAltId
                  && g.IsResultReceived == true & g.IsResultSent == false).OrderBy(g => g.MdrresultDetailId)
                .Select(g => new MdrresultDetail()
                {
                    MdrresultDetailId = g.MdrresultDetailId,
                    MdrresultDetailAltId = g.MdrresultDetailAltId,
                    MdrresultHeaderId = g.MdrresultHeaderId,
                    MdrresultHeaderAltId = g.MdrresultHeaderAltId,
                    ServiceCallLogId = g.ServiceCallLogId,
                    PayloadType = g.PayloadType,
                    MdrresultDetailUuid = g.MdrresultDetailUuid,
                    MdrresultDetailRefUuid = g.MdrresultDetailRefUuid,
                    MdrresultDetailRecipientSystemBusinessId = g.MdrresultDetailRecipientSystemBusinessId,
                    MdrresultDetailCreationDatetime = g.MdrresultDetailCreationDatetime,
                    MdrresultDetailMeterReadingDocumentId = g.MdrresultDetailMeterReadingDocumentId,
                    MdrresultDetailMeterReadingReasonCode = g.MdrresultDetailMeterReadingReasonCode,
                    MdrresultDetailScheduledMeterReadingDate = g.MdrresultDetailScheduledMeterReadingDate,
                    MdrresultDetailUtilitiesMeasurementTaskId = g.MdrresultDetailUtilitiesMeasurementTaskId,
                    MdrresultDetailUtilitiesObjectIdentificationSystemCodeText = g.MdrresultDetailUtilitiesObjectIdentificationSystemCodeText,
                    MdrresultDetailUtilitiesUtilitiesDeviceId = g.MdrresultDetailUtilitiesUtilitiesDeviceId,
                    MdrresultDetailActualMeterReadingDate = g.MdrresultDetailActualMeterReadingDate,
                    MdrresultDetailActualMeterReadingTime = g.MdrresultDetailActualMeterReadingTime,
                    MdrresultDetailMeterReadingTypeCode = g.MdrresultDetailMeterReadingTypeCode,
                    MdrresultDetailMeterReadingResultValue = g.MdrresultDetailMeterReadingResultValue,
                    IsResultSent = g.IsResultSent,
                    IsResultReceived = g.IsResultReceived,
                    CreatedOn = g.CreatedOn,
                    UpdatedOn = g.UpdatedOn,
                    CreatedBy = g.CreatedBy,
                    UpdatedBy = g.UpdatedBy
                }).ToList();

                if (RsltDtl != null)
                {
                    foreach (var objDtl in RsltDtl)
                    {
                        var isCancel = _dbContext.Mdrdetails
                       .Where(g => g.MeterReadingDocumentId == Convert.ToString(objDtl.MdrresultDetailMeterReadingDocumentId)
                        && g.IsCancelled == false);
                        if (isCancel != null && isCancel.Count() > 0)
                        {
                            objDtls.Add(objDtl);
                        }
                    }
                }


                //*** When All Good - Return ResultHeader & List<ResultDetail> Object
                //return Tuple.Create(RsltHdr ?? null, RsltDtl ?? null); ;
                return Tuple.Create(RsltHdr ?? null, objDtls ?? null); ;
            }



            //***End of Method***
            return null;// Tuple.Create(hdr, dtl);
        }

        private Tuple<MeterReadResultConfirmationBulkMessageHeader, MeterReadingDocumentResult> PopuplateResultDetail(MdrresultDetail obj)
        {
            #region messageheader

            MeterReadResultConfirmationBulkMessageHeader rootHeader = new();
            rootHeader.UUID = obj.MdrresultDetailUuid;
            rootHeader.ReferenceUUID = obj.MdrresultDetailRefUuid;
            rootHeader.RecipientBusinessSystemID = obj.MdrresultDetailRecipientSystemBusinessId.Trim();


            //string pattern = "yyyy-MM-dd HH:mm:ss.fffffff";

            //DateTime value = DateTime.ParseExact(obj.MdrresultDetailCreationDatetime.ToString(), pattern, CultureInfo.InvariantCulture);
            //**** Datetime Issue in SAP ****
            //*** DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK")	2015-05-16T05:50:06.7199222-04:00
            DateTime result = DateTime.Now;
            DateTime result2 = DateTime.Now;
            if (obj.MdrresultDetailCreationDatetime.HasValue)
                result = Convert.ToDateTime(obj.MdrresultDetailCreationDatetime);
            //*** Assign 
            result2 = DateTime.SpecifyKind(obj.MdrresultDetailCreationDatetime ?? DateTime.Now, DateTimeKind.Local);
            //result2 = DateTime.SpecifyKind(value, DateTimeKind.Local);
            rootHeader.CreationDateTime = result2;// result.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK GMT"); // obj.MdrresultDetailCreationDatetime ?? DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument


            MeterReadingDocumentResult MeterReadingDocument = null;
            UtiltiesMeasurementTaskResult UtiltiesMeasurementTask = null;
            UtiltiesDeviceResult UtiltiesDevice = null;

            SentResult Result = null;


            #region MeterReadingDocument
            MeterReadingDocument = new();

            MeterReadingDocument.ID = obj.MdrresultDetailMeterReadingDocumentId.ToString().Trim();
            MeterReadingDocument.MeterReadingReasonCode = obj.MdrresultDetailMeterReadingReasonCode;
            MeterReadingDocument.ScheduledMeterReadingDate = obj?.MdrresultDetailScheduledMeterReadingDate?.ToDateOnly().ToString(format: "yyyy-MM-dd");// TO DO -- > DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");

            UtiltiesMeasurementTask = new();
            var taskID = Convert.ToInt64(obj?.MdrresultDetailUtilitiesMeasurementTaskId.ToString().Trim());
            UtiltiesMeasurementTask.UtilitiesMeasurementTaskID = taskID.ToString();// obj?.MdrresultDetailUtilitiesMeasurementTaskId;
            //var Register = obj.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText.ToString();
            var Register = obj?.MdrresultDetailUtilitiesObjectIdentificationSystemCodeText?.ToString().Trim();
            UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText = Register.ToString().Trim();

            UtiltiesDevice = new();
            UtiltiesDevice.UtilitiesDeviceID
               = obj?.MdrresultDetailUtilitiesUtilitiesDeviceId?.ToString().Trim();

            UtiltiesMeasurementTask.UtiltiesDevice = UtiltiesDevice;

            //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
            #endregion

            #region Result

            //var registerValueCanned = staticReading[Register.ToString().Trim()];

            Result = new();
            //DEV01 >>> Result.ActualMeterReadingDate = Convert.ToDateTime(obj?.MdrresultDetailActualMeterReadingDate).ToDateOnly().ToString(format: "yyyy-MM-dd"); // DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");

            //********>>>[ ActualMeterReadingDate = ScheduledMeterReadingDate ] => This is As Requested by Felix Sir [23.08.2024 - During Tech UAT]
            //Iteration 01 >>> Result.ActualMeterReadingDate = obj?.MdrresultDetailScheduledMeterReadingDate?.ToDateOnly().ToString(format: "yyyy-MM-dd");// TO DO -- > DateTime.Now.ToDateOnly().ToString(format: "yyyy-MM-dd");

            //Iteration 02 >>> ***>>> changed as per discusiion with Fleix Sir[27.08.2024] >> Email : MR date in Meter reading payload
            Result.ActualMeterReadingDate = Convert.ToDateTime(obj?.MdrresultDetailActualMeterReadingDate).ToDateOnly().ToString(format: "yyyy-MM-dd");
            Result.ActualMeterReadingTime = Convert.ToDateTime(obj?.MdrresultDetailActualMeterReadingTime).ToTimeOnly().ToString(format: "HH:mm:ss"); //DateTime.Now.ToTimeOnly().ToString(format: "HH:mm:ss");
            Result.MeterReadingTypeCode = obj?.MdrresultDetailMeterReadingTypeCode?.ToString(); //"1";
            Result.MeterReadingResultValue = obj.MdrresultDetailMeterReadingResultValue;// registerValueCanned;

            #endregion

            MeterReadingDocument.UtiltiesMeasurementTask = UtiltiesMeasurementTask;
            MeterReadingDocument.Result = Result;


            #endregion

            return Tuple.Create(rootHeader, MeterReadingDocument);
        }

        #endregion

        #region Common Methods for DB Parsing - SINGLE and BULK Both - Confirmation Related
        private Tuple<int, string> GetConfirmationValues(int ServiceId)
        {
            var row = _dbContext.ServiceCallLogs.Where(r => r.ServiceId == ServiceId && r.IsSuccess == true).OrderBy(r => r.EntryId).FirstOrDefault();
            int EntryId = row != null ? (int)row.EntryId : 0;
            string ServiceJSON = row != null ? row.ServiceParamJson : string.Empty;

            //===> Return
            return Tuple.Create(EntryId, ServiceJSON);
        }
        #endregion


        #region Private Class - Not a Place to Declare - Bulk - Send Multiple response

        public class MROPost
        {
            public MdrresultHeader HeaderGuid { get; set; }
            public List<MdrresultDetail> DetailGuidList { get; set; }

        }
        #endregion
    }
}
