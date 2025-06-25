using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{

    /// <summary>
    /// 32	Single	Billing Meter read Request/ On Demand Meter Read Request	Meter Read Request
    /// METER READ REQUEST >>> SINGLE >> Entry Point of SINGLE Cycle
    /// </summary>
    /// 
    public class SmartMeterMeterReadingDocumentERPCreateRequestRoot
    {
        public SmartMeterMeterReadingDocumentERPCreateRequest SmartMeterMeterReadingDocumentERPCreateRequest { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPCreateRequest
    {
        public SingleMeterReadMessageHeader MessageHeader { get; set; }
        public SingleMeterReadingDocument MeterReadingDocument { get; set; }
    }

    public class SingleMeterReadMessageHeader
    {
        public Guid? UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? SenderBusinessSystemID { get; set; }
    }

    public class SingleMeterReadingDocument
    {
        public string ID { get; set; }
        public string MeterReadingReasonCode { get; set; }
        public DateTime ScheduledMeterReadingDate { get; set; }
        public string UtilitiesAdvancedMeteringDataSourceTypeCode { get; set; }
        public SingleUtiltiesMeasurementTask UtiltiesMeasurementTask { get; set; }
    }

    public class SingleUtiltiesMeasurementTask
    {
        public string UtilitiesMeasurementTaskID { get; set; }
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public SingleUtiltiesDevice UtiltiesDevice { get; set; }
    }

    public class SingleUtiltiesDevice
    {
        public string UtilitiesDeviceID { get; set; }
        public SingleSmartMeter SmartMeter { get; set; }
    }

    public class SingleSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
