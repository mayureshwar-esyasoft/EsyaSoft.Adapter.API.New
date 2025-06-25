
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    //[XmlRoot(ElementName = "MeterReadingDocumentERPResultBulkCreateRequest")]
    //[XmlType("MeterReadingDocumentERPResultBulkCreateRequest")]
    public class MeterReadingDocumentERPResultBulkCreateRequest_Self
    {
        //public MeterReadingDocumentMessageHeader MessageHeader { get; set; }
        //public List<clsSmartMeterMeterReadingDocumentERPCreateRequestMessage> SmartMeterMeterReadingDocumentERPCreateRequestMessage { get; set; }
    }

    #region Comment 
    /*
    public class clsSmartMeterMeterReadingDocumentERPCreateRequestMessage
    { 
        public MeterReadingDocumentMessageHeader MessageHeader { get; set; }
        public clsMeterReadingDocument MeterReadingDocument {  get; set; }

    }
    public class MeterReadingDocumentMessageHeader
    {
        public Guid? UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? SenderBusinessSystemID { get; set; }
    }
    public class clsMeterReadingDocument
    {
        public long ID { get; set; }
        public int MeterReadingReasonCode { get; set; }
        public DateTime ScheduledMeterReadingDate { get; set; }
        public int UtilitiesAdvancedMeteringDataSourceTypeCode { get; set; }

        public clsUtiltiesMeasurementTask UtiltiesMeasurementTask { get; set; }
    }
    public class clsUtiltiesMeasurementTask
    {
        public string UtilitiesMeasurementTaskID { get; set; }
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public clsUtiltiesMeasurementTask UtiltiesDevice { get; set; }

    }
    public class clsUtiltiesDevice
    {
        public string UtilitiesDeviceID { get; set; }
        public clsSmartMeter SmartMeter { get; set; }

    }
    public class clsSmartMeter { 
     public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
    */
    #endregion
}