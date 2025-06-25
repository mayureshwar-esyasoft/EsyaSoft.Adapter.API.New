using EsyaSoft.Adapter.API.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using EsyaSoft.Adapter.API.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Net.Http;
using System.Text;
using EsyaSoft.Adapter.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using EsyaSoft.Adapter.Domain.Models.MeterReadCancelOUT;
using System.Reflection.PortableExecutable;
using ExtensionMethod;
using EsyaSoft.Adapter.Domain.Models.RegisterChange.Single;
using System.ServiceModel.Channels;
using Newtonsoft.Json.Linq;
using static Azure.Core.HttpHeader;
using System.Reflection.Metadata;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;


namespace EsyaSoft.Adapter.API.DataProcessor
{
    public static class ContextRepository
    {

        #region Static methods
        public static int updateDBTransaction (AdapterContext _dbContext, long IDval, bool status, string ServiceKey, int ServiceValue)
        {
            int returnVal = 0;
             
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
                            entity.Remark = "OUT call for - " + ServiceKey + " Completed.";

                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();

                            returnVal = 1;
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }

                return returnVal;
            }
            catch (Exception ex)
            {

                throw;
            }
            //throw new NotImplementedException();
        }
        public static long SaveDBTransaction(AdapterContext _dbContext, string txtXML, string ServiceKey, int ServiceValue)
        {
            long returnVal = 0;
            try
            {
                ServiceCallLog obj = new ServiceCallLog()
                {
                    IsSuccess = false,
                    Remark = "OUT call for - " + ServiceKey + " initiated.",
                    ServiceId = ServiceValue,
                    ServiceName = ServiceKey,
                    CallTimings = DateTime.Now,
                    ServiceParamJson = txtXML
                };

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
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
        public static int UpdateConfirmationSent(AdapterContext _dbContext, long IDval, string PayloadType)
        {
            //isConfirmationSent >> MDRHeader, MDRDetail,MDRConfirmationHeader, MDRConfirmationDetail
            int returnVal = 0;

            try
            {
                var entityMDRHeader = _dbContext.Mdrheaders.FirstOrDefault(itm => itm.ServiceCallLogId == IDval);
                var entityMDRMDRDetail = _dbContext.Mdrdetails.FirstOrDefault(itm => itm.MasterServiceCallLogId == IDval);
                var entityMDRConfirmationHeader = _dbContext.MdrconfirmationHeaders.FirstOrDefault(itm => itm.ServiceCallLogId == IDval);
                var entityMDRConfirmationDetail = _dbContext.MdrconfirmationDetails.FirstOrDefault(itm => itm.ServiceCallLogId == IDval);

                // Validate entity is not null
                if (entityMDRHeader != null &&
                    entityMDRMDRDetail != null &&
                    entityMDRConfirmationHeader != null &&
                    entityMDRConfirmationDetail != null )
                {
                    using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //var std = new Student() { StudentName = "Steve" };
                            //entityMDRHeader
                            entityMDRHeader.IsConfirmationSent = true;
                            entityMDRHeader.UpdatedBy = "Adapter-Auto";
                            entityMDRHeader.UpdatedOn = DateTime.Now;

                            //entityMDRMDRDetail
                            entityMDRMDRDetail.IsConfirmationSent = true;

                            //entityMDRConfirmationHeader
                            entityMDRConfirmationHeader.IsConfirmationSent = true;
                            entityMDRConfirmationHeader.UpdatedBy = "Adapter-Auto";
                            entityMDRConfirmationHeader.UpdatedOn = DateTime.Now;

                            //entityMDRConfirmationDetail
                            entityMDRConfirmationDetail.IsConfirmationSent = true;
                            entityMDRConfirmationDetail.UpdatedBy = "Adapter-Auto";
                            entityMDRConfirmationDetail.UpdatedOn = DateTime.Now;

                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();

                            returnVal = 1;
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }

                return returnVal;
            }
            catch (Exception ex)
            {

                throw;
            }
            //throw new NotImplementedException();
        }

        #endregion

        #region UPDATE DB FLAGS after MRO CONFIRMATION SENT - 

        #endregion

        #region 32 & 33 METER DATA READ PARSING AND INSERT - SINGLE and BULK
        /// <summary>
        /// Meter Data Request INSERT for Both SINGLE and BULK
        /// SINGLE - 32
        /// BULK - 33
        /// </summary>
        /// <param name="_dbContext"></param>
        /// <param name="ObjXML"></param>
        /// <param name="payloadType"></param>
        /// <param name="ServiceKey"></param>
        /// <param name="ServiceValue"></param>
        /// <returns></returns>
        public static long SaveMeterReadRequestTransactionSINGLE(AdapterContext _dbContext, long EntryId, SmartMeterMeterReadingDocumentERPCreateRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                Mdrheader hdrObj = new();

                hdrObj.MdrheaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";





                Mdrdetail dtlObj = new();

                dtlObj.MdrdetailAltId = Guid.NewGuid();
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MdrheaderAltId = ThisRecAltId;
                dtlObj.MdrheaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                dtlObj.DetailMhuuid = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.UUID ?? new Guid();
                dtlObj.DetailMhcreationTime = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.CreationDateTime;
                dtlObj.DetailMhsenderBusinessSystemId = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                dtlObj.MeterReadingDocumentId = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.ID;
                dtlObj.MeterReadingReasonCode = Convert.ToInt16(ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.MeterReadingReasonCode);
                dtlObj.ScheduledMeterReadingDate = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.ScheduledMeterReadingDate;
                dtlObj.UtilitiesAdvancedMeteringDataSourceTypeCode = Convert.ToInt16(ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtilitiesAdvancedMeteringDataSourceTypeCode);
                dtlObj.UtilitiesMeasurementTaskId = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesMeasurementTaskID;
                dtlObj.UtilitiesObjectIdentificationSystemCodeText = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText;
                dtlObj.UtilitiesDeviceId = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtiltiesDevice.UtilitiesDeviceID;
                dtlObj.UtilitiesAdvancedMeteringSystemId = ObjXML.SmartMeterMeterReadingDocumentERPCreateRequest.MeterReadingDocument.UtiltiesMeasurementTask.UtiltiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;


                dtlObj.IsItemProcessed = false;
                dtlObj.IsConfirmationSent = false;
                dtlObj.IsMdminvoked = false;


                //**** ===>> This Space for Confirmation Payload Insertion in [MDRConfirmationHeader] Table
                //**************>>> This Payload is created at the time of Request Parsing But not Sent
                //**************>>> Will be sent Once Confirmation Controller is Invoked
                MdrconfirmationHeader ObjConfirmHdr = new();

                ObjConfirmHdr.MdrconfirmationHeaderAltId = Guid.NewGuid();
                ObjConfirmHdr.MdrheaderId = 1;  //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID
                ObjConfirmHdr.MdrheaderAltId = ThisRecAltId;
                ObjConfirmHdr.ServiceCallLogId = EntryId;
                ObjConfirmHdr.PayloadType = payloadType.ToUpper().Trim();
                ObjConfirmHdr.ConfirmationPayload = string.Empty; //==>> Initially Insert NULL, Then Update at the time of Updation;

                ObjConfirmHdr.IsConfirmationSent = false; //At this Stage - Confirmation is not Sent

               ObjConfirmHdr.CreatedOn = DateTime.Now;
               ObjConfirmHdr.UpdatedOn = DateTime.Now;
               ObjConfirmHdr.CreatedBy = "MDRParser";
               ObjConfirmHdr.UpdatedBy = "MDRParser";

                //MDRParser - IServerSideBlazorBuilder responsoble to create this Payload and Save

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        _dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MdrheaderId;

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


        /// <summary>
        /// Bulk Payload for Meter data Read
        /// </summary>
        /// <param name="_dbContext"></param>
        /// <param name="EntryId"></param>
        /// <param name="ObjXML"></param>
        /// <param name="payloadType"></param>
        /// <param name="ServiceKey"></param>
        /// <param name="ServiceValue"></param>
        /// <returns></returns>
        public static long SaveMeterReadRequestTransactionBULK(AdapterContext _dbContext, long EntryId, SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                Mdrheader hdrObj = new();

                hdrObj.MdrheaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.Mhuuid = Guid.Parse(ObjXML.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.UUID);
                hdrObj.MhcreationDatetime = ObjXML.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.CreationDateTime;
                hdrObj.SenderSystemBusinessId = ObjXML.SmartMeterMeterReadingDocumentERPBulkCreateRequest.MessageHeader.SenderBusinessSystemID;

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;

                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                #region LIST of MeterDocument
                
                Mdrdetail dtlObj = null;
                foreach (var objReading in ObjXML.SmartMeterMeterReadingDocumentERPBulkCreateRequest.SmartMeterMeterReadingDocumentERPCreateRequestMessage)
                {
                    dtlObj = new();
                    
                    dtlObj.MdrdetailAltId = Guid.NewGuid();
                    dtlObj.MasterServiceCallLogId = EntryId;
                    dtlObj.MdrheaderAltId = ThisRecAltId;
                    dtlObj.MdrheaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                    dtlObj.DetailMhuuid = Guid.Parse(objReading.MessageHeader.UUID) ;
                    dtlObj.DetailMhcreationTime = objReading.MessageHeader.CreationDateTime;
                    dtlObj.DetailMhsenderBusinessSystemId = objReading.MessageHeader.SenderBusinessSystemID.ToString();

                    dtlObj.MeterReadingDocumentId = objReading.MeterReadingDocument.ID;
                    dtlObj.MeterReadingReasonCode = Convert.ToInt16(objReading.MeterReadingDocument.MeterReadingReasonCode);
                    dtlObj.ScheduledMeterReadingDate = objReading.MeterReadingDocument.ScheduledMeterReadingDate;
                    dtlObj.UtilitiesAdvancedMeteringDataSourceTypeCode = Convert.ToInt16(objReading.MeterReadingDocument.UtilitiesAdvancedMeteringDataSourceTypeCode);
                    dtlObj.UtilitiesMeasurementTaskId = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesMeasurementTaskID;
                    dtlObj.UtilitiesObjectIdentificationSystemCodeText = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtilitiesObjectIdentificationSystemCodeText;
                    dtlObj.UtilitiesDeviceId = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtiltiesDevice.UtilitiesDeviceID;
                    dtlObj.UtilitiesAdvancedMeteringSystemId = objReading.MeterReadingDocument.UtiltiesMeasurementTask.UtiltiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;


                    dtlObj.IsItemProcessed = false;
                    dtlObj.IsConfirmationSent = false;
                    dtlObj.IsMdminvoked = false;

                    //--Add to Context
                    _dbContext.Add(dtlObj);
                }
                #endregion

                
                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MdrheaderId;

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


        #region 34 & 35 SEND CONFIRMATION TO SAP 

        #endregion

        #region 36 & 37 - RESULT for SINGLE and BULK Respectively
        public static long GetResultFromDB(AdapterContext _dbContext, string PayloadType)
        {




            return 1;
        }
        #endregion

        #region MeterCancel - 40 and 41 - Save SAP OUT Requests
        
        /// <summary>
        /// This is for READ CANCEL - SINGLE - 40
        /// </summary>
        /// <param name="_dbContext"></param>
        /// <param name="EntryId"></param>
        /// <param name="ObjXML"></param>
        /// <param name="payloadType"></param>
        /// <param name="ServiceKey"></param>
        /// <param name="ServiceValue"></param>
        /// <returns></returns>
        public static long SaveMeterCancelRequestTransactionSINGLE(AdapterContext _dbContext, long EntryId, 
            SmartMeterMeterReadingDocumentERPCancellationRequestRoot ObjXML, string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MdrcancelHeader hdrObj = new();

                hdrObj.MdrcancelHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsCancellationConfirmationSentToSap = false;
                hdrObj.IsMdminvokedCancellation = false;
                hdrObj.IsCancellationProcessedByMdm = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";


                MdrcancelDetail dtlObj = new();
                

                dtlObj.MdrcancelDetailAltId = Guid.NewGuid();
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MdrcancelHeaderAltId = ThisRecAltId;
                dtlObj.MdrcancelHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID
                dtlObj.PayloadType = payloadType.ToUpper().Trim();

                dtlObj.CancelDetailUuid = ObjXML.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.UUID ?? new Guid();
                dtlObj.CancelDetailMhcreationTime = ObjXML.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.CreationDateTime;
                dtlObj.CancelDetailMhsenderBusinessSystemId = ObjXML.SmartMeterMeterReadingDocumentERPCancellationRequest.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                var MeterDocId = ObjXML.SmartMeterMeterReadingDocumentERPCancellationRequest.MeterReadingDocument.ID;
                dtlObj.MeterReadingDocumentId = MeterDocId;
                dtlObj.UtilitiesAdvancedMeteringSystemId = ObjXML.SmartMeterMeterReadingDocumentERPCancellationRequest.MeterReadingDocument.UtilitiesDeviceSmartMeter.UtilitiesAdvancedMeteringSystemID;

                dtlObj.IsCancellationConfirmationSentToSap = false;
                dtlObj.IsMdminvokedCancellation = false;
                dtlObj.IsCancellationProcessedByMdm = false;

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);

                        //****** Update Cancellation Flag *********//
                        Mdrdetail dtlMdr = (from x in _dbContext.Mdrdetails
                                            where x.MeterReadingDocumentId.Trim() == MeterDocId.Trim()
                                            select x).First();
                        dtlMdr.IsCancelled = true;
                        //****** Update Cancellation Flag *********//

                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MdrcancelHeaderId;

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

        /// <summary>
        /// This is for READ CANCEL - BULK - 41
        /// </summary>
        /// <param name="_dbContext"></param>
        /// <param name="EntryId"></param>
        /// <param name="ObjXML"></param>
        /// <param name="payloadType"></param>
        /// <param name="ServiceKey"></param>
        /// <param name="ServiceValue"></param>
        /// <returns></returns>
        public static long SaveMeterCancelRequestTransactionBULK(AdapterContext _dbContext, long EntryId, CancellationRequestBulkRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue)
        {
            List<string> MeterDocIds =  new();
            long returnVal = 0;
            try
            {
                MdrcancelDetail dtlObj = null;

                Guid ThisRecAltId = Guid.NewGuid();
                MdrcancelHeader hdrObj = new();

                hdrObj.MdrcancelHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.Mcuuid = ObjXML.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.UUID ;
                hdrObj.MccreationDatetime = ObjXML.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.CreationDateTime;
                hdrObj.McsenderSystemBusinessId = ObjXML.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.MessageHeader.SenderBusinessSystemID;

                hdrObj.IsCancellationConfirmationSentToSap = false;
                hdrObj.IsMdminvokedCancellation = false;
                hdrObj.IsCancellationProcessedByMdm = false;
                hdrObj.IsProcessCompleted = false;

                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                #region LIST of MeterDocument

               
                foreach (var objReading in ObjXML.SmartMeterMeterReadingDocumentERPBulkCancellationRequest.SmartMeterMeterReadingDocumentERPBulkCancellationRequestMessage)
                {
                    dtlObj = new();

                    dtlObj.MdrcancelDetailAltId = Guid.NewGuid();
                    dtlObj.MasterServiceCallLogId = EntryId;
                    dtlObj.MdrcancelHeaderAltId = ThisRecAltId;
                    dtlObj.MdrcancelHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                    dtlObj.CancelDetailUuid = objReading.MessageHeader.UUID ?? Guid.NewGuid(); //**==> New Guid is a Wrong Case
                    dtlObj.CancelDetailMhcreationTime = objReading.MessageHeader.CreationDateTime;
                    dtlObj.CancelDetailMhsenderBusinessSystemId = objReading.MessageHeader.SenderBusinessSystemID.ToString();

                    dtlObj.MeterReadingDocumentId = objReading.MeterReadingDocument.ID;
                    dtlObj.UtilitiesAdvancedMeteringSystemId = objReading.MeterReadingDocument.UtilitiesDeviceSmartMeter.UtilitiesAdvancedMeteringSystemID;


                    dtlObj.IsCancellationConfirmationSentToSap = false;
                    dtlObj.IsMdminvokedCancellation = false;
                    dtlObj.IsCancellationProcessedByMdm = false;


                    MeterDocIds.Add(objReading.MeterReadingDocument.ID.Trim());
                    //--Add to Context
                    _dbContext.Add(dtlObj);
                }
                #endregion

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //**** >> Updated Cancelled MeterIds
                        //var Ids = (from x in _dbContext.Mdrdetails
                        //           where MeterDocIds.Contains(x.MeterReadingDocumentId.Trim())
                        //           select x).ToList();

                        foreach (var id in MeterDocIds)
                        {
                            Mdrdetail dtlMdr = (from x in _dbContext.Mdrdetails
                                                where x.MeterReadingDocumentId.Trim() == id.Trim()
                                                select x).First();
                            dtlMdr.IsCancelled = true;
                        }

                        //*********************************
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MdrcancelHeaderId;

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

        #region Meter Connect and Disconnect Request - 24 and 26 - Save SAP OUT Requests

        //****>> Only Single Request is Implemented for Now
        public static long MeterStatusChangeRequestSINGLE(AdapterContext _dbContext, long EntryId,
            SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequestRoot ObjXML, string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeterConnectionStatusSingle Obj = new();

                Obj.MeterConnectionStatusSingleAltId = ThisRecAltId;
                Obj.ServiceCallLogId = EntryId;
                Obj.PayloadType = payloadType.Trim().ToUpper();
                
                Obj.Mcsuuid = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.UUID;
                Obj.McscreationDatetimeBySap = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.CreationDateTime;
                Obj.McssenderBusinessSystemIdbySap = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.MessageHeader.SenderBusinessSystemID;

                Obj.McschangeRequestId = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.ID;
                Obj.McschangeRequestCategoryCode = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.CategoryCode;
                Obj.McsplannedProcessingDateTimeBySap = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.PlannedProcessingDateTime;   
                
                Obj.McsutilitiesDeviceId 
                    = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus.UtilitiesDeviceID;

                Obj.McsutilitiesAdvancedMeteringSystemId 
                    = ObjXML.SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest.UtilitiesConnectionStatusChangeRequest.DeviceConnectionStatus.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                Obj.CreatedBy = "FromSAP";
                Obj.CreatedOn = DateTime.Now;
                Obj.UpdatedBy = "Initial";
                Obj.UpdatedOn = DateTime.Now;

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(Obj);
                        _dbContext.SaveChanges();

                        returnVal = Obj.MeterConnectionStatusSingleId;

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

        //Phase2.0 Region >>> STARTED
        #region 1 & 3 METER CREATE PARSING AND INSERT - SINGLE [1] and BULK [3 - ON HOLD] 
        //UtilitiesDeviceERPSmartMeterCreateRequest
        public static long SaveMeterCreateRequestSINGLE(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterCreateRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeterCreateRequestHeader hdrObj = new();

                hdrObj.MeterCreateRequestHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";





                MeterCreateRequestDetail dtlObj = new();

                dtlObj.MeterCreateRequestDetailAltId = Guid.NewGuid();
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MeterCreateRequestHeaderAltId = ThisRecAltId;
                dtlObj.MeterCreateRequestHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                dtlObj.MeterCreateRequestDetailMhuuid = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.MessageHeader.UUID ?? new Guid();
                dtlObj.MeterCreateRequestDetailMhcreationTime = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.MessageHeader.CreationDateTime;
                dtlObj.MeterCreateRequestDetailMhsenderBusinessSystemId = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                dtlObj.UtilitiesDeviceId = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.ID;
                dtlObj.UtilitiesDeviceStartDate = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.StartDate.ToDateOnly();
                dtlObj.UtilitiesDeviceEndDate =  ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.EndDate.ToDateOnly();
                dtlObj.UtilitiesDeviceSerialId = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.SerialID;
                dtlObj.UtilitiesDeviceMaterialId = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.MaterialID;
                dtlObj.PartyInternalId = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.IndividualMaterialManufacturerInformation.PartyInternalID;
                dtlObj.UtilitiesAdvancedMeteringSystemId = ObjXML.UtilitiesDeviceERPSmartMeterCreateRequest.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                dtlObj.IsItemProcessed = false;
                dtlObj.IsConfirmationSent = false;
                dtlObj.IsMdminvoked = false;


                //**** ===>> This Space for Confirmation Payload Insertion in [MDRConfirmationHeader] Table
                //**************>>> This Payload is created at the time of Request Parsing But not Sent
                //**************>>> Will be sent Once Confirmation Controller is Invoked
                MeterCreateConfirmationHeader ObjConfirmHdr = new();

                ObjConfirmHdr.MeterCreateConfirmationHeaderAltId = Guid.NewGuid();
                ObjConfirmHdr.MeterCreateRequestHeaderId = 1;  //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID
                ObjConfirmHdr.MeterCreateRequestHeaderAltId = ThisRecAltId;
                ObjConfirmHdr.ServiceCallLogId = EntryId;
                ObjConfirmHdr.PayloadType = payloadType.ToUpper().Trim();
                ObjConfirmHdr.ConfirmationPayload = string.Empty; //==>> Initially Insert NULL, Then Update at the time of Updation;

                ObjConfirmHdr.IsConfirmationSent = false; //At this Stage - Confirmation is not Sent

                ObjConfirmHdr.CreatedOn = DateTime.Now;
                ObjConfirmHdr.UpdatedOn = DateTime.Now;
                ObjConfirmHdr.CreatedBy = "MeterCreateParser";
                ObjConfirmHdr.UpdatedBy = "MeterCreateParser";

                //MDRParser - IServerSideBlazorBuilder responsoble to create this Payload and Save

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        _dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterCreateRequestHeaderId;

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

        #region 7 & 9 METER CHANGE PARSING AND INSERT - SINGLE [7] and BULK [9 - ON HOLD] 
        //UtilitiesDeviceERPSmartMeterCreateRequest
        public static long SaveMeterChangeRequestSINGLE(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterChangeRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeterChangeRequestHeader hdrObj = new();

                hdrObj.MeterChangeRequestHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";





                MeterChangeRequestDetail dtlObj = new();

                dtlObj.MeterChangeRequestDetailAltId = Guid.NewGuid();
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MeterChangeRequestHeaderAltId = ThisRecAltId;
                dtlObj.MeterChangeRequestHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                dtlObj.MeterChangeRequestDetailMhuuid = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.MessageHeader.UUID;
                dtlObj.MeterChangeRequestDetailMhcreationTime = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.MessageHeader.CreationDateTime;
                dtlObj.MeterChangeRequestDetailMhsenderBusinessSystemId = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                dtlObj.UtilitiesDeviceId = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.UtilitiesDevice.ID;
                dtlObj.UtilitiesDeviceSerialId = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.UtilitiesDevice.SerialID;
                dtlObj.UtilitiesDeviceMaterialId = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.UtilitiesDevice.MaterialID;
                dtlObj.PartyInternalId = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.UtilitiesDevice.IndividualMaterialManufacturerInformation.PartyInternalID;
                dtlObj.UtilitiesAdvancedMeteringSystemId = ObjXML.UtilitiesDeviceERPSmartMeterChangeRequest.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                dtlObj.IsItemProcessed = false;
                dtlObj.IsConfirmationSent = false;
                dtlObj.IsMdminvoked = false;


                //**** ===>> This Space for Confirmation Payload Insertion in [MDRConfirmationHeader] Table
                //**************>>> This Payload is created at the time of Request Parsing But not Sent
                //**************>>> Will be sent Once Confirmation Controller is Invoked
                MeterChangeConfirmationHeader ObjConfirmHdr = new();

                ObjConfirmHdr.MeterChangeConfirmationHeaderAltId = Guid.NewGuid();
                ObjConfirmHdr.MeterChangeRequestHeaderId = 1;  //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID
                ObjConfirmHdr.MeterChangeRequestHeaderAltId = ThisRecAltId;
                ObjConfirmHdr.ServiceCallLogId = EntryId;
                ObjConfirmHdr.PayloadType = payloadType.ToUpper().Trim();
                ObjConfirmHdr.ConfirmationPayload = string.Empty; //==>> Initially Insert NULL, Then Update at the time of Updation;

                ObjConfirmHdr.IsConfirmationSent = false; //At this Stage - Confirmation is not Sent

                ObjConfirmHdr.CreatedOn = DateTime.Now;
                ObjConfirmHdr.UpdatedOn = DateTime.Now;
                ObjConfirmHdr.CreatedBy = "MeterChangeParser";
                ObjConfirmHdr.UpdatedBy = "MeterChangeParser";

                //MDRParser - IServerSideBlazorBuilder responsoble to create this Payload and Save

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        _dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterChangeRequestHeaderId;

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

        public static long SaveMeterChangeRequestBULK(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterBulkChangeRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeterChangeRequestHeader hdrObj = new();

                hdrObj.MeterChangeRequestHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.MchangeRmessageUuid = ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.MessageHeader.UUID;
                hdrObj.MchangeRcreationDatetime = ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.MessageHeader.CreationDateTime;
                hdrObj.MchangeRsenderSystemBusinessId = ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.MessageHeader.SenderBusinessSystemID;

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                #region LIST of Messages

                MeterChangeRequestDetail dtlObj = null;
                foreach (var objMessage in ObjXML.UtilitiesDeviceERPSmartMeterBulkChangeRequest.UtilitiesDeviceERPSmartMeterChangeRequestMessage)
                {

                    dtlObj = new();

                    dtlObj.MeterChangeRequestDetailAltId = Guid.NewGuid();
                    dtlObj.MasterServiceCallLogId = EntryId;
                    dtlObj.MeterChangeRequestHeaderAltId = ThisRecAltId;
                    dtlObj.MeterChangeRequestHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                    dtlObj.MeterChangeRequestDetailMhuuid = objMessage.MessageHeader.UUID;
                    dtlObj.MeterChangeRequestDetailMhcreationTime = objMessage.MessageHeader.CreationDateTime;
                    dtlObj.MeterChangeRequestDetailMhsenderBusinessSystemId = objMessage.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                    dtlObj.UtilitiesDeviceId = objMessage.UtilitiesDevice.ID;
                    dtlObj.UtilitiesDeviceSerialId = objMessage.UtilitiesDevice.SerialID;
                    dtlObj.UtilitiesDeviceMaterialId = objMessage.UtilitiesDevice.MaterialID;
                    dtlObj.PartyInternalId = objMessage.UtilitiesDevice.IndividualMaterialManufacturerInformation.PartyInternalID;
                    dtlObj.UtilitiesAdvancedMeteringSystemId = objMessage.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                    dtlObj.IsItemProcessed = false;
                    dtlObj.IsConfirmationSent = false;
                    dtlObj.IsMdminvoked = false;

                    //--Add to Context
                    _dbContext.Add(dtlObj);
                }




                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        //_dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterChangeRequestHeaderId;

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


        #region REGISER CREATE PARSING AND INSERT - SINGLE [13] and BULK [15]

        public static long SaveMeterRegisterCreateRequestSINGLE(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterRegisterCreateRequestRoot ObjXML,
       string payloadType, string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {
                Guid ThisRecAltId = Guid.NewGuid();
                Guid ThisDetailAltId = Guid.NewGuid();

                MeterRegisterHeader hdrObj = new();

                hdrObj.MeterRegisterHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();
                hdrObj.RegisterType = "CREATE";

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";


                //---- DEtail Fill Up
                MeterRegisterDetail dtlObj = new();

                dtlObj.MeterRegisterDetailAltId = ThisDetailAltId;
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MeterRegisterHeaderAltId = ThisRecAltId;
                dtlObj.MeterRegisterHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                dtlObj.PayloadType = payloadType.ToUpper().Trim();
                dtlObj.RegisterType = "CREATE";

                //dtlObj.ReferenceServiceCallLogId = 1;  //This is Dummy Value - Required to Track Corrsponding Create

                dtlObj.MeterRegisterDetailMhuuid = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.MessageHeader.UUID;
                dtlObj.MeterRegisterDetailMhcreationTime = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.MessageHeader.CreationDateTime;
                dtlObj.MeterRegisterDetailMhsenderBusinessSystemId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                dtlObj.UtilitiesDeviceId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.UtilitiesDevice.ID;

                //dtlObj.RegisterListCompleteTransmissionIndicatorBool = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.UtilitiesDevice.RegisterListCompleteTransmissionIndicator ? 1 : 0;

                #region Register Detail
                MeterCreateChangeRegisterDetail objRegister = null;
                var objRegisterList = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.UtilitiesDevice.Register;
                if (objRegisterList != null)
                {
                    string JsonList = string.Empty;
                    if (!objRegisterList.ToString().Contains("["))
                        JsonList = "[" + objRegisterList.ToString() + "]";
                    else
                        JsonList = objRegisterList.ToString();

                    //var objRList = JsonConvert.DeserializeObject<List<Register>>("[" + objRegisterList.ToString() +"]");
                    var objRList = JsonConvert.DeserializeObject<List<Register>>(JsonList);
                    foreach (Register objR in objRList)
                    {
                        //objR = JsonConvert.DeserializeObject<Register>(reg.ToString());

                        objRegister = new();
                        objRegister.MeterCreateChangeRegisterDetailAltId = Guid.NewGuid();
                        objRegister.ReferenceDetailId = 1; //Dummy Value
                        objRegister.ReferenceDetailAltId = ThisDetailAltId;
                        objRegister.MasterServiceCallLogId = EntryId;
                        objRegister.PayloadType = "SINGLE";
                        objRegister.RegisterSourceType = "RegisterCreateSingle";

                        //objRegister.RegisterDeviceAssignmentListCompleteTransmissionIndicator = objR.DeviceAssignmentListCompleteTransmissionIndicator;
                        objRegister.RegisterStartDate = objR.StartDate.ToDateOnly();
                        objRegister.RegisterEndDate = objR.EndDate.ToDateOnly();
                        objRegister.UtilitiesMeasurementTaskId = objR.UtilitiesMeasurementTaskID.ToString();
                        objRegister.UtilitiesObjectIdentificationSystemCodeText = objR.UtilitiesObjectIdentificationSystemCodeText.ToString();
                        objRegister.UtiltiesMeasurementTaskCategoryCode = Convert.ToInt32(objR.UtilitiesMeasurementTaskCategoryCode);
                        objRegister.UtilitiesDivisionCategoryCode = Convert.ToInt32(objR.UtilitiesDivisionCategoryCode);
                        objRegister.TimeZoneCode = objR.TimeZoneCode;

                        objRegister.SpecMeasureUnitCode = objR.Specifications.MeasureUnitCode;

                        objRegister.DecimalValuePrecisionTotalDigitNumberValue = objR.Specifications.DecimalValuePrecision.TotalDigitNumberValue;
                        //--> Not in Change: objRegister.DecimalValuePrecisionFractionDigitNumberValue = objR.Specifications.DecimalValuePrecision.FractionDigitNumberValue;
                        objRegister.SpecMeterReadingResultAdjustmentFactorValue = (decimal)objR.Specifications.MeterReadingResultAdjustmentFactorValue;


                        objRegister.IsItemProcessed = false;
                        objRegister.IsConfirmationSent = false;
                        objRegister.IsMdminvoked = false;
                        objRegister.IsActive = true;
                        objRegister.IsCancelled = false;

                        //=========Add to Context
                        _dbContext.Add(objRegister);
                    }
                }
                #endregion


                dtlObj.UtilitiesAdvancedMeteringSystemId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterCreateRequest.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                dtlObj.IsItemProcessed = false;
                dtlObj.IsConfirmationSent = false;
                dtlObj.IsMdminvoked = false;

                //----Register Specific DEtail Fill Up

                //---- TO DO ----


                //**** ===>> This Space for Confirmation Payload Insertion in [MDRConfirmationHeader] Table
                //**************>>> This Payload is created at the time of Request Parsing But not Sent
                //**************>>> Will be sent Once Confirmation Controller is Invoked
                MeterRegisterCreateConfirmationHeader ObjConfirmHdr = new();

                ObjConfirmHdr.MeterRegisterCreateConfirmationHeaderAltId = Guid.NewGuid();
                ObjConfirmHdr.MeterRegisterCreateHeaderId = 1;  //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID
                ObjConfirmHdr.MeterRegisterCreateHeaderAltId = ThisRecAltId;
                ObjConfirmHdr.ServiceCallLogId = EntryId;
                ObjConfirmHdr.PayloadType = payloadType.ToUpper().Trim();
                ObjConfirmHdr.ConfirmationPayload = string.Empty; //==>> Initially Insert NULL, Then Update at the time of Updation;

                ObjConfirmHdr.IsConfirmationSent = false; //At this Stage - Confirmation is not Sent

                ObjConfirmHdr.CreatedOn = DateTime.Now;
                ObjConfirmHdr.UpdatedOn = DateTime.Now;
                ObjConfirmHdr.CreatedBy = "RegisterCreateParser";
                ObjConfirmHdr.UpdatedBy = "RegisterCreateParser";

                //MDRParser - IServerSideBlazorBuilder responsoble to create this Payload and Save

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        _dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterRegisterHeaderId;

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
            catch (Exception ex)
            {

                throw;
            }
        }

        public static long SaveMeterRegisterCreateRequestBULK(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {
                Guid ThisRecAltId = Guid.NewGuid();
                MeterRegisterCreateHeader hdrObj = new();

                hdrObj.MeterRegisterCreateHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                #region LIST of MeterDocument
                /*
                MeterRegisterCreateDetail dtlObj = null;
                foreach (var objReading in ObjXML.UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequest.UtilitiesDeviceERPSmartMeterRegisterCreateRequestMessage)
                {
                    dtlObj = new();
                    dtlObj.MeterRegisterCreateDetailAltId = Guid.NewGuid();
                    dtlObj.MasterServiceCallLogId = EntryId;
                    dtlObj.MeterRegisterCreateHeaderAltId = ThisRecAltId;
                    dtlObj.MeterRegisterCreateHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                    dtlObj.MeterRegisterCreateDetailMhuuid = objReading.MessageHeader.UUID;
                    dtlObj.MeterRegisterCreateDetailMhcreationTime = objReading.MessageHeader.CreationDateTime;
                    dtlObj.MeterRegisterCreateDetailMhsenderBusinessSystemId = objReading.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                    dtlObj.UtilitiesDeviceId = objReading.UtilitiesDevice.ID;
                    dtlObj.RegisterStartDate = objReading.UtilitiesDevice.Register.StartDate.ToDateOnly();
                    dtlObj.RegisterEndDate = objReading.UtilitiesDevice.Register.EndDate.ToDateOnly();
                    dtlObj.UtilitiesMeasurementTaskId = objReading.UtilitiesDevice.Register.UtilitiesMeasurementTaskID;
                    dtlObj.UtilitiesObjectIdentificationSystemCodeText = objReading.UtilitiesDevice.Register.UtilitiesObjectIdentificationSystemCodeText;
                    dtlObj.UtiltiesMeasurementTaskCategoryCode = Convert.ToInt16(objReading.UtilitiesDevice.Register.UtiltiesMeasurementTaskCategoryCode);
                    dtlObj.UtilitiesDivisionCategoryCode = Convert.ToInt16(objReading.UtilitiesDevice.Register.UtilitiesDivisionCategoryCode);
                    dtlObj.TimeZoneCode = objReading.UtilitiesDevice.Register.TimeZoneCode;
                    dtlObj.SpecMeasureUnitCode = objReading.UtilitiesDevice.Register.Specifications.MeasureUnitCode;
                    dtlObj.DecimalValuePrecisionTotalDigitNumberValue = objReading.UtilitiesDevice.Register.Specifications.DecimalValuePrecision.TotalDigitNumberValue;
                    dtlObj.SpecMeterReadingResultAdjustmentFactorValue = (decimal)objReading.UtilitiesDevice.Register.Specifications.MeterReadingResultAdjustmentFactorValue;
                    dtlObj.UtilitiesAdvancedMeteringSystemId = objReading.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                    dtlObj.IsItemProcessed = false;
                    dtlObj.IsConfirmationSent = false;
                    dtlObj.IsMdminvoked = false;

                    //--Add to Context
                    _dbContext.Add(dtlObj);
                }
                */
                #endregion


                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterRegisterCreateHeaderId;

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

        #region REGISER CHANGE PARSING AND INSERT - SINGLE [13] and BULK [15]

        public static long SaveMeterRegisterChangeRequestSINGLE(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterRegisterChangeRequestRoot ObjXML,
       string payloadType, string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                Guid ThisDetailAltId = Guid.NewGuid();

                MeterRegisterHeader hdrObj = new();

                hdrObj.MeterRegisterHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();
                hdrObj.RegisterType = "CHANGE";

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";


                //---- DEtail Fill Up
                MeterRegisterDetail dtlObj = new();

                dtlObj.MeterRegisterDetailAltId = ThisDetailAltId;
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MeterRegisterHeaderAltId = ThisRecAltId;
                dtlObj.MeterRegisterHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                dtlObj.PayloadType = payloadType.ToUpper().Trim();
                dtlObj.RegisterType = "CHANGE";

                //dtlObj.ReferenceServiceCallLogId = 1;  //This is Dummy Value - Required to Track Corrsponding Create

                dtlObj.MeterRegisterDetailMhuuid = Guid.Parse(ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.MessageHeader.UUID);
                dtlObj.MeterRegisterDetailMhcreationTime = ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.MessageHeader.CreationDateTime;
                dtlObj.MeterRegisterDetailMhsenderBusinessSystemId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                dtlObj.UtilitiesDeviceId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.UtilitiesDevice.ID;

                dtlObj.RegisterListCompleteTransmissionIndicatorBool = ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.UtilitiesDevice.RegisterListCompleteTransmissionIndicator ? 1 : 0;

                #region Register Detail
                MeterCreateChangeRegisterDetail objRegister = null;
                var objRegisterList = ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.UtilitiesDevice.Register;
                if (objRegisterList != null)
                {
                    string JsonList = string.Empty;
                    if (!objRegisterList.ToString().Contains("["))
                        JsonList = "[" + objRegisterList.ToString() + "]";
                    else
                        JsonList = objRegisterList.ToString();

                    //var objRList = JsonConvert.DeserializeObject<List<Register>>("[" + objRegisterList.ToString() +"]");
                    var objRList = JsonConvert.DeserializeObject<List<Register>>(JsonList);

                    foreach (Register objR in objRList)
                    {
                        //objR = JsonConvert.DeserializeObject<Register>(reg.ToString());

                        objRegister = new();
                        objRegister.MeterCreateChangeRegisterDetailAltId = Guid.NewGuid();
                        objRegister.ReferenceDetailId = 1; //Dummy Value
                        objRegister.ReferenceDetailAltId = ThisDetailAltId;
                        objRegister.MasterServiceCallLogId = EntryId;
                        objRegister.PayloadType = "SINGLE";
                        objRegister.RegisterSourceType = "RegisterChangeSingle";

                        //objRegister.RegisterDeviceAssignmentListCompleteTransmissionIndicator = objR.DeviceAssignmentListCompleteTransmissionIndicator;
                        objRegister.RegisterStartDate = objR.StartDate.ToDateOnly();
                        objRegister.RegisterEndDate = objR.EndDate.ToDateOnly();
                        objRegister.UtilitiesMeasurementTaskId = objR.UtilitiesMeasurementTaskID.ToString();
                        objRegister.UtilitiesObjectIdentificationSystemCodeText = objR.UtilitiesObjectIdentificationSystemCodeText.ToString();
                        objRegister.UtiltiesMeasurementTaskCategoryCode = Convert.ToInt32(objR.UtilitiesMeasurementTaskCategoryCode);
                        objRegister.UtilitiesDivisionCategoryCode = Convert.ToInt32(objR.UtilitiesDivisionCategoryCode);
                        objRegister.TimeZoneCode = objR.TimeZoneCode;

                        objRegister.SpecStartDate = objR.Specifications.StartDate.ToDateOnly();//only for change
                        objRegister.SpecEndDate = objR.Specifications.EndDate.ToDateOnly();//only for change
                        objRegister.SpecMeasureUnitCode = objR.Specifications.MeasureUnitCode;

                        objRegister.DecimalValuePrecisionTotalDigitNumberValue = objR.Specifications.DecimalValuePrecision.TotalDigitNumberValue;
                        //--> Not in Change: objRegister.DecimalValuePrecisionFractionDigitNumberValue = objR.Specifications.DecimalValuePrecision.FractionDigitNumberValue;
                        objRegister.SpecMeterReadingResultAdjustmentFactorValue = (decimal)objR.Specifications.MeterReadingResultAdjustmentFactorValue;


                        objRegister.IsItemProcessed = false;
                        objRegister.IsConfirmationSent = false;
                        objRegister.IsMdminvoked = false;
                        objRegister.IsActive = true;
                        objRegister.IsCancelled = false;

                        //=========Add to Context
                        _dbContext.Add(objRegister);
                    }
                }
                #endregion


                dtlObj.UtilitiesAdvancedMeteringSystemId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterChangeRequest.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                dtlObj.IsItemProcessed = false;
                dtlObj.IsConfirmationSent = false;
                dtlObj.IsMdminvoked = false;

                //----Register Specific DEtail Fill Up

                //---- TO DO ----


                //**** ===>> This Space for Confirmation Payload Insertion in [MDRConfirmationHeader] Table
                //**************>>> This Payload is created at the time of Request Parsing But not Sent
                //**************>>> Will be sent Once Confirmation Controller is Invoked
                MeterRegisterCreateConfirmationHeader ObjConfirmHdr = new();

                ObjConfirmHdr.MeterRegisterCreateConfirmationHeaderAltId = Guid.NewGuid();
                ObjConfirmHdr.MeterRegisterCreateHeaderId = 1;  //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID
                ObjConfirmHdr.MeterRegisterCreateHeaderAltId = ThisRecAltId;
                ObjConfirmHdr.ServiceCallLogId = EntryId;
                ObjConfirmHdr.PayloadType = payloadType.ToUpper().Trim();
                ObjConfirmHdr.ConfirmationPayload = string.Empty; //==>> Initially Insert NULL, Then Update at the time of Updation;

                ObjConfirmHdr.IsConfirmationSent = false; //At this Stage - Confirmation is not Sent

                ObjConfirmHdr.CreatedOn = DateTime.Now;
                ObjConfirmHdr.UpdatedOn = DateTime.Now;
                ObjConfirmHdr.CreatedBy = "RegisterChangeParser";
                ObjConfirmHdr.UpdatedBy = "RegisterChangeParser";

                //MDRParser - IServerSideBlazorBuilder responsoble to create this Payload and Save

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        _dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterRegisterHeaderId;

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
            catch (Exception ex)
            {

                throw;
            }
        }


        public static long SaveMeterRegisterChangeRequestBULK(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequestRoot ObjXML,
        string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {
                Guid ThisRecAltId = Guid.NewGuid();

                MeterRegisterHeader hdrObj = new();

                hdrObj.MeterRegisterHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();
                hdrObj.RegisterType = "CHANGE";

                hdrObj.MeterRegisterHeaderMessageUuid = Guid.Parse(ObjXML.UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest.MessageHeader.UUID);
                hdrObj.MeterRegisterHeaderCreationDatetime = ObjXML.UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest.MessageHeader.CreationDateTime;
                hdrObj.MeterRegisterHeaderMessageSenderSystemBusinessId = ObjXML.UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest.MessageHeader.SenderBusinessSystemID;

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                #region Parsing Dynamic Messages and Prepare List of Objects
                MeterRegisterDetail objDetail = null;
                MeterCreateChangeRegisterDetail objRegister = null;

                var allMessages = ObjXML.UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest.UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage;
                UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage objMessage = null;
                //Register objR = null;
                foreach (var message in allMessages)
                {
                    objMessage = JsonConvert.DeserializeObject<UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage>(message.ToString());

                    objDetail = new();

                    var ThisMeterRegisterDetailAltId = Guid.NewGuid();

                    objDetail.MeterRegisterDetailAltId = ThisMeterRegisterDetailAltId;
                    objDetail.MasterServiceCallLogId = EntryId;
                    objDetail.MeterRegisterHeaderAltId = ThisRecAltId;
                    objDetail.MeterRegisterHeaderId = 1;

                    objDetail.PayloadType = payloadType.ToUpper().Trim();
                    objDetail.RegisterType = "CHANGE-BULK";

                    objDetail.MeterRegisterDetailMhuuid = Guid.Parse(objMessage.MessageHeader.UUID);
                    objDetail.MeterRegisterDetailMhcreationTime = objMessage.MessageHeader.CreationDateTime;
                    objDetail.MeterRegisterDetailMhsenderBusinessSystemId = objMessage.MessageHeader.SenderBusinessSystemID;

                    objDetail.UtilitiesDeviceId = objMessage.UtilitiesDevice.ID;

                    int Indicator = objMessage.UtilitiesDevice.RegisterListCompleteTransmissionIndicator ? 1 : 0;
                    objDetail.RegisterListCompleteTransmissionIndicatorBool = Indicator;

                    objDetail.UtilitiesAdvancedMeteringSystemId = objMessage.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                    objDetail.IsItemProcessed = false;
                    objDetail.IsConfirmationSent = false;
                    objDetail.IsMdminvoked = false;
                    objDetail.IsCancelled = false;

                    var objRegisterList = objMessage.UtilitiesDevice.Register;
                    if (objRegisterList!=null)
                    {
                        string JsonList = string.Empty;
                        if (!objRegisterList.ToString().Contains("["))
                            JsonList = "[" + objRegisterList.ToString() + "]";
                        else
                            JsonList = objRegisterList.ToString();
                        //var objRList = JsonConvert.DeserializeObject<List<Register>>("[" + objRegisterList.ToString() +"]");
                        var objRList = JsonConvert.DeserializeObject<List<Register>>(JsonList);

                        //foreach (var reg in objRegisterList)
                        foreach (Register objR in objRList)
                         {
                            //objR = JsonConvert.DeserializeObject<ReplicationRegister>(reg.ToString());
                            //var ThisMeterReplicationDetailAltId = Guid.NewGuid();

                            objRegister = new();
                            objRegister.MeterCreateChangeRegisterDetailAltId = Guid.NewGuid();
                            objRegister.ReferenceDetailId = 1; //Dummy Value
                            objRegister.ReferenceDetailAltId = ThisMeterRegisterDetailAltId;
                            objRegister.MasterServiceCallLogId = EntryId;
                            objRegister.PayloadType = "Bulk";
                            objRegister.RegisterSourceType = "RegisterChangeBulk";

                            objRegister.RegisterStartDate = objR.StartDate.ToDateOnly();
                            objRegister.RegisterEndDate = objR.EndDate.ToDateOnly();
                            objRegister.UtilitiesMeasurementTaskId = objR.UtilitiesMeasurementTaskID.ToString();
                            objRegister.UtilitiesObjectIdentificationSystemCodeText = objR.UtilitiesObjectIdentificationSystemCodeText.ToString();
                            objRegister.UtiltiesMeasurementTaskCategoryCode = Convert.ToInt32(objR.UtilitiesMeasurementTaskCategoryCode);
                            objRegister.UtilitiesDivisionCategoryCode = Convert.ToInt32(objR.UtilitiesDivisionCategoryCode);
                            objRegister.TimeZoneCode = objR.TimeZoneCode;

                            objRegister.SpecStartDate = objR.Specifications.StartDate.ToDateOnly();//only for change
                            objRegister.SpecEndDate = objR.Specifications.EndDate.ToDateOnly();//only for change
                            objRegister.SpecMeasureUnitCode = objR.Specifications.MeasureUnitCode;

                            objRegister.DecimalValuePrecisionTotalDigitNumberValue = objR.Specifications.DecimalValuePrecision.TotalDigitNumberValue;
                            //--> Not in Change: objRegister.DecimalValuePrecisionFractionDigitNumberValue = objR.Specifications.DecimalValuePrecision.FractionDigitNumberValue;
                            objRegister.SpecMeterReadingResultAdjustmentFactorValue = (decimal)objR.Specifications.MeterReadingResultAdjustmentFactorValue;


                            objRegister.IsItemProcessed = false;
                            objRegister.IsConfirmationSent = false;
                            objRegister.IsMdminvoked = false;
                            objRegister.IsActive = true;
                            objRegister.IsCancelled = false;

                            //=========Add to Context
                            _dbContext.Add(objRegister);
                        }

                    }

                    //=========Add to Context
                    _dbContext.Add(objDetail);
                }
                #endregion

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterRegisterHeaderId;

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
        #endregion

        #region Con DCON - Update IsResultSentByAdapter Flag

        public static int UpdateIsResultSentByAdapterFlag(AdapterContext _dbContext, long UpdateId)
        {
            int returnVal = 0;

            try
            {
                var entity = _dbContext.MeterConnectionStatusSingles.FirstOrDefault(itm => itm.ServiceCallLogId == UpdateId);

                // Validate entity is not null
                if (entity != null)
                {
                    using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //var std = new Student() { StudentName = "Steve" };
                            entity.IsResultSentByAdapter = true;
                            entity.UpdatedBy = "Adapter-Schedular";
                            entity.UpdatedOn = DateTime.Now;
                            
                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();

                            returnVal = 1;
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }

                return returnVal;
            }
            catch (Exception ex)
            {

                throw;
            }
            //throw new NotImplementedException();
        }
        #endregion

        #region Cancellation - Update IsCancellationConfirmationSentToSAP | IsMDMInvokedCancellation | IsCancellationProcessedByMDM for both Header and Child

        public static int UpdateMROCancellationFlags(AdapterContext _dbContext, long UpdateId)
        {
            int returnVal = 0;

            try
            {
                var entityHeader = _dbContext.MdrcancelHeaders.FirstOrDefault(itm => itm.ServiceCallLogId == UpdateId);
                var entityDetail = _dbContext.MdrcancelDetails.FirstOrDefault(itm => itm.MasterServiceCallLogId == UpdateId);

                // Validate entity is not null
                if (entityHeader != null && entityDetail != null)
                {
                    using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //MdrcancelHeaders
                            entityHeader.IsCancellationConfirmationSentToSap = true;
                            entityHeader.IsMdminvokedCancellation = true;
                            entityHeader.IsCancellationProcessedByMdm = true;

                            entityHeader.UpdatedBy = "Adapter-Auto";
                            entityHeader.UpdatedOn = DateTime.Now;


                            //MdrcancelDetails
                            entityDetail.IsCancellationConfirmationSentToSap = true;
                            entityDetail.IsMdminvokedCancellation = true;
                            entityDetail.IsCancellationProcessedByMdm = true;
                         

                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();

                            returnVal = 1;
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }

                return returnVal;
            }
            catch (Exception ex)
            {

                throw;
            }
            //throw new NotImplementedException();
        }
        #endregion


        #region Update IsResultSent Flag in ResultHeader and ResultDetail - SINGLE 36 | BULK 37

        public static int UpdateIsResultSentForResult(AdapterContext _dbContext, List<Guid> ResultDetailAltIds, Guid ResultHeaderAltId, string PayloadType)
        {
            int returnVal = 0;

            //Update all the Detail IDs from the List
            //get the count of Childs with IsResultSent Flag true and Get the Number of Childs from MDRDETAIL for that HEaderALTID
            //HEaderALTID will get from 

            try
            {
                //var entityDetail = _dbContext.MdrresultDetails.FirstOrDefault(itm => itm.MdrresultHeaderAltId == ResultHeaderAltId);
                var entityDetails = _dbContext.MdrresultDetails.Where(itm => ResultDetailAltIds.Contains(itm.MdrresultDetailAltId)).ToList();

                // Validate entity is not null
                if (entityDetails != null)
                {
                    using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                    {

                        try
                        {
                            //var std = new Student() { StudentName = "Steve" };
                            foreach (var item in entityDetails) { 
                                
                                item.IsResultSent = true;
                                item.UpdatedBy = "Adapter-Schedular";
                                item.UpdatedOn = DateTime.Now;
                            }
                            if (PayloadType.Trim().ToUpper() == "SINGLE")
                            {
                                var entityHeader = _dbContext.MdrresultHeaders.FirstOrDefault(itm => itm.MdrresultHeaderAltId == ResultHeaderAltId);

                                if (entityHeader != null)
                                {
                                    entityHeader.IsResultSent = true;
                                    entityHeader.UpdatedBy = "Adapter-Schedular";
                                    entityHeader.UpdatedOn = DateTime.Now;

                                    /* This should be done after Result Confirmation Came back from SAP
                                    var parentHeader = entityHeader.MdrheaderAltId;
                                    var entityParentHeader = _dbContext.Mdrheaders.FirstOrDefault(itm => itm.MdrheaderAltId == parentHeader);
                                    if (entityParentHeader != null)
                                    {
                                        entityParentHeader.IsProcessCompleted = true;
                                        entityParentHeader.UpdatedBy = "Adapter-Schedular";
                                        entityParentHeader.UpdatedOn = DateTime.Now;
                                    }
                                    */
                                }
                            }

                            if (PayloadType.Trim().ToUpper() == "BULK")
                            {
                                var entityHeader4Bulk = _dbContext.MdrresultHeaders.FirstOrDefault(itm => itm.MdrresultHeaderAltId == ResultHeaderAltId);
                                var entityChild4BulkCount = _dbContext.MdrresultDetails.Where(itm => itm.MdrresultHeaderAltId == ResultHeaderAltId 
                                                                && itm.IsResultSent == true && itm.IsResultReceived == true).Count();
                                var entityParentChild4BulkCount = _dbContext.Mdrdetails.Where(itm => itm.MdrheaderAltId == entityHeader4Bulk.MdrheaderAltId).Count();

                            entityChild4BulkCount = entityChild4BulkCount + entityDetails.Count(); //Not Added yet in context
                            if (entityChild4BulkCount == entityParentChild4BulkCount)
                                    {

                                        if (entityHeader4Bulk != null)
                                        {
                                            entityHeader4Bulk.IsResultSent = true;
                                            entityHeader4Bulk.IsResultReceived = true;
                                            entityHeader4Bulk.UpdatedBy = "Adapter-Schedular";
                                            entityHeader4Bulk.UpdatedOn = DateTime.Now;
                                        }
                                    }
                            }

                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();

                            returnVal = 1;
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }

                return returnVal;
            }
            catch (Exception ex)
            {

                throw;
            }
            //throw new NotImplementedException();
        }
        #endregion


        #region FLEX SYNC 

        private static void AddParameters(List<Domain.Models.Parameter> ObjList)
        {
            foreach (Domain.Models.Parameter obj in ObjList)
            {
            //    obj= new();

            //    obj.FlexSyncParameterAltId = Guid.NewGuid();
            //    obj.FlexSyncHeaderAltId = ThisRecHdrAltId;
            //    obj.ParentAltId = ThisRecAccountAltId;
            //    obj.ParentType = "FlexSyncAccount";
            //    obj.Name = accParam.Name;
            //    obj.Value = accParam.Value;
            //    obj.StartDate = accParam.StartDate;
            //       .
            //    obj.CreatedOn = DateTime.Now;
            //    obj.CreatedBy = "FlexSyncParser";
            //    obj.UpdatedOn = DateTime.Now;
            //    obj.UpdatedBy = "FlexSyncParser";

            //    objParamList.Add(objParam);
            }
        }
        public static long FLexSyncRequestParse(AdapterContext _dbContext, long EntryId,
            MTFlexSyncAMIReqRoot ObjXML, string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecHdrAltId = Guid.NewGuid();
                Guid ThisRecAccountAltId = Guid.NewGuid();
                Guid ThisRecServicePointAltId = Guid.NewGuid();
                Guid ThisRecConsumerAltId = Guid.NewGuid();
                Guid ThisRecConsumerAddressAltId = Guid.NewGuid();
                Guid ThisRecServicePointGroupAltId = Guid.NewGuid();
                Guid ThisRecServicePointServicePointGroupAssociationAltId = Guid.NewGuid();
                Guid ThisRecAccountServicePointAssociationAltId = Guid.NewGuid();
                Guid ThisRecConsumerAccountAssociationAltId = Guid.NewGuid();
                Guid ThisRecConsumerConsumerAddressAssociationAltId = Guid.NewGuid();

                /* 
                   1. FlexSyncHeader
                   2. FlexSyncAccount
                   3. FlexSyncServicePoint
                   4. FlexSyncConsumer
                   5. FlexSyncConsumerAddress
                   6. FlexSyncServicePointGroup
                   7. FlexSyncParameter
                   8. FlexSyncServicePointServicePointGroupAssociation
                   9. FlexSyncAccountServicePointAssociation
                   10. FlexSyncConsumerAccountAssociation
                   11. FlexSyncConsumerConsumerAddressAssociation
                */
                List<FlexSyncParameter> objParamList = new List<FlexSyncParameter>();
                FlexSyncParameter objParam;
                
                //--- 1. FlexSyncHeader
                #region FlexSyncHeader
                FlexSyncHeader objHdr = new();

                objHdr.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objHdr.Noun = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.Noun;
                objHdr.Verb = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.Verb;
                objHdr.Revision = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.Revision;
                objHdr.HeaderDateTime = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.DateTime;
                objHdr.Source = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.Source;
                objHdr.MessageId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.MessageID;
                objHdr.AsyncReplyTo = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.AsyncReplyTo;
                objHdr.SyncMode = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Header.SyncMode;

                objHdr.IsActive = false;
                objHdr.IsProcessed = false;

                objHdr.CreatedOn = DateTime.Now;
                objHdr.CreatedBy = "FlexSyncParser";
                objHdr.UpdatedOn = DateTime.Now;
                objHdr.UpdatedBy = "FlexSyncParser";
                #endregion

                //--- 2.FlexSyncAccount
                #region FlexSyncAccount
                FlexSyncAccount objAccount = new();

                objAccount.FlexSyncAccountAltId = ThisRecAccountAltId;
                objAccount.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objAccount.AccountId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Account.MRID;
                objAccount.Name = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Account.Name;
                objAccount.AccountType = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Account.AccountType;
                objAccount.AccountClass = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Account.AccountClass;
                objAccount.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Account.Status;

                var AccountParamList = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Account.Parameter;
                foreach (var accParam in AccountParamList)
                {
                    objParam = new();
                    objParam.FlexSyncParameterAltId = Guid.NewGuid();
                    objParam.FlexSyncHeaderAltId = ThisRecHdrAltId;
                    objParam.ParentAltId = ThisRecAccountAltId;
                    objParam.ParentType = "FlexSyncAccount";
                    objParam.Name = accParam.Name;
                    objParam.Value = accParam.Value;
                    objParam.StartDate = accParam.StartDate;

                    objParam.CreatedOn = DateTime.Now;
                    objParam.CreatedBy = "FlexSyncParser";
                    objParam.UpdatedOn = DateTime.Now;
                    objParam.UpdatedBy = "FlexSyncParser";

                    objParamList.Add(objParam);
                }

                objAccount.CreatedOn = DateTime.Now;
                objAccount.CreatedBy = "FlexSyncParser";
                objAccount.UpdatedOn = DateTime.Now;
                objAccount.UpdatedBy = "FlexSyncParser";
                #endregion

                //--- FlexSyncServicePoint
                #region FlexSyncServicePoint
                FlexSyncServicePoint objServicePoint = new();

                //objServicePoint.FlexSyncServicePointId = 
                objServicePoint.FlexSyncServicePointAltId = ThisRecServicePointAltId;
                objServicePoint.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objServicePoint.ServicePointId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.MRID;
                objServicePoint.ClassName = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.ClassName;
                objServicePoint.Type = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.Type;
                objServicePoint.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.Status;
                objServicePoint.LocInfo = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.LocInfo;

                var ObjServicePointPremise = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.PremiseId;
                objServicePoint.PremiseMrid = ObjServicePointPremise.MRID;
                objServicePoint.PremiseDescription = ObjServicePointPremise.Description;

                var ServicePointParamList = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePoint.Parameter;
                foreach (var SPParam in ServicePointParamList)
                {
                    objParam = new();
                    objParam.FlexSyncParameterAltId = Guid.NewGuid();
                    objParam.FlexSyncHeaderAltId = ThisRecHdrAltId;
                    objParam.ParentAltId = ThisRecServicePointAltId;
                    objParam.ParentType = "FlexSyncServicePoint";
                    objParam.Name = SPParam.Name;
                    objParam.Value = SPParam.Value;
                    objParam.StartDate = SPParam.StartDate;

                    objParam.CreatedOn = DateTime.Now;
                    objParam.CreatedBy = "FlexSyncParser";
                    objParam.UpdatedOn = DateTime.Now;
                    objParam.UpdatedBy = "FlexSyncParser";

                    objParamList.Add(objParam);
                }
                objServicePoint.CreatedOn = DateTime.Now;
                objServicePoint.CreatedBy = "FlexSyncParser";
                objServicePoint.UpdatedOn = DateTime.Now;
                objServicePoint.UpdatedBy = "FlexSyncParser";

                #endregion

                //---  FlexSyncConsumer
                #region 4 - Consumer
                FlexSyncConsumer objConsumer = new();

                objConsumer.FlexSyncConsumerAltId = ThisRecConsumerAltId;
                objConsumer.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objConsumer.ConsumerId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Consumer.MRID;
                objConsumer.Description = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Consumer.Description;
                objConsumer.Name = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Consumer.Name;
                objConsumer.Type = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Consumer.Type;
                objConsumer.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.Consumer.Status;

                objConsumer.CreatedOn = DateTime.Now;
                objConsumer.CreatedBy = "FlexSyncParser";
                objConsumer.UpdatedOn = DateTime.Now;
                objConsumer.UpdatedBy = "FlexSyncParser";
                #endregion

                //---  5.FlexSyncConsumerAddress
                #region 5 - FlexSyncConsumerAddress
               
                FlexSyncConsumerAddress objConsumerAddress = new();

                objConsumerAddress.FlexSyncConsumerAddressAltId = ThisRecConsumerAddressAltId;
                objConsumerAddress.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objConsumerAddress.ConsumerAddressId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.MRID;
                objConsumerAddress.Description = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.Description;
                objConsumerAddress.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.Status;
                objConsumerAddress.AddressLine1 = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.AddressLine1;
                objConsumerAddress.AddressLine2 = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.AddressLine2;
                objConsumerAddress.City = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.City;
                objConsumerAddress.StateOrProvince = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.StateOrProvince;
                objConsumerAddress.PostalCode = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.PostalCode;
                objConsumerAddress.Country = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.Country;
                objConsumerAddress.Timezone = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAddress.Timezone;

                objConsumerAddress.CreatedOn = DateTime.Now;
                objConsumerAddress.CreatedBy = "FlexSyncParser";
                objConsumerAddress.UpdatedOn = DateTime.Now;
                objConsumerAddress.UpdatedBy = "FlexSyncParser";

                #endregion

                //---  6.FlexSyncServicePointGroup
                #region 6 - FlexSyncServicePointGroup
                FlexSyncServicePointGroup objServicePointGrp = new();

                objServicePointGrp.FlexSyncServicePointGroupAltId = ThisRecServicePointGroupAltId;
                objServicePointGrp.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objServicePointGrp.ServicePointGroupId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointGroup.MRID;
                objServicePointGrp.Name = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointGroup.Name;
                objServicePointGrp.RouteType = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointGroup.RouteType;
                objServicePointGrp.RouteSubType = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointGroup.RouteSubType;
                objServicePointGrp.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointGroup.Status;

                var ServicePointGrpParamList = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointGroup.Parameter;
                foreach (var SPParam in ServicePointGrpParamList)
                {
                    objParam = new();
                    objParam.FlexSyncParameterAltId = Guid.NewGuid();
                    objParam.FlexSyncHeaderAltId = ThisRecHdrAltId;
                    objParam.ParentAltId = ThisRecServicePointAltId;
                    objParam.ParentType = "FlexSyncServicePointGroup";
                    objParam.Name = SPParam.Name;
                    objParam.Value = SPParam.Value;
                    objParam.StartDate = SPParam.StartDate;

                    objParam.CreatedOn = DateTime.Now;
                    objParam.CreatedBy = "FlexSyncParser";
                    objParam.UpdatedOn = DateTime.Now;
                    objParam.UpdatedBy = "FlexSyncParser";

                    objParamList.Add(objParam);
                }

                objServicePointGrp.CreatedOn = DateTime.Now;
                objServicePointGrp.CreatedBy = "FlexSyncParser";
                objServicePointGrp.UpdatedOn = DateTime.Now;
                objServicePointGrp.UpdatedBy = "FlexSyncParser";

                #endregion

                //--- 8. FlexSyncServicePointServicePointGroupAssociation
                #region 8 - FlexSyncServicePointServicePointGroupAssociation
                FlexSyncServicePointServicePointGroupAssociation objServicePointServicePointGroupAsso = new();

                objServicePointServicePointGroupAsso.FlexSyncServicePointServicePointGroupAssociationAltId = ThisRecServicePointServicePointGroupAssociationAltId;
                objServicePointServicePointGroupAsso.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objServicePointServicePointGroupAsso.StartDate = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointServicePointGroupAssociation.StartDate;

                var objServicePointId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointServicePointGroupAssociation.ServicePointId;
                objServicePointServicePointGroupAsso.ServicePointId = objServicePointId.MRID;
                objServicePointServicePointGroupAsso.ServicePointType = objServicePointId.Type;
                objServicePointServicePointGroupAsso.FlexSyncServicePointAltId = ThisRecServicePointAltId;

                var objServicePointGroupId = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ServicePointServicePointGroupAssociation.ServicePointGroupId;
                objServicePointServicePointGroupAsso.ServicePointGroupId = objServicePointGroupId.MRID;
                objServicePointServicePointGroupAsso.ServicePointGroupType = objServicePointGroupId.Type;
                objServicePointServicePointGroupAsso.FlexSyncServicePointGroupAltId = ThisRecServicePointGroupAltId;


                objServicePointServicePointGroupAsso.CreatedOn = DateTime.Now;
                objServicePointServicePointGroupAsso.CreatedBy = "FlexSyncParser";
                objServicePointServicePointGroupAsso.UpdatedOn = DateTime.Now;
                objServicePointServicePointGroupAsso.UpdatedBy = "FlexSyncParser";

                #endregion

                //--- 9.FlexSyncAccountServicePointAssociation
                #region 9 - FlexSyncAccountServicePointAssociation
                FlexSyncAccountServicePointAssociation objFlexSyncAccountServicePointAssociation = new();
               
                objFlexSyncAccountServicePointAssociation.FlexSyncAccountServicePointAssociationAltId = ThisRecAccountServicePointAssociationAltId;
                objFlexSyncAccountServicePointAssociation.FlexSyncHeaderAltId = ThisRecHdrAltId;
                objFlexSyncAccountServicePointAssociation.StartDate = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.AccountServicePointAssociation.StartDate;
                objFlexSyncAccountServicePointAssociation.EndDate = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.AccountServicePointAssociation.EndDate;

                var objAccSpAssoAcc = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.AccountServicePointAssociation.AccountId;
                objFlexSyncAccountServicePointAssociation.AccountId = objAccSpAssoAcc.MRID;
                objFlexSyncAccountServicePointAssociation.AccountType = objAccSpAssoAcc.Description;
                objFlexSyncAccountServicePointAssociation.FlexSyncAccountAltId = ThisRecAccountAltId;

                var objAccSpAssoSP = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.AccountServicePointAssociation.ServicePointId;
                objFlexSyncAccountServicePointAssociation.ServicePointId = objAccSpAssoSP.MRID;
                objFlexSyncAccountServicePointAssociation.ServicePointType = objAccSpAssoSP.Type;
                objFlexSyncAccountServicePointAssociation.FlexSyncServicePointAltId = ThisRecServicePointAltId;

               objFlexSyncAccountServicePointAssociation.CreatedOn = DateTime.Now;
               objFlexSyncAccountServicePointAssociation.CreatedBy = "FlexSyncParser";
               objFlexSyncAccountServicePointAssociation.UpdatedOn = DateTime.Now;
               objFlexSyncAccountServicePointAssociation.UpdatedBy = "FlexSyncParser";

                #endregion

                //-- 10. FlexSyncConsumerAccountAssociation
                #region 10 - FlexSyncConsumerAccountAssociation
                FlexSyncConsumerAccountAssociation objFlexSyncConsumerAccountAssociation = new();

                objFlexSyncConsumerAccountAssociation.FlexSyncConsumerAccountAssociationAltId = ThisRecConsumerAccountAssociationAltId;
                objFlexSyncConsumerAccountAssociation.FlexSyncHeaderAltId = ThisRecHdrAltId;

                var objConAccAssoCon = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAccountAssociation.ConsumerId;
                objFlexSyncConsumerAccountAssociation.ConsumerId = objConAccAssoCon.MRID;
                objFlexSyncConsumerAccountAssociation.ConsumerDescription = objConAccAssoCon.Description;
                objFlexSyncConsumerAccountAssociation.FlexSyncConsumerAltId = ThisRecConsumerAltId;

                var objConAccAssoAcc = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAccountAssociation.AccountId;
                objFlexSyncConsumerAccountAssociation.AccountId = objConAccAssoAcc.MRID;
                objFlexSyncConsumerAccountAssociation.AccountDescription = objConAccAssoAcc.Description;
                objFlexSyncConsumerAccountAssociation.FlexSyncAccountAltId = ThisRecAccountAltId;

                objFlexSyncConsumerAccountAssociation.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAccountAssociation.Status;
                objFlexSyncConsumerAccountAssociation.RelType = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerAccountAssociation.RelType;

                objFlexSyncConsumerAccountAssociation.CreatedOn = DateTime.Now;
                objFlexSyncConsumerAccountAssociation.CreatedBy = "FlexSyncParser";
                objFlexSyncConsumerAccountAssociation.UpdatedOn = DateTime.Now;
                objFlexSyncConsumerAccountAssociation.UpdatedBy = "FlexSyncParser";

                #endregion

                //--- 11. FlexSyncConsumerConsumerAddressAssociation
                #region 11  FlexSyncConsumerConsumerAddressAssociation
                FlexSyncConsumerConsumerAddressAssociation objFlexSyncConsumerConsumerAddressAssociation = new();
                objFlexSyncConsumerConsumerAddressAssociation.FlexSyncConsumerConsumerAddressAssociationAltId = ThisRecConsumerConsumerAddressAssociationAltId;
                objFlexSyncConsumerConsumerAddressAssociation.FlexSyncHeaderAltId = ThisRecHdrAltId;


                var ObjAssoConsumer = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerConsumerAddressAssociation.ConsumerId;
                objFlexSyncConsumerConsumerAddressAssociation.ConsumerId = ObjAssoConsumer.MRID;
                objFlexSyncConsumerConsumerAddressAssociation.ConsumerTypeDesc = "";// ObjAssoConsumer.Description;
                objFlexSyncConsumerConsumerAddressAssociation.FlexSyncConsumerAltId = ThisRecConsumerAltId;


                var ObjAssoConsumerAddress = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerConsumerAddressAssociation.ConsumerAddressId;
                objFlexSyncConsumerConsumerAddressAssociation.ConsumerAddressId = ObjAssoConsumerAddress.MRID;
                objFlexSyncConsumerConsumerAddressAssociation.ConsumerTypeDesc = "";// ObjAssoConsumer.Description;
                objFlexSyncConsumerConsumerAddressAssociation.FlexSyncConsumerAddressAltId = ThisRecConsumerAddressAltId;


                objFlexSyncConsumerConsumerAddressAssociation.Status = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerConsumerAddressAssociation.Status;
                objFlexSyncConsumerConsumerAddressAssociation.AddressRole = ObjXML.MT_FlexSyncAMI_Req.SDPSyncMessage.Payload.ConsumerConsumerAddressAssociation.AddressRole;

                objFlexSyncConsumerConsumerAddressAssociation.CreatedOn = DateTime.Now;
                objFlexSyncConsumerConsumerAddressAssociation.CreatedBy = "FlexSyncParser";
                objFlexSyncConsumerConsumerAddressAssociation.UpdatedOn = DateTime.Now;
                objFlexSyncConsumerConsumerAddressAssociation.UpdatedBy = "FlexSyncParser";


                #endregion
                //--->> INSERT INTO DB IN A TRANSACTION
                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(objHdr); 
                        _dbContext.Add(objAccount);
                        foreach (var prm in objParamList)
                        {
                            _dbContext.Add(prm);
                        }
                        _dbContext.Add(objServicePoint);
                        _dbContext.Add(objConsumer);
                        _dbContext.Add(objConsumerAddress);
                        _dbContext.Add(objServicePointGrp);
                        _dbContext.Add(objServicePointServicePointGroupAsso);
                        _dbContext.Add(objFlexSyncAccountServicePointAssociation);
                        _dbContext.Add(objFlexSyncConsumerAccountAssociation);
                        _dbContext.Add(objFlexSyncConsumerConsumerAddressAssociation);

                        _dbContext.SaveChanges();

                        returnVal = objHdr.HeaderId;

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


        #region REPLICATION

        private static int CountWords(string text, string word)
        {
            int count = (text.Length - text.Replace(word, "").Length) / word.Length;
            return count;
        }
        public static long SaveReplicationRequestBULK(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterReplicationBulkRequestRoot ObjXML,
       string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeterReplicationHeader hdrObj = new();

                hdrObj.MeterReplicationHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.ReplicationMessageUuid = Guid.Parse(ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.MessageHeader.UUID);
                hdrObj.ReplicationCreationDatetime = ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.MessageHeader.CreationDateTime;
                hdrObj.ReplicationSenderSystemBusinessId = ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.MessageHeader.SenderBusinessSystemID;

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                #region Parsing Dynamic Messages and Prepare List of Objects
                List<MeterReplicationDetail> objDetailList = new();
                MeterReplicationDetail objDetail = null;
                List<ReplicationRegisterDetail> objRegtList = new List<ReplicationRegisterDetail>();
                ReplicationRegisterDetail objRegister = null;

                var allMessages = ObjXML.UtilitiesDeviceERPSmartMeterReplicationBulkRequest.UtilitiesDeviceERPSmartMeterReplicationRequestMessage;
                string JsonMsgList = string.Empty;
                if (!allMessages.ToString().Contains("["))
                    JsonMsgList = "[" + allMessages.ToString() + "]";
                else
                    JsonMsgList = allMessages.ToString();

                //var objRList = JsonConvert.DeserializeObject<List<Register>>("[" + objRegisterList.ToString() +"]");
                var objMsgList = JsonConvert.DeserializeObject<List<UtilitiesDeviceERPSmartMeterReplicationRequestMessage>>(JsonMsgList);

                //UtilitiesDeviceERPSmartMeterReplicationRequestMessage objMessage = null;
                //ReplicationRegister objR = null;
                foreach (var objMessage in objMsgList)
                {
                    objDetail = new();

                    var ThisMeterReplicationDetailAltId = Guid.NewGuid();

                    objDetail.MeterReplicationDetailAltId = ThisMeterReplicationDetailAltId;
                    objDetail.ParentServiceCallLogId = EntryId;
                    objDetail.MeterReplicationHeaderAltId = ThisRecAltId;
                    objDetail.MeterReplicationHeaderId = 1;

                    objDetail.MeterReplicationDetailMhuuid = Guid.Parse(objMessage.MessageHeader.UUID);
                    objDetail.MeterReplicationDetailMhcreationTime = objMessage.MessageHeader.CreationDateTime;
                    objDetail.MeterReplicationDetailMhsenderBusinessSystemId = objMessage.MessageHeader.SenderBusinessSystemID;

                    objDetail.UtilitiesDeviceId = objMessage.UtilitiesDevice.ID;

                    objDetail.RegisterListCompleteTransmissionIndicator = objMessage.UtilitiesDevice.RegisterListCompleteTransmissionIndicator;
                    objDetail.LogicalLocationListCompleteTransmissionIndicator = objMessage.UtilitiesDevice.LogicalLocationListCompleteTransmissionIndicator;
                    objDetail.LocationListCompleteTransmissionIndicator = objMessage.UtilitiesDevice.LocationListCompleteTransmissionIndicator;
                    objDetail.RelationshipListCompleteTransmissionIndicator = objMessage.UtilitiesDevice.RelationshipListCompleteTransmissionIndicator;
                    
                    objDetail.UtilitiesDeviceStartDate = objMessage.UtilitiesDevice.StartDate.ToDateOnly();
                    objDetail.UtilitiesDeviceEndDate = objMessage.UtilitiesDevice.EndDate.ToDateOnly();
                    objDetail.UtilitiesDeviceSerialId = objMessage.UtilitiesDevice.SerialID;
                    objDetail.UtilitiesDeviceMaterialId = objMessage.UtilitiesDevice.MaterialID;

                    objDetail.LogicalLocationStartDate = objMessage.UtilitiesDevice.LogicalLocation.StartDate.ToDateOnly();
                    objDetail.LogicalLocationEndDate = objMessage.UtilitiesDevice.LogicalLocation.EndDate.ToDateOnly();
                    objDetail.LogicalInstallationPointId = objMessage.UtilitiesDevice.LogicalLocation.LogicalInstallationPointID;

                    objDetail.LocationStartDate = objMessage.UtilitiesDevice.Location.StartDate.ToDateOnly();
                    objDetail.LocationEndDate = objMessage.UtilitiesDevice.Location.EndDate.ToDateOnly();
                    objDetail.InstallationPointId = objMessage.UtilitiesDevice.Location.InstallationPointID;

                    objDetail.StreetPostalCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.StreetPostalCode;
                    objDetail.CityName = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.CityName;
                    objDetail.StreetName = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.StreetName;
                    objDetail.CountryCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.CountryCode;
                    objDetail.RegionCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.RegionCode;
                    objDetail.TimeZoneCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.TimeZoneCode;
                    objDetail.ParentInstallationPointId = objMessage.UtilitiesDevice.Location.InstallationPointHierarchyRelationship.ParentInstallationPointID;

                    objDetail.UtilitiesAdvancedMeteringSystemId = objMessage.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                    objDetail.IsItemProcessed = false;
                    objDetail.IsConfirmationSent = false;
                    objDetail.IsMdminvoked = false;



                    var objRegisterList = objMessage.UtilitiesDevice.Register;
                    string JsonRegList = string.Empty;
                    if (!objRegisterList.ToString().Contains("["))
                        JsonRegList = "[" + objRegisterList.ToString() + "]";
                    else
                        JsonRegList = objRegisterList.ToString();

                    //var objRList = JsonConvert.DeserializeObject<List<Register>>("[" + objRegisterList.ToString() +"]");
                    var objRegList = JsonConvert.DeserializeObject< List<ReplicationRegister>>(JsonRegList);
                    foreach (var objR in objRegList)
                    {
                        //objR = JsonConvert.DeserializeObject<ReplicationRegister>(reg.ToString());
                        //var ThisMeterReplicationDetailAltId = Guid.NewGuid();

                        objRegister = new();

                        objRegister.ReplicationRegisterDetailAltId = Guid.NewGuid();
                        objRegister.ReferenceDetailId = 1; //Dummy Value
                        objRegister.ReferenceDetailAltId = ThisMeterReplicationDetailAltId;
                        objRegister.MasterServiceCallLogId = EntryId;
                        objRegister.PayloadType = "Bulk";
                        objRegister.RegisterSourceType = "ReplicationBulkRequest";

                        objRegister.RegisterDeviceAssignmentListCompleteTransmissionIndicator = objR.DeviceAssignmentListCompleteTransmissionIndicator;
                        objRegister.RegisterStartDate = objR.StartDate.ToDateOnly();
                        objRegister.RegisterEndDate = objR.EndDate.ToDateOnly();
                        objRegister.UtilitiesMeasurementTaskId = objR.UtilitiesMeasurementTaskID.ToString();
                        objRegister.UtilitiesObjectIdentificationSystemCodeText = objR.UtilitiesObjectIdentificationSystemCodeText.ToString();
                        objRegister.UtiltiesMeasurementTaskCategoryCode = Convert.ToInt32(objR.UtilitiesMeasurementTaskCategoryCode);
                        objRegister.UtilitiesDivisionCategoryCode = Convert.ToInt32(objR.UtilitiesDivisionCategoryCode);
                        objRegister.TimeZoneCode = objR.TimeZoneCode;
                        objRegister.SpecMeasureUnitCode = objR.Specifications.MeasureUnitCode;
                        objRegister.DecimalValuePrecisionTotalDigitNumberValue = objR.Specifications.DecimalValuePrecision.TotalDigitNumberValue;
                        objRegister.DecimalValuePrecisionFractionDigitNumberValue = objR.Specifications.DecimalValuePrecision.FractionDigitNumberValue;
                        objRegister.SpecMeterReadingResultAdjustmentFactorValue = (decimal)objR.Specifications.MeterReadingResultAdjustmentFactorValue;


                        objRegister.IsItemProcessed = false;
                        objRegister.IsConfirmationSent = false;
                        objRegister.IsMdminvoked = false;
                        objRegister.IsActive = true;
                        objRegister.IsCancelled = false;

                        objRegtList.Add(objRegister);
                        //=========Add to Context
                        _dbContext.Add(objRegister);
                    }

                    objDetail.CreatedOn = DateTime.Now;
                    objDetail.UpdatedOn = DateTime.Now;
                    objDetail.CreatedBy = "FromSAP";
                    objDetail.UpdatedBy = "FromSAP";

                    objDetailList.Add(objDetail);
                    //=========Add to Context
                    _dbContext.Add(objDetail);

                    //When There is only one element 
                    //if(count==1)
                    //{
                    //    break;
                    //}
                }
                #endregion


                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterReplicationHeaderId;

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
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


        #region LOCATION 11 and 12 - Single - Bulk - NO CONFIRMATION REQUIRED
        public static long SaveLocationRequestBULK(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterLocationBulkNotificationRoot ObjXML,
      string payloadType, string ServiceKey, int ServiceValue)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeterLocationHeader hdrObj = new();

                hdrObj.MeterLocationHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.MlmessageUuid = ObjXML.UtilitiesDeviceERPSmartMeterLocationBulkNotification.MessageHeader.UUID;
                hdrObj.MlcreationDatetime = ObjXML.UtilitiesDeviceERPSmartMeterLocationBulkNotification.MessageHeader.CreationDateTime;
                hdrObj.MlsenderSystemBusinessId = ObjXML.UtilitiesDeviceERPSmartMeterLocationBulkNotification.MessageHeader.SenderBusinessSystemID;

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);
                

                #region Parsing Dynamic Messages and Prepare List of Objects
                List<MeterLocationDetail> objDetailList = new();
                MeterLocationDetail objDetail = null;
                //List<ReplicationRegisterDetail> objRegtList = new List<ReplicationRegisterDetail>();
                //ReplicationRegisterDetail objRegister = null;

                var allMessages = ObjXML.UtilitiesDeviceERPSmartMeterLocationBulkNotification.UtilitiesDeviceERPSmartMeterLocationNotificationMessage;
                UtilitiesDeviceERPSmartMeterLocationNotificationMessage objMessage = null;
                //ReplicationRegister objR = null;
                foreach (var message in allMessages)
                {
                    objMessage = JsonConvert.DeserializeObject<UtilitiesDeviceERPSmartMeterLocationNotificationMessage>(message.ToString());

                    objDetail = new();

                    var ThisMeterLocationDetailAltId = Guid.NewGuid();

                    objDetail.MeterLocationDetailAltId = ThisMeterLocationDetailAltId;
                    objDetail.ParentServiceCallLogId = EntryId;
                    objDetail.MeterLocationHeaderAltId = ThisRecAltId;
                    objDetail.MeterLocationHeaderId = 1;

                    objDetail.MeterLocationDetailMhuuid = objMessage.MessageHeader.UUID;
                    objDetail.MeterLocationDetailMhcreationTime = objMessage.MessageHeader.CreationDateTime;
                    objDetail.MeterLocationDetailMhsenderBusinessSystemId = objMessage.MessageHeader.SenderBusinessSystemID;

                    objDetail.UtilitiesDeviceId = objMessage.UtilitiesDevice.ID;

                    objDetail.LogicalLocationListCompleteTransmissionIndicator = objMessage.UtilitiesDevice.LogicalLocationListCompleteTransmissionIndicator;
                    objDetail.LocationListCompleteTransmissionIndicator = objMessage.UtilitiesDevice.LocationListCompleteTransmissionIndicator;

                    //objDetail.UtilitiesDeviceStartDate = objMessage.UtilitiesDevice.StartDate.ToDateOnly();
                    //objDetail.UtilitiesDeviceEndDate = objMessage.UtilitiesDevice.EndDate.ToDateOnly();
                    //objDetail.UtilitiesDeviceSerialId = objMessage.UtilitiesDevice.SerialID;
                    //objDetail.UtilitiesDeviceMaterialId = objMessage.UtilitiesDevice.MaterialID;
                    
                    objDetail.LogicalLocationStartDate = objMessage.UtilitiesDevice.LogicalLocation.StartDate.ToDateOnly();
                    objDetail.LogicalLocationEndDate = objMessage.UtilitiesDevice.LogicalLocation.EndDate.ToDateOnly();
                    objDetail.LogicalInstallationPointId = objMessage.UtilitiesDevice.LogicalLocation.LogicalInstallationPointID;

                    objDetail.LocationStartDate = objMessage.UtilitiesDevice.Location.StartDate.ToDateOnly();
                    objDetail.LocationEndDate = objMessage.UtilitiesDevice.Location.EndDate.ToDateOnly();
                    objDetail.InstallationPointId = objMessage.UtilitiesDevice.Location.InstallationPointID;

                    objDetail.StreetPostalCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.StreetPostalCode;
                    objDetail.CityName = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.CityName;
                    objDetail.StreetName = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.StreetName;
                    objDetail.CountryCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.CountryCode;
                    objDetail.RegionCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.RegionCode;
                    objDetail.TimeZoneCode = objMessage.UtilitiesDevice.Location.InstallationPointAddressInformation.TimeZoneCode;
                    objDetail.ParentInstallationPointId = objMessage.UtilitiesDevice.Location.InstallationPointHierarchyRelationship.ParentInstallationPointID;

                    objDetail.InstallationDate = objMessage.UtilitiesDevice.Location.ModificationInformation.InstallationDate.ToDateOnly();
                    objDetail.RemoveDate = objMessage.UtilitiesDevice.Location.ModificationInformation.RemoveDate.ToDateOnly();
                    objDetail.ModificationTimeZoneCode = objMessage.UtilitiesDevice.Location.ModificationInformation.TimeZoneCode;

                    objDetail.UtilitiesAdvancedMeteringSystemId = objMessage.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                    objDetail.IsItemProcessed = false;
                    objDetail.IsConfirmationSent = false;
                    objDetail.IsMdminvoked = false;

                    objDetail.CreatedOn = DateTime.Now;
                    objDetail.UpdatedOn = DateTime.Now;
                    objDetail.CreatedBy = "FromSAP";
                    objDetail.UpdatedBy = "FromSAP";

                    objDetailList.Add(objDetail);
                    //=========Add to Context
                    _dbContext.Add(objDetail); 
                }
                #endregion


                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterLocationHeaderId;

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
                return returnVal;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public static long SaveLocationRequestSINGLE(AdapterContext _dbContext, long EntryId, UtilitiesDeviceERPSmartMeterLocationNotificationRoot ObjXML,
      string payloadType, string ServiceKey, int ServiceValue) 
        {

            long returnVal = 0;
            try
            {
                
                Guid ThisRecAltId = Guid.NewGuid();
                MeterLocationHeader hdrObj = new();

                hdrObj.MeterLocationHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;

                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";

                //--Add to Context
                _dbContext.Add(hdrObj);

                //---- DEtail Fill Up
                MeterLocationDetail objDetail = new();

                var ThisMeterLocationDetailAltId = Guid.NewGuid();

                objDetail.MeterLocationDetailAltId = ThisMeterLocationDetailAltId;
                objDetail.ParentServiceCallLogId = EntryId;
                objDetail.MeterLocationHeaderAltId = ThisRecAltId;
                objDetail.MeterLocationHeaderId = 1;

                objDetail.MeterLocationDetailMhuuid = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.MessageHeader.UUID   ;
                objDetail.MeterLocationDetailMhcreationTime = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.MessageHeader.CreationDateTime;
                objDetail.MeterLocationDetailMhsenderBusinessSystemId = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.MessageHeader.SenderBusinessSystemID;

                objDetail.UtilitiesDeviceId = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.ID;

                objDetail.LogicalLocationListCompleteTransmissionIndicator = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.LogicalLocationListCompleteTransmissionIndicator;
                objDetail.LocationListCompleteTransmissionIndicator = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.LocationListCompleteTransmissionIndicator;

                //objDetail.UtilitiesDeviceStartDate = objMessage.UtilitiesDevice.StartDate.ToDateOnly();
                //objDetail.UtilitiesDeviceEndDate = objMessage.UtilitiesDevice.EndDate.ToDateOnly();
                //objDetail.UtilitiesDeviceSerialId = objMessage.UtilitiesDevice.SerialID;
                //objDetail.UtilitiesDeviceMaterialId = objMessage.UtilitiesDevice.MaterialID;

                objDetail.LogicalLocationStartDate = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.LogicalLocation.StartDate.ToDateOnly();
                objDetail.LogicalLocationEndDate = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.LogicalLocation.EndDate.ToDateOnly();
                objDetail.LogicalInstallationPointId = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.LogicalLocation.LogicalInstallationPointID;

                objDetail.LocationStartDate = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.StartDate.ToDateOnly();
                objDetail.LocationEndDate = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.EndDate.ToDateOnly();
                objDetail.InstallationPointId = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointID;

                objDetail.StreetPostalCode = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointAddressInformation.StreetPostalCode;
                objDetail.CityName = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointAddressInformation.CityName;
                objDetail.StreetName = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointAddressInformation.StreetName;
                objDetail.CountryCode = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointAddressInformation.CountryCode;
                objDetail.RegionCode = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointAddressInformation.RegionCode;
                objDetail.TimeZoneCode = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointAddressInformation.TimeZoneCode;
                objDetail.ParentInstallationPointId = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.InstallationPointHierarchyRelationship.ParentInstallationPointID;

                objDetail.InstallationDate = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.ModificationInformation.InstallationDate.ToDateOnly();
                objDetail.RemoveDate = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.ModificationInformation.RemoveDate.ToDateOnly();
                objDetail.ModificationTimeZoneCode = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.Location.ModificationInformation.TimeZoneCode;

                objDetail.UtilitiesAdvancedMeteringSystemId = ObjXML.UtilitiesDeviceERPSmartMeterLocationNotification.UtilitiesDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                objDetail.IsItemProcessed = false;
                objDetail.IsConfirmationSent = false;
                objDetail.IsMdminvoked = false;

                objDetail.CreatedOn = DateTime.Now;
                objDetail.UpdatedOn = DateTime.Now;
                objDetail.CreatedBy = "FromSAP";
                objDetail.UpdatedBy = "FromSAP";


                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(objDetail);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeterLocationHeaderId;

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


        #region Measurement Task CTPT
        #region 1 & 3 METER CREATE PARSING AND INSERT - SINGLE [1] and BULK [3 - ON HOLD] 
        //UtilitiesDeviceERPSmartMeterCreateRequest
        public static long SaveMeasurmentCTPTRequestSINGLE(AdapterContext _dbContext, long EntryId,
            SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotificationRoot ObjXML, string payloadType, 
            string ServiceKey, int ServiceValue) //--, string ConfirmationPayLoad)
        {

            long returnVal = 0;
            try
            {

                Guid ThisRecAltId = Guid.NewGuid();
                MeasurementTaskRequestHeader hdrObj = new();

                hdrObj.MeasurementTaskRequestHeaderAltId = ThisRecAltId;
                hdrObj.ServiceCallLogId = EntryId;
                hdrObj.PayloadType = payloadType.ToUpper().Trim();

                hdrObj.IsConfirmationSent = false;
                hdrObj.IsMdminvoked = false;
                hdrObj.IsProcessCompleted = false;


                hdrObj.CreatedOn = DateTime.Now;
                hdrObj.UpdatedOn = DateTime.Now;
                hdrObj.CreatedBy = "FromSAP";
                hdrObj.UpdatedBy = "FromSAP";
                //======================
                _dbContext.Add(hdrObj);

                MeasurementTaskRequestDetail dtlObj = new();

                dtlObj.MeasurementTaskRequestDetailAltId = Guid.NewGuid();
                dtlObj.MasterServiceCallLogId = EntryId;
                dtlObj.MeasurementTaskRequestHeaderAltId = ThisRecAltId;
                dtlObj.MeasurementTaskRequestHeaderId = 1; //This is Dummy Value - Not Needed, Drop This Column from Table. Corleate Master - Child With ALTID

                dtlObj.MeasurementTaskRequestDetailMhuuid = Guid.Parse(ObjXML.SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification.MessageHeader.UUID);
                dtlObj.MeasurementTaskRequestDetailMhcreationTime = ObjXML.SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification.MessageHeader.CreationDateTime;
                dtlObj.MeasurementTaskRequestDetailMhsenderBusinessSystemId = ObjXML.SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification.MessageHeader.SenderBusinessSystemID ?? string.Empty;

                var objTask = ObjXML.SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification.UtilitiesMeasurementTask;
                dtlObj.UtilitiesMeasurementTaskId = objTask.ID;
                dtlObj.DeviceAssignmentListCompleteTransmissionIndicator = objTask.DeviceAssignmentListCompleteTransmissionIndicator;
                dtlObj.UtilitiesPointOfDeliveryAssignmentListCompleteTransmissionIndicator = false;
                dtlObj.UtilitiesPointOfDeliveryAssignmentListCompleteTransmissionIndicator = false;

                var msrmntTask = objTask.DeviceAssignment;
                dtlObj.DeviceAssignmentStartDate = msrmntTask.StartDate;
                dtlObj.DeviceAssignmentEndDate = msrmntTask.EndDate;
                dtlObj.DeviceAssignmentTypeCode = msrmntTask.TypeCode;
                dtlObj.UtilitiesQuantityAdjustmentFactorValue = (decimal)msrmntTask.UtilitiesQuantityAdjustmentFactorValue;

                var objDevice = objTask.UtilitiesDevice;
                dtlObj.UtilitiesDeviceId = objDevice.UtilitiesDeviceID;
                dtlObj.UtilitiesAdvancedMeteringSystemId = objDevice.SmartMeter.UtilitiesAdvancedMeteringSystemID;

                dtlObj.IsItemProcessed = false;
                dtlObj.IsConfirmationSent = false;
                dtlObj.IsMdminvoked = false;

                //======================
                _dbContext.Add(dtlObj);
                //MDRParser - IServerSideBlazorBuilder responsoble to create this Payload and Save

                using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Add(hdrObj);
                        _dbContext.Add(dtlObj);
                        //_dbContext.Add(ObjConfirmHdr);
                        _dbContext.SaveChanges();

                        returnVal = hdrObj.MeasurementTaskRequestHeaderId;

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

        #endregion
    }
}
