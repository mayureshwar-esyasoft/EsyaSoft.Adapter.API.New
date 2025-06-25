using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    //internal class ManualMeterReadBulkCreateRequest
    //{
    //}
    #region SAP OUT - 45 - SINGLE - SAP sends manual meter reads create request to MDMS 
    public class SmartMeterMeterReadingDocumentERPResultCreateRequestRoot
    {
        public SmartMeterMeterReadingDocumentERPResultCreateRequest SmartMeterMeterReadingDocumentERPResultCreateRequest { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPResultCreateRequest
    {
        public ManualMeterRead_MessageHeader_BulkCreateRequest MessageHeader { get; set; }
        public ManualMeterRead_MeterReadingDocument_BulkCreateRequest MeterReadingDocument { get; set; }
    }
    #endregion

    #region SAP OUT - 44 - BULK - SAP sends Bulk manual meter reads create request to MDMS
    public class SmartMeterMeterReadingDocumentERPResultBulkCreateRequestRoot
    {
        public SmartMeterMeterReadingDocumentERPResultBulkCreateRequest SmartMeterMeterReadingDocumentERPResultBulkCreateRequest { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPResultBulkCreateRequest
    {
        public ManualMeterRead_MessageHeader_BulkCreateRequest MessageHeader { get; set; }
        public List<SmartMeterMeterReadingDocumentERPResultCreateRequestMessage> SmartMeterMeterReadingDocumentERPResultCreateRequestMessage { get; set; }
    }

    public class ManualMeterRead_MessageHeader_BulkCreateRequest
    {
        public Guid UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class SmartMeterMeterReadingDocumentERPResultCreateRequestMessage
    {
        public ManualMeterRead_MessageHeader_BulkCreateRequest MessageHeader { get; set; }
        public ManualMeterRead_MeterReadingDocument_BulkCreateRequest MeterReadingDocument { get; set; }
    }

    public class ManualMeterRead_MeterReadingDocument_BulkCreateRequest
    {
        public string ID { get; set; }
        public string MeterReadingReasonCode { get; set; }
        public ManualMeterRead_UtiltiesMeasurementTask_BulkCreateRequest UtilitiesMeasurementTask { get; set; }

        public ManualMeterRead_SentResult_BulkCreateRequest Result { get; set; }
     }

    public class ManualMeterRead_UtiltiesMeasurementTask_BulkCreateRequest
    {
        public string UtilitiesMeasurementTaskID { get; set; }
        public ManualMeterRead_UtiltiesDevice_BulkCreateRequest UtilitiesDevice { get; set; }
    }
    public class ManualMeterRead_UtiltiesDevice_BulkCreateRequest
    {
        public string UtilitiesDeviceID { get; set; }
        public ManualMeterRead_SmartMeter_BulkCreateRequest SmartMeter { get; set; }
    }
    public class ManualMeterRead_SmartMeter_BulkCreateRequest
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }


    public class ManualMeterRead_SentResult_BulkCreateRequest
    {
        public string MeterReadingDate { get; set; }
        public string MeterReadingTime { get; set; }
        public string MeterReadingTypeCode { get; set; }
        public decimal? MeterReadingResultValue { get; set; }
    }
    #endregion



}
