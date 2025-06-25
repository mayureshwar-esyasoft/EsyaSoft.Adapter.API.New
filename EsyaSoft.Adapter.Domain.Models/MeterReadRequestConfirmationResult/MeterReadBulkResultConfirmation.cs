using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class MeterReadingDocumentERPResultCreateRequest
    {
        public MeterReadResultConfirmationBulkMessageHeader MessageHeader { get; set; }
        public MeterReadingDocumentResult MeterReadingDocument { get; set; }
    }
    //FOR RESULT SENDING - SINGLE >> SERVICE# 36
    //FOR RESULT SENDING - BULK >> SERVICE# 37
    public class MeterReadingDocumentERPResultBulkCreateRequest
    {
        public MeterReadResultConfirmationBulkMessageHeader MessageHeader { get; set; }
        public List<MeterReadingDocumentERPResultCreateRequestMessage> LstMeterDocumentResult { get; set; }
    }

    public class MeterReadingDocumentERPResultCreateRequestMessage
    {
        public MeterReadResultConfirmationBulkMessageHeader MessageHeader { get; set; }
        public MeterReadingDocumentResult MeterReadingDocument {  get; set; }

    }
    public class MeterReadingDocumentResult
    {
        public string ID { get; set; }
        public int MeterReadingReasonCode { get; set; }
        public string ScheduledMeterReadingDate { get; set; }
        public UtiltiesMeasurementTaskResult UtiltiesMeasurementTask { get; set; }

        public SentResult Result {  get; set; }
    }
    public class SentResult
    {
        public string ActualMeterReadingDate { get; set; }
        public string ActualMeterReadingTime { get; set; }
        public string MeterReadingTypeCode { get; set; }
        public string MeterReadingResultValue { get; set; }
    }
    public class UtiltiesMeasurementTaskResult
    {
        public string UtilitiesMeasurementTaskID { get; set; }
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public UtiltiesDeviceResult UtiltiesDevice { get; set; }
    }

    public class UtiltiesDeviceResult
    {
        public string UtilitiesDeviceID { get; set; }
    }
    public class MeterReadResultConfirmationBulkMessageHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        //public string CreationDateTime { get; set; }
        public string? RecipientBusinessSystemID { get; set; }
    }

   
}
