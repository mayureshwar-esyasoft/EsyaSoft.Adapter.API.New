using EsyaSoft.Adapter.API.DataProcessor;
using EsyaSoft.Adapter.API.EFModels;
using EsyaSoft.Adapter.API.Utils;
using EsyaSoft.Adapter.Domain.Models;
using EsyaSoft.Adapter.Domain.Models.MeterReadCancelOUT;
using ExtensionMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class MeterReadCancelController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<MeterReadCancelController> _logger;

        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        public string xmlSOAP = string.Empty;
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly string SectionHeader = "EndPoints";

        public MeterReadCancelController(ILogger<MeterReadCancelController> logger, AdapterContext dbContext,
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

        #region Bulk 41 - SAP OUT - Meter Read Cancel

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForBulk = ServiceEnum.MeterReadCancelBulk.ToString();
        private readonly int ServiceEnumValueForBulk = (int)ServiceEnum.MeterReadCancelBulk;
        [Authorize]
        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCancellationRequest_OutService")]
        [Consumes("text/xml")]
        public void MeterReadCancelBulk([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:MeterReadCancelBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCancellationRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;

            if (Data != null)
            {
                try
                {

                    ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                    CancellationRequestBulkRoot obj = null;
                    try
                    {
                        XMLHelper objXML = new XMLHelper();
                        obj = objXML.Deserialize<CancellationRequestBulkRoot>(Data.ToString());
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    ///=====>>> XSS Part End ======
                    // SaveTransaction to DB - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:MeterReadCancelBulk >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:MeterReadCancelBulk: {Data}");
                    //formatter.Serialize()
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : MeterReadCancelBULK >> Data Insert to HEADER and DETAIL Table of Cancellation Request - Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationRequest_OutService");

                    ContextRepository.SaveMeterCancelRequestTransactionBULK(_dbContext, IDval, obj, "BULK", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : MeterReadCancelBULK >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        //MeterReadRequestConfirmationController MeterReadRequestConfirmationControllerObj = new(_loggerConfirmationClass, _dbContext, _configuration);
                        //string confirmationPayload = MeterReadRequestConfirmationControllerObj.MeterReadSingleConfirmationFromReadController(obj);
                        string confirmationPayload = MeterReadCancellationConfirmationBULK(obj);
                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {

                            //Update Confirmation Header
                            //********* TODO
                            //===> Get HeaderALTID with Confirmation Paylaod >> Not Needed - ServiceCallLogID is there
                            //===> Update IsCancellationConfirmationSentToSAP | IsMDMInvokedCancellation | IsCancellationProcessedByMDM for both Header and Child
                            ContextRepository.UpdateMROCancellationFlags(_dbContext, IDval);
                            _logger.LogInformation("Debug:MeterReadCancelBULK >> Updated DB with Final Value 1 - Success");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                            _logger.LogInformation("Debug:MeterReadCancelBULK >> Updated DB with Final Value 1 - Success");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    //Update Success/Failure
                    ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForBulk, ServiceEnumValueForBulk);
                    _logger.LogInformation("Debug:MeterReadCancelBulk >> Updated DB with Final Value 1 - Success");

                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:MeterReadCancelBulk: {ex.Message}");
                    _logger.LogCritical("Debug:MeterReadCancelBulk >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion


        #region SINGLE 40 - SAP OUT - Meter Read Cancel

        //private readonly string SectionHeader = "EndPoints";
        private readonly string keyForSingle = ServiceEnum.MeterReadCancelSingle.ToString();
        private readonly int ServiceEnumValueForSingle = (int)ServiceEnum.MeterReadCancelSingle;

        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationRequest_OutService")]
        [Consumes("text/xml")]
        [Authorize]
        public void MeterReadCancelSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug:ManualMeterReadSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationRequest_OutService");

            string returnVal = string.Empty;
            long IDval = 0;
            Boolean RetStatus = false;
            if (Data != null)
            {
                try
                {
                    ///XSS Check - VAPT - Checking if the Object is desirilizable to ensure any Script is injected or not
                    SmartMeterMeterReadingDocumentERPCancellationRequestRoot obj = null;
                    try
                    {
                        XMLHelper objXML = new XMLHelper();
                        obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCancellationRequestRoot>(Data.ToString());
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    ///=====>>> XSS Part End ======
                    // SaveTransaction to DB - ServiceCallLog - Initiation
                    IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForSingle, ServiceEnumValueForSingle);
                    _logger.LogInformation("Debug:MeterReadCancelSingle >> Saved Into DB with inital Value 0 - Initiation");

                    // Process the received data
                    Console.WriteLine($"Received data:MeterReadCancelSingle: {Data}");
                    // Here you can implement your logic to save the data to your database or perform any other actions
                    if (IDval > 0)
                    {
                        RetStatus = true;
                    }

                    #region serialization & DB Operation
                    _logger.LogInformation("Debug : MeterReadCancelSingle >> Data Insert to HEADER and DETAIL Table of Cancellation Request - Starts: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationRequest_OutService");

                     ContextRepository.SaveMeterCancelRequestTransactionSINGLE(_dbContext, IDval, obj, "SINGLE", keyForSingle, ServiceEnumValueForSingle); //, confirmationPayload);

                    _logger.LogInformation("Debug : MeterReadSingleConfirmation >> Parsing & DB Insert Completed: Serilizaing - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCreateConfirmation_In");

                    #endregion
                    try
                    {
                        //***** Region to Start Creating for Confirmation Payload - START *****************

                        //MeterReadRequestConfirmationController MeterReadRequestConfirmationControllerObj = new(_loggerConfirmationClass, _dbContext, _configuration);
                        //string confirmationPayload = MeterReadRequestConfirmationControllerObj.MeterReadSingleConfirmationFromReadController(obj);
                        string confirmationPayload = MeterReadCancellationConfirmationSingle(obj);
                        //**************************** END ************************************************
                        if (!string.IsNullOrEmpty(confirmationPayload) && confirmationPayload.Trim().ToUpper().Contains("OK"))
                        {
                            //Update Confirmation Header
                            //********* TODO
                            //===> Get HeaderALTID with Confirmation Paylaod >> Not Needed - ServiceCallLogID is there
                            //===> Update IsCancellationConfirmationSentToSAP | IsMDMInvokedCancellation | IsCancellationProcessedByMDM for both Header and Child
                            ContextRepository.UpdateMROCancellationFlags(_dbContext, IDval);
                            _logger.LogInformation("Debug:MeterReadCancelSingle >> Updated DB with Final Value 1 - Success");

                            //Update Success/Failure
                            ContextRepository.updateDBTransaction(_dbContext, IDval, RetStatus, keyForSingle, ServiceEnumValueForSingle);
                            _logger.LogInformation("Debug:MeterReadCancelSingle >> Updated DB with Final Value 1 - Success");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    // Return success response
                    //return Ok("Data submitted successfully. length of submitted data is - " + Data.ToString().Length);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error occurred:MeterReadCancelSingle: {ex.Message}");
                    _logger.LogCritical("Debug:MeterReadCancelSingle >> Error occurred: " + ex.Message.ToString());
                }
            }
            //return StatusCode(500, $"Data is not in correct format");
        }

        #endregion


        #region SAP 43 IN - Meter Read Cancel CONFIRMATION - BULK

        private readonly string keyForBulkCancelConfirmation = ServiceEnum.MeterReadBulkCancellationConfirmationBulk.ToString();
        private readonly int ServiceEnumValueForBulkCancelConfirmation = (int)ServiceEnum.MeterReadBulkCancellationConfirmationBulk;

        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation_In")]
        [Consumes("text/xml")]
        public string MeterReadCancellationConfirmationBULK([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : MeterReadCancellationConfirmationBULK >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation_In");
            CancellationRequestBulkRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<CancellationRequestBulkRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            

            string? url = _configuration.GetSection(SectionHeader)[keyForBulkCancelConfirmation];
            string result = string.Empty;


            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End

            SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation model = new();
            #region messageheader
            var SenderID = obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.SenderBusinessSystemID;
            //SenderID = "ESFT";

            BulkCancellationConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = SenderID;// obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument

            List<SmartMeterMeterReadingDocumentERPCancellationConfirmationMessage> ObjLst = new();


            BulkCancellationConfirmationMessageHeader MessageHeader = null;

            MeterReadingDocumentCancellationConfirmation MeterReadingDocument = null;

            BulkCancellationConfirmationLog ChildLog = null;
            BulkCancellationConfirmationLogItem ChildLogItem = null;


            foreach (var objReading in obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.SmartMeterMeterReadingDocumentERPBulkCancellationRequestMessage)
            {
                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = objReading.MessageHeader.UUID;
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MessageHeader = MessageHeader;
                #endregion


                #region MeterReadingDocument
                MeterReadingDocument = new();

                MeterReadingDocument.ID = objReading.MeterReadingDocument.ID;


                //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
                #endregion



                #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB

                ChildLog = new();

                //ChildLog.BusinessDocumentProcessingResultCode = 5;
                //ChildLog.MaximumLogItemSeverityCode = 3;
                ChildLog.BusinessDocumentProcessingResultCode = 3;
                ChildLog.MaximumLogItemSeverityCode = 1;

                ChildLogItem = new();
                //ChildLogItem.Note = "No Meter Reads Record Found with given Document ID";
                //ChildLogItem.TypeID = "61257";
                //ChildLogItem.SeverityCode = 3;

                ChildLogItem.Note = "Request already processed";
                ChildLogItem.TypeID = "00000";
                ChildLogItem.SeverityCode = 1;

                ChildLog.Item = ChildLogItem;



                #endregion


                ObjLst.Add(new Domain.Models.SmartMeterMeterReadingDocumentERPCancellationConfirmationMessage()
                {
                    MessageHeader = MessageHeader,
                    MeterReadingDocument = MeterReadingDocument,
                    Log = ChildLog
                });

            }
            #endregion

            #region Log
            BulkCancellationConfirmationLog rootLog = new();
            
            rootLog.BusinessDocumentProcessingResultCode = 3;
            rootLog.MaximumLogItemSeverityCode = 1;

            BulkCancellationConfirmationLogItem rootLogItem = new();
            rootLogItem.TypeID = "0000";
            rootLogItem.SeverityCode = 1;
            rootLogItem.Note = "Request Processed Successfully";

            rootLog.Item = rootLogItem;

            //model.Log = rootLog;
            #endregion

            model.MessageHeader = rootHeader;
            model.LstCancellationConfirmationMessage = ObjLst;
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

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForBulkCancelConfirmation, ServiceEnumValueForBulkCancelConfirmation);
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

        public string MeterReadCancellationConfirmationBULK(CancellationRequestBulkRoot obj)
        {
            _logger.LogInformation("Debug : MeterReadCancellationConfirmationBULK >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation_In");
            //CancellationRequestBulkRoot obj = null;
            //try
            //{
            //    XMLHelper objXML = new XMLHelper();
            //    obj = objXML.Deserialize<CancellationRequestBulkRoot>(Data.ToString());
            //}
            //catch (Exception)
            //{

            //    throw;
            //}


            string? url = _configuration.GetSection(SectionHeader)[keyForBulkCancelConfirmation];
            string result = string.Empty;


            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End

            SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation model = new();
            #region messageheader
            var SenderID = obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.SenderBusinessSystemID;
            //SenderID = "ESFT";

            BulkCancellationConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = SenderID;// obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region LIST of MeterDocument

            List<SmartMeterMeterReadingDocumentERPCancellationConfirmationMessage> ObjLst = new();


            BulkCancellationConfirmationMessageHeader MessageHeader = null;

            MeterReadingDocumentCancellationConfirmation MeterReadingDocument = null;

            BulkCancellationConfirmationLog ChildLog = null;
            BulkCancellationConfirmationLogItem ChildLogItem = null;


            foreach (var objReading in obj.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.SmartMeterMeterReadingDocumentERPBulkCancellationRequestMessage)
            {
                //MeterReadingDocumentERPResultCreateConfirmationMessage = new ClsMeterReadingDocumentERPResultCreateConfirmationMessage();
                #region messageHeader
                MessageHeader = new();
                MessageHeader.UUID = Guid.NewGuid();
                MessageHeader.ReferenceUUID = objReading.MessageHeader.UUID;
                MessageHeader.RecipientBusinessSystemID = objReading.MessageHeader.SenderBusinessSystemID.ToString();
                MessageHeader.CreationDateTime = DateTime.Now;

                //MeterReadingDocumentERPResultCreateConfirmationMessage.MessageHeader = MessageHeader;
                #endregion


                #region MeterReadingDocument
                MeterReadingDocument = new();

                MeterReadingDocument.ID = objReading.MeterReadingDocument.ID;


                //MeterReadingDocumentERPResultCreateConfirmationMessage.MeterReadingDocument = MeterReadingDocument;
                #endregion



                #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB

                ChildLog = new();

                //ChildLog.BusinessDocumentProcessingResultCode = 5;
                //ChildLog.MaximumLogItemSeverityCode = 3;
                ChildLog.BusinessDocumentProcessingResultCode = 3;
                ChildLog.MaximumLogItemSeverityCode = 1;

                ChildLogItem = new();
                //ChildLogItem.Note = "No Meter Reads Record Found with given Document ID";
                //ChildLogItem.TypeID = "61257";
                //ChildLogItem.SeverityCode = 3;

                ChildLogItem.Note = "Request already processed";
                ChildLogItem.TypeID = "00000";
                ChildLogItem.SeverityCode = 1;

                ChildLog.Item = ChildLogItem;



                #endregion


                ObjLst.Add(new Domain.Models.SmartMeterMeterReadingDocumentERPCancellationConfirmationMessage()
                {
                    MessageHeader = MessageHeader,
                    MeterReadingDocument = MeterReadingDocument,
                    Log = ChildLog
                });

            }
            #endregion

            #region Log
            BulkCancellationConfirmationLog rootLog = new();

            rootLog.BusinessDocumentProcessingResultCode = 3;
            rootLog.MaximumLogItemSeverityCode = 1;

            BulkCancellationConfirmationLogItem rootLogItem = new();
            rootLogItem.TypeID = "0000";
            rootLogItem.SeverityCode = 1;
            rootLogItem.Note = "Request Processed Successfully";

            rootLog.Item = rootLogItem;

            //model.Log = rootLog;
            #endregion

            model.MessageHeader = rootHeader;
            model.LstCancellationConfirmationMessage = ObjLst;
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

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForBulkCancelConfirmation, ServiceEnumValueForBulkCancelConfirmation);
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
            xml = xml.Replace("<LstCancellationConfirmationMessage>", "");
            xml = xml.Replace("</LstCancellationConfirmationMessage>", "");
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation", "glob:SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation");
        }


        #endregion  


        #region SAP 42 IN - Meter Read Cancel CONFIRMATION - SINGLE

        private readonly string keyForSingleCancelConfirmation = ServiceEnum.MeterReadCancellationConfirmationSingle.ToString();
        private readonly int ServiceEnumValueForSingleCancelConfirmation = (int)ServiceEnum.MeterReadCancellationConfirmationSingle;

        //[HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationConfirmation_In")]
        //[Consumes("text/xml")]
        //public string MeterReadCancellationConfirmationSingle([FromBody] XElement Data)


        [HttpPost("SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationConfirmation_In")]
        [Consumes("text/xml")]
        public string MeterReadCancellationConfirmationSingle([FromBody] XElement Data)
        {
            _logger.LogInformation("Debug : MeterReadCancellationConfirmationSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationConfirmation_In");
            SmartMeterMeterReadingDocumentERPCancellationRequestRoot obj = null;
            try
            {
                XMLHelper objXML = new XMLHelper();
                obj = objXML.Deserialize<SmartMeterMeterReadingDocumentERPCancellationRequestRoot>(Data.ToString());
            }
            catch (Exception)
            {

                throw;
            }


            string? url = _configuration.GetSection(SectionHeader)[keyForSingleCancelConfirmation];
            string result = string.Empty;


            // EXIT Condition : Start
            if (obj == null)
            {
                result = "Object sent is Null";
                return result;
            }

            // EXIT Condition : End


            SmartMeterMeterReadingDocumentERPCancellationConfirmation model = new();
            #region messageheader

            SingleMeterReadingCancellationConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region MeterDocument
            SingleMeterReadingDocumentCancellationConfirmation MeterReadingDocument = new();

            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPCancellationRequest.MeterReadingDocument.ID;

            #endregion

            #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB
            SingleMeterReadingCancellationConfirmationLog ChildLog = new();

            ChildLog.BusinessDocumentProcessingResultCode = 3;
            ChildLog.MaximumLogItemSeverityCode = 1;

            SingleMeterReadingCancellationConfirmationLogItem ChildLogItem = new();
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
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPCancellationConfirmation", "glob:SmartMeterMeterReadingDocumentERPCancellationConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;

            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                result = PostSOAPRequestAsync(url, xmlSOAP, keyForSingleCancelConfirmation, ServiceEnumValueForSingleCancelConfirmation);
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
        private string MeterReadCancellationConfirmationSingle(SmartMeterMeterReadingDocumentERPCancellationRequestRoot obj)
        {
            _logger.LogInformation("Debug : MeterReadCancellationConfirmationSingle >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPCancellationConfirmation_In");
            
            //string? url = _configuration.GetSection(SectionHeader)[keyForSingleCancelConfirmation];
            //string result = string.Empty;

            //***********
            string? url = _configuration.GetSection(SectionHeader)[keyForSingleCancelConfirmation];
            string resultSend = string.Empty;
            // EXIT Condition : Start
            if (obj == null)
            {
                resultSend = "Object sent is Null";
                return resultSend;
            }
            
            // EXIT Condition : End


            SmartMeterMeterReadingDocumentERPCancellationConfirmation model = new();
            #region messageheader

            SingleMeterReadingCancellationConfirmationMessageHeader rootHeader = new();
            rootHeader.UUID = Guid.NewGuid();
            rootHeader.ReferenceUUID = obj.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.UUID;
            rootHeader.RecipientBusinessSystemID = obj.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.SenderBusinessSystemID;
            rootHeader.CreationDateTime = DateTime.Now;

            //assign to main object
            //model.MessageHeader = rootHeader;
            #endregion

            #region MeterDocument
            SingleMeterReadingDocumentCancellationConfirmation MeterReadingDocument = new();

            MeterReadingDocument.ID = obj.SmartMeterMeterReadingDocumentERPCancellationRequest.MeterReadingDocument.ID;

            #endregion

            #region Log Child --- TO DO: NEEDS TO UPDATED FROM DB
            SingleMeterReadingCancellationConfirmationLog ChildLog = new();

            ChildLog.BusinessDocumentProcessingResultCode = 3;
            ChildLog.MaximumLogItemSeverityCode = 1;

            SingleMeterReadingCancellationConfirmationLogItem ChildLogItem = new();
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
            xml = xml.Replace("SmartMeterMeterReadingDocumentERPCancellationConfirmation", "glob:SmartMeterMeterReadingDocumentERPCancellationConfirmation");
            xmlSOAP = xmlSOAPPrefix + xml + xmlSOAPSuffix;

            _logger.LogInformation("Debug >> Preapred Request in SOAP Format to Push into SAP API");

            try
            {
                Console.WriteLine("SAP IN POC - Calling IN Service Anysc...");
                _logger.LogInformation("Debug >>SAP IN POC - Calling IN Service Anysc..");

                resultSend = PostSOAPRequestAsync(url, xmlSOAP, keyForSingleCancelConfirmation, ServiceEnumValueForSingleCancelConfirmation);
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
        //private void EnrichMeterReadCancellationConfirmationXML(ref string xml)
        //{
        //    xml = xml.Replace("<LstCancellationConfirmationMessage>", "");
        //    xml = xml.Replace("</LstCancellationConfirmationMessage>", "");
        //    xml = xml.Replace("SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation", "glob:SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation");
        //}


        #endregion  

        #region private Methods
        private string PostSOAPRequestAsync(string url, string text, string key, int ServiceEnumValue)
        {
            _logger.LogInformation("Debug >> SAP IN POC - Within Post Asnyc Method ...");
            string? AuthCred = _configuration.GetSection("SAPCredential")["BasicAuthKey"];


            Console.WriteLine("SAP IN POC - Within Post Asnyc Method ...");
            string returnVal = string.Empty;
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
                request.Headers.Add("Authorization", AuthCred);
                //request.Headers.Add("Authorization", "Basic dG1waV9kZXY6Z3RkQDEyMw==");

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
