using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models.ManualMeterRead;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class ConnectionDisconnectionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<ConnectionDisconnectionController> _logger;

        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        public string xmlSOAP = string.Empty;
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string SectionHeader = "EndPoints";
        public ConnectionDisconnectionController(ILogger<ConnectionDisconnectionController> logger, AdapterContext dbContext,
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

        #region Bulk - SAP OUT - Connection Disconnection Request Bulk

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForBulk = ServiceEnum.ConnectionDisconnectionRequestBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.ConnectionDisconnectionRequestBulk;

        [HttpPost("SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPBulkCreateRequest_OutService")]
        [Consumes("text/xml")]
        public void ConnectionDisconnectionRequestBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:ConnectionDisconnectionRequestBulk >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPBulkCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            bool RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:ConnectionDisconnectionRequestBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ConnectionDisconnectionRequestBulk: {Data}");
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
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:ConnectionDisconnectionRequestBulk >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:ConnectionDisconnectionRequestBulk: {ex.Message}");
                    _logger.LogCritical("Debug:ConnectionDisconnectionRequestBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion

        #region SINGLE 24 - SAP OUT - Connection Disconnection Request

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.ConnectionDisconnectionRequestSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.ConnectionDisconnectionRequestSingle;


        [HttpPost("SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest_OutService")]
        [Consumes("text/xml")]
        [Authorize]
        public void ConnectionDisconnectionRequestSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:ConnectionDisconnectionRequestSingle >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            bool RetStatus = false;

            if (Data != null)
            {
                try
                {
                    ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                    SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequestRoot obj = null;
                    try
                    {
                        XMLHelper objXML = new XMLHelper();
                        obj = objXML.Deserialize<SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequestRoot>(Data.ToString());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ///=====>>> XSS Part End ======
                    ///

                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:ConnectionDisconnectionRequestSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ConnectionDisconnectionRequestSingle: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : MeterReadCancelSingle >> Data Insert to HEADER and DETAIL Table of Cancellation Request - Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationRequest_OutService");

                    ContextRepository.MeterStatusChangeRequestSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion

                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:ConnectionDisconnectionRequestSingle >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:ConnectionDisconnectionRequestSingle: {ex.Message}");
                    _logger.LogCritical("Debug:ConnectionDisconnectionRequestSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion

        #region SINGLE - SAP OUT - Connection Disconnection CANCELLATION Request

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingleCancellation = ServiceEnum.ConnectionDisconnectioncancellationRequest.ToString();
        private readonly int ServiceEnumValueForSingleCancellation = (int)ServiceEnum.ConnectionDisconnectioncancellationRequest;

        [HttpPost("SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCancellationRequest_OutService")]
        [Consumes("text/xml")]

        public void ConnectionDisconnectioncancellationRequest([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:ConnectionDisconnectioncancellationRequest >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCancellationRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            bool RetStatus = false;

            if (Data != null)
            {
                try
                {
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForSingleCancellation, ServiceEnumValueForSingleCancellation);
                    _logger.LogInformation("Debug:ConnectionDisconnectioncancellationRequest >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:ConnectionDisconnectioncancellationRequest: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }


                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingleCancellation, ServiceEnumValueForSingleCancellation);
                    _logger.LogInformation("Debug:ConnectionDisconnectioncancellationRequest >> Updated DB with Final Value 1 - Success");

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:ConnectionDisconnectioncancellationRequest: {ex.Message}");
                    _logger.LogCritical("Debug:ConnectionDisconnectioncancellationRequest >> Error occurred: " + ex.Message.ToString());
                }
            }
        }

        #endregion

        #region SAP 25 IN - Single	Connection/Disconnection Request	Connect/Disconnect Confirmation

        private readonly string keyForConnectionDisconnectionRequestConfirmationSingle = ServiceEnum.ConnectionDisconnectionRequestConfirmationSingle.ToString();
        private readonly int ServiceEnumValueForConnectionDisconnectionRequestConfirmationSingle = (int)ServiceEnum.ConnectionDisconnectionRequestConfirmationSingle;

        [HttpPost("SAPAdapterWS/    ")]
        [Consumes("text/xml")]
        public string ConnectionDisconnectionConfirmationSINGLE([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : ManualMeterReadsCreateConfirmationSINGLE >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation_In");
            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }


            string? url = _configuration.GetSection(SectionHeader)[keyForConnectionDisconnectionRequestConfirmationSingle];
            string result = string.Empty;


            string? isSuccess = _configuration.GetSection(SectionHeader)["ConnectionDisconnectionSuccess"];


            var ResultCode = 1;
            var ProcessingResultCode = 3;
            var LogSeverityCode = 1;
            var itemTypeID = "0000";
            var ItemSeverityCode = 1;
            var ItemNote = "Request Processed Successfully";
            
            if (isSuccess.Trim().ToUpper() != "TRUE")
            {
                ResultCode = 2;
                ProcessingResultCode =5;
                LogSeverityCode = 3;
                itemTypeID = "01";
                ItemSeverityCode = 3;
                ItemNote = "Request Processed Failed";
            }
            

            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End




            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation model = new();

            #region messageheader

            ConDecon_MessageHeader_IN rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region UtilitiesConnectionStatusChangeRequest
            UtilitiesConnectionStatusChangeRequest_IN objUtilitiesConnectionStatusChangeRequest = new();

            objUtilitiesConnectionStatusChangeRequest.ID = obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.ID;
            objUtilitiesConnectionStatusChangeRequest.CategoryCode = obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.CategoryCode;

            DeviceConnectionStatus_IN objDeviceConnectionStatus = new();

            objDeviceConnectionStatus.UtilitiesDeviceConnectionStatusProcessingResultCode = ResultCode; //1-- Success | 2 - Failure
            objDeviceConnectionStatus.UtilitiesDeviceID = obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus.UtilitiesDeviceID;
            objDeviceConnectionStatus.ProcessingDateTime = obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.PlannedProcessingDateTime;

            objUtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus = objDeviceConnectionStatus;
            #endregion

            #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB

            ConDecon_Log ChildLog = new();

            ChildLog.BusinessDocumentProcessingResultCode = ProcessingResultCode;
            ChildLog.MaximumLogItemSeverityCode = LogSeverityCode;

            ConDecon_LogItem ChildLogItem = new();
            ChildLogItem.Note = ItemNote;
            ChildLogItem.TypeID = itemTypeID;
            ChildLogItem.SeverityCode = ItemSeverityCode;

            ChildLog.Item = ChildLogItem;

            #endregion

            model.MessageHeader = rootHeader;
            model.UtilitiesConnectionStatusChangeRequest = objUtilitiesConnectionStatusChangeRequest;
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
            xml = xml.Replace("SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation", "glob:SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;

            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForConnectionDisconnectionRequestConfirmationSingle, ServiceEnumValueForConnectionDisconnectionRequestConfirmationSingle);
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

        // --> Through Schedular
        public string ConnectionDisconnectionConfirmationSINGLE()
        {
            _logger.LogInformation("Debug : ConnectionDisconnectionConfirmationSINGLE >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation_In");
            var UpdateId = 0;
            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation model = null;

            string resultHngfire = string.Empty;
            string result = string.Empty;
            string? url = _configuration.GetSection(SectionHeader)[keyForConnectionDisconnectionRequestConfirmationSingle];

            try
            {
                var objList = GetCDCONValues();


                if (objList != null && objList.Count > 0)
                {
                    

                    foreach (var obj in objList)
                    {
                        model = obj.Item1;
                        UpdateId = obj.Item2;



                        #region SAP XML SOAP Preapre

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
                        xml = xml.Replace("SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation", "glob:SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation");
                        xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;

                        _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");


                        #endregion

                        try
                        {
                            Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                            _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                            result = PostSOAPRequestAsync(url, xmlSOAP, keyForConnectionDisconnectionRequestConfirmationSingle, ServiceEnumValueForConnectionDisconnectionRequestConfirmationSingle);
                            //Console.WriteLine(result);


                            if (!string.IsNullOrEmpty(result) && result.Trim().ToUpper().Contains("OK"))
                            {
                                resultHngfire = "CDC Return Success for ServiceCallLog: " + UpdateId.ToString();
                                ContextRepository.UpdateIsResultSentByAdapterFlag(_dbContext, UpdateId);
                            }

                            Console.WriteLine("SAP IN POC - Result Returned : " + resultHngfire);
                            _logger.LogInformation("Debug >>SAP IN POC - Result Returned : " + resultHngfire);

                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex.Message);
                            resultHngfire = ex.Message;
                            _logger.LogCritical("Debug >> Error occurred in calling PostSOAPRequestAsync for IN service: " + ex.Message.ToString());
                        }
                    }


                    _logger.LogInformation("Debug >> Completed Creating the Model");

                }
                else {
                    if (model == null || UpdateId == 0)
                    {
                        resultHngfire = "Object sent is Null";
                        return resultHngfire;
                    }
                }

               
            }
            catch (Exception)
            {
                throw;
            }
            // EXIT Condition : Start

            // EXIT Condition : End


            

            return resultHngfire;
        }



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

        //private Tuple<MdrresultHeader?, List<MdrresultDetail>?> GetCDCONValues(string strPayLoadType)
        private List<Tuple<SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation, Int32>> GetCDCONValues()
        {
            int CDCFetchSize = Convert.ToInt16(_configuration.GetSection("MROSplit")["CDCFetchSize"]);
            List<Tuple<SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation, Int32>> objReturnList = null;

            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation model = null;

            try
            {

                var objDBList = _dbContext.MeterConnectionStatusSingles
                        .Where(r => r.IsCompletedByMdm == true && r.IsResultSentByAdapter == false)
                        .AsEnumerable()
                        .OrderBy(r => r.MeterConnectionStatusSingleId)
                        .Take(CDCFetchSize);
                //var hdrAltId = row != null ? row.MdrheaderAltId : Guid.Empty;

                if (objDBList == null)
                {
                    _logger.LogInformation("CDC >> GetCDCONValues >> No Data Found for CDC SINGLE RETURN");
                    //return Tuple.Create(model, 0);
                    return null;
                }

                objReturnList = new();
                foreach (var obj in objDBList)
                {
                    model = new();
                    #region Creation of Return Object - SAP MESSAGE

                    #region messageheader

                    ConDecon_MessageHeader_IN rootHeader = new();
                    rootHeader.UUID = obj.McsrefUuidbyMdm;// Guid.NewGuid();
                    rootHeader.ReferenceUUID = obj.Mcsuuid;//obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.UUID;
                    rootHeader.RecipientBusinessSystemID = obj.McssenderBusinessSystemIdbySap.Trim();// obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.SenderBusinessSystemID;
                    rootHeader.CreationDateTime = DateTime.SpecifyKind(obj.McscreationDatetimeByMdm ?? DateTime.Now, DateTimeKind.Local);

                    //assign to main object
                    //model.MessageHeader = rootHeader;
                    #endregion

                    #region UtilitiesConnectionStatusChangeRequest
                    UtilitiesConnectionStatusChangeRequest_IN objUtilitiesConnectionStatusChangeRequest = new();

                    objUtilitiesConnectionStatusChangeRequest.ID = obj.McschangeRequestId.Trim();
                    //obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.ID;
                    objUtilitiesConnectionStatusChangeRequest.CategoryCode = obj.McschangeRequestCategoryCode ?? 0; //0 Won;t happen - Wrong
                                                                                                                    //obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.CategoryCode;

                    DeviceConnectionStatus_IN objDeviceConnectionStatus = new();

                    objDeviceConnectionStatus.UtilitiesDeviceConnectionStatusProcessingResultCode = obj.McsutilitiesDeviceConnectionStatusProcessingResultCodeByMdm ?? 0;
                    //ResultCode; //1-- Success | 2 - Failure
                    objDeviceConnectionStatus.UtilitiesDeviceID = obj.McsutilitiesDeviceId;
                    // obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus.UtilitiesDeviceID;
                    objDeviceConnectionStatus.ProcessingDateTime = DateTime.SpecifyKind(obj.McsprocessingDateTimeByMdm ?? DateTime.Now, DateTimeKind.Local);
                    //obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.PlannedProcessingDateTime;

                    objUtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus = objDeviceConnectionStatus;
                    #endregion

                    #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB

                    ConDecon_Log ChildLog = new();

                    ChildLog.BusinessDocumentProcessingResultCode = obj.McslogBusinessDocumentProcessingResultCodeByMdm ?? 0;// ProcessingResultCode;
                    ChildLog.MaximumLogItemSeverityCode = obj.McslogMaximumLogItemSeverityCodeByMdm ?? 0; // LogSeverityCode;

                    ConDecon_LogItem ChildLogItem = new();
                    ChildLogItem.Note = obj.McslogItemNoteByMdm.Trim();
                    ChildLogItem.TypeID = obj.McslogItemTypeIdByMdm.Trim();
                    ChildLogItem.SeverityCode = obj.McslogItemSeverityCodeByMdm ?? 0;// ItemSeverityCode;

                    ChildLog.Item = ChildLogItem;

                    #endregion

                    model.MessageHeader = rootHeader;
                    model.UtilitiesConnectionStatusChangeRequest = objUtilitiesConnectionStatusChangeRequest;
                    model.Log = ChildLog;

                    #endregion

                    var itrObj = Tuple.Create(model, Convert.ToInt32(obj.ServiceCallLogId));
                    objReturnList.Add(itrObj);
                }


            }
            catch (Exception ex)
            {
                _logger.LogInformation("CDC SINGLE RETURN>> GetCDCONValues >> Error While Fetch Top 20 Records from MeterConnectionStatusSingles.." + ex.ToString());
                return null;

                //throw;
            }
            

            //return Tuple.Create(model, Convert.ToInt32(obj.ServiceCallLogId));
            return objReturnList;
        }


        private Tuple<SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation, Int32> GetTotalCDCONCollection()
        {
            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation model = new();
            var obj = _dbContext.MeterConnectionStatusSingles.FirstOrDefault(r => r.IsCompletedByMdm == true && r.IsResultSentByAdapter == false);
            //var hdrAltId = row != null ? row.MdrheaderAltId : Guid.Empty;

            if (obj == null)
                return Tuple.Create(model, 0);




            #region messageheader

            ConDecon_MessageHeader_IN rootHeader = new();
            rootHeader.UUID = obj.McsrefUuidbyMdm;// Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.Mcsuuid;//obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.McssenderBusinessSystemIdbySap.Trim();// obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.SpecifyKind(obj.McscreationDatetimeByMdm ?? DateTime.Now, DateTimeKind.Local);

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region UtilitiesConnectionStatusChangeRequest
            UtilitiesConnectionStatusChangeRequest_IN objUtilitiesConnectionStatusChangeRequest = new();

            objUtilitiesConnectionStatusChangeRequest.ID = obj.McschangeRequestId.Trim();
            //obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.ID;
            objUtilitiesConnectionStatusChangeRequest.CategoryCode = obj.McschangeRequestCategoryCode ?? 0; //0 Won;t happen - Wrong
                                                                                                            //obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.CategoryCode;

            DeviceConnectionStatus_IN objDeviceConnectionStatus = new();

            objDeviceConnectionStatus.UtilitiesDeviceConnectionStatusProcessingResultCode = obj.McsutilitiesDeviceConnectionStatusProcessingResultCodeByMdm ?? 0;
            //ResultCode; //1-- Success | 2 - Failure
            objDeviceConnectionStatus.UtilitiesDeviceID = obj.McsutilitiesDeviceId;
            // obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus.UtilitiesDeviceID;
            objDeviceConnectionStatus.ProcessingDateTime = DateTime.SpecifyKind(obj.McsprocessingDateTimeByMdm ?? DateTime.Now, DateTimeKind.Local);
            //obj.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.PlannedProcessingDateTime;

            objUtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus = objDeviceConnectionStatus;
            #endregion

            #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB

            ConDecon_Log ChildLog = new();

            ChildLog.BusinessDocumentProcessingResultCode = obj.McslogBusinessDocumentProcessingResultCodeByMdm ?? 0;// ProcessingResultCode;
            ChildLog.MaximumLogItemSeverityCode = obj.McslogMaximumLogItemSeverityCodeByMdm ?? 0; // LogSeverityCode;

            ConDecon_LogItem ChildLogItem = new();
            ChildLogItem.Note = obj.McslogItemNoteByMdm.Trim();
            ChildLogItem.TypeID = obj.McslogItemTypeIdByMdm.Trim();
            ChildLogItem.SeverityCode = obj.McslogItemSeverityCodeByMdm ?? 0;// ItemSeverityCode;

            ChildLog.Item = ChildLogItem;

            #endregion

            model.MessageHeader = rootHeader;
            model.UtilitiesConnectionStatusChangeRequest = objUtilitiesConnectionStatusChangeRequest;
            model.Log = ChildLog;

            return Tuple.Create(model, Convert.ToInt32(obj.ServiceCallLogId));
        }
        #endregion



        #region OVERIDDEN BY NEW METHOD

        public string ConnectionDisconnectionConfirmationSINGLEOLD()
        {
            _logger.LogInformation("Debug : ConnectionDisconnectionConfirmationSINGLE >> Reached the controller - SAPAdapterWS/SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation_In");
            var UpdateId = 0;
            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation model = null;
            try
            {
                var obj = GetTotalCDCONCollection();

                model = obj.Item1;
                UpdateId = obj.Item2;

            }
            catch (Exception)
            {
                throw;
            }
            // EXIT Condition : Start
            string result = string.Empty;
            if (model == null || UpdateId == 0)
            {
                result = "Object sent is Null";
                return result;
            }
            // EXIT Condition : End

            string? url = _configuration.GetSection(SectionHeader)[keyForConnectionDisconnectionRequestConfirmationSingle];
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
            xml = xml.Replace("SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation", "glob:SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;

            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForConnectionDisconnectionRequestConfirmationSingle, ServiceEnumValueForConnectionDisconnectionRequestConfirmationSingle);
                //Console.WriteLine(result);


                if (!string.IsNullOrEmpty(result) && result.Trim().ToUpper().Contains("OK"))
                {
                    ContextRepository.UpdateIsResultSentByAdapterFlag(_dbContext, UpdateId);
                }

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
    }
}
