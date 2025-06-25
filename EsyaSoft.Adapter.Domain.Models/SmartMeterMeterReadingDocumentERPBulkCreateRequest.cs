using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EsyaSoft.Adapter.Domain.Models
//{
//    public class SmartMeterMeterReadingDocumentERPBulkCreateRequest
//    {
//    }
//}


using System;
using System.Collections.Generic;
namespace EsyaSoft.Adapter.Domain.Models
{

    public class SmartMeterMeterReadingDocumentERPBulkCreateRequestRoot
    {
        public SmartMeterMeterReadingDocumentERPBulkCreateRequest SmartMeterMeterReadingDocumentERPBulkCreateRequest { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPBulkCreateRequest
    {
        public MessageHeader MessageHeader { get; set; }
        public List<SmartMeterMeterReadingDocumentERPCreateRequestMessage> SmartMeterMeterReadingDocumentERPCreateRequestMessage { get; set; }
    }

    public class MessageHeader
    {
        public string UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class SmartMeterMeterReadingDocumentERPCreateRequestMessage
    {
        public MessageHeader MessageHeader { get; set; }
        public MeterReadingDocument MeterReadingDocument { get; set; }
    }

    public class MeterReadingDocument
    {
        public string ID { get; set; }
        public string MeterReadingReasonCode { get; set; }
        public DateTime ScheduledMeterReadingDate { get; set; }
        public string UtilitiesAdvancedMeteringDataSourceTypeCode { get; set; }
        public UtiltiesMeasurementTask UtiltiesMeasurementTask { get; set; }
    }

    public class UtiltiesMeasurementTask
    {
        public string UtilitiesMeasurementTaskID { get; set; }
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public UtiltiesDevice UtiltiesDevice { get; set; }
    }

    public class UtiltiesDevice
    {
        public string UtilitiesDeviceID { get; set; }
        public SmartMeter SmartMeter { get; set; }
    }

    public class SmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}