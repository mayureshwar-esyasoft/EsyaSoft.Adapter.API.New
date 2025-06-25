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
using EsyaSoft.Adapter.API.Controllers;
using static EsyaSoft.Adapter.API.Controllers.MeterReadRequestConfirmationController;


namespace EsyaSoft.Adapter.API.JobDirectory.ScheduledJobs.JobsImplementation
{
    public class MeterReads : IMeterReads
    {
        

        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<MeterReads> _logger;

        private string xmlSOAPPrefix = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:glob=""http://sap.com/xi/SAPGlobal20/Global"">
                                        <soapenv:Header/>
                                        <soapenv:Body>";
        private string xmlSOAPSuffix = "</soapenv:Body></soapenv:Envelope>";
        public string xmlSOAP = string.Empty;
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly string SectionHeader = "EndPoints";


        private readonly string keyForBulkResult = ServiceEnum.MeterReadBulkResult.ToString();
        private readonly int ServiceEnumValueForBulkResult = (int)ServiceEnum.MeterReadBulkResult;

        public MeterReads(ILogger<MeterReads> logger, AdapterContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }
        
        public async Task<string> MeterReadBULKRESULTConfirmationTask()
        {
            //// Simulate a long-running background task
            //_logger.LogInformation("Background job started.");
            //await Task.Delay(5000); // Simulate 5 seconds of work
            //_logger.LogInformation("Background job completed.");

            #region Migrated Code
            _logger.LogInformation("Debug : MeterReadRequestConfirmationBulk >> Reached the controller - SAPAdapterWS/SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_In");
            MdrresultHeader ObjResultHdr = null;
            List<MdrresultDetail> LstObjResultDtl = null;

            string? url = _configuration.GetSection(SectionHeader)[keyForBulkResult];
            int SplitSize = Convert.ToInt16(_configuration.GetSection("MROSplit")["SplitSize"]);
            string result = string.Empty;

            SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot obj = null;
            try
            {
                var resultRoot = GetResultValuesForMultipleMROSendForBulk("BULK");
                if (resultRoot != null)
                {
                    //ObjResultHdr = new();
                    //LstObjResultDtl = new();
                    //ObjResultHdr = resultRoot.Item1;
                    //LstObjResultDtl = resultRoot.Item2;
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

            #endregion
        }

        #region Common methods for DB Parsing - SINGLE and BULK Both - RESULT Related
        private List<MROPost> GetResultValuesForMultipleMROSendForBulk(string strPayLoadType)
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
            /* NEWLY ADDED IMPROVE PERFORMANCE - 0801 */
            //var result = _dbContext.MdrresultDetails
            //            .Join(
            //                _dbContext.MdrresultDetails, // Joining with the same table
            //                mrd => mrd.ServiceCallLogId, // Key for the first table
            //                mrh => mrh.ServiceCallLogId, // Key for the second table
            //                (mrd, mrh) => new { mrh } // Select the first table
            //            )
            //            .Where(joined =>
            //                //joined.mrd.MdrresultDetailScheduledMeterReadingDate >= DateTime.Today.AddDays(-1).Date
            //                joined.mrd.IsResultSent == false
            //                //&& joined.mrd.PayloadType == "BULK"
            //            )
            //            .Select(joined => joined.mrd.ServiceCallLogId) // Select distinct ServiceCallLogId
            //            .Distinct() // Remove duplicates
            //            .OrderByDescending(id => id) // Order by descending
            //            .ToList(); // Fetch the result as a list


            /* NEWLY ADDED IMPROVE PERFORMANCE - 0801 */

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

        //private Tuple<MeterReadResultConfirmationBulkMessageHeader, MeterReadingDocumentResult> PopuplateResultDetail(List<MdrresultDetail> LstDetailObject)
        private Tuple<MeterReadResultConfirmationBulkMessageHeader, MeterReadingDocumentResult> PopuplateResultDetail(MdrresultDetail obj)
        {
            //MdrresultDetail obj = LstDetailObject[0];

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
            //IDval = ContextRepository.SaveDBTransaction(_dbContext, Data.ToString(), keyForBulk, ServiceEnumValueForBulk);
            //var uri = new Uri("https://issdev.adanielectricity.com/XISOAPAdapter/MessageServlet?senderParty=InterPO&senderService=MDM_SERV&receiverParty=&receiverService=&interface=MeterReadingDocumentERPResultBulkCreateRequest_In&interfaceNamespace=http://sap.com/xi/IS-U/Global2");
            //var host = uri.Host;
            //var sss = Uri.CheckHostName("https://issdev.adanielectricity.com/XISOAPAdapter/MessageServlet?senderParty=InterPO&senderService=MDM_SERV&receiverParty=&receiverService=&interface=MeterReadingDocumentERPResultBulkCreateRequest_In&interfaceNamespace=http://sap.com/xi/IS-U/Global2");
            IDval = ContextRepository.SaveDBTransaction(_dbContext, text, keyForBulkResult, ServiceEnumValueForBulkResult);
            if (IDval > 0)
            {
                RetStatusDB = true;
            }
            _logger.LogInformation("Debug >> Data Saved into DB for IN Service with Initial Value 0 ...");


            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            //httpClient.DefaultRequestHeaders.Host = 
            //httpClient.DefaultRequestHeaders.AcceptLanguage = "En-Us";
            //httpClient.DefaultRequestHeaders.Host = host;
            using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                //request.Headers.Add("SOAPAction", "");
                //request.Headers.Add("Authorization", "Basic dG1waV9kZXY6Z3RkQDEyMw==");
                request.Headers.Add("Authorization", AuthCred);
                //request.Headers.Add("Host", host);
                //request.Headers.Add("Host123", "Something else");
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
    }
}
