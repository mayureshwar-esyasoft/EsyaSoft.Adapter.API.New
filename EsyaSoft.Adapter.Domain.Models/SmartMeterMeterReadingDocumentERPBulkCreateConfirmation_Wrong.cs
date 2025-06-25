using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class SmartMeterMeterReadingDocumentERPBulkCreateConfirmation_Wrong
    {
        public MeterCreateConfirmationBulkMessageHeader MessageHeader { get; set; }
        public List<SmartMeterMeterReadingDocumentERPCreateConfirmationMessage> LstRegisterRequest { get; set; }
        public MeterCreateBulkConfirmationLog Log { get; set; }
    }
    /*
    public class MeterCreateConfirmationBulkMessageHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? SenderBusinessSystemID { get; set; }
    }

    public class SmartMeterMeterReadingDocumentERPCreateConfirmationMessage
    {
        public MeterCreateConfirmationBulkMessageHeader MessageHeader { get; set; }
        public MeterReadingDocumentConfirmation MeterReadingDocument { get; set; }
        public MeterCreateBulkConfirmationLog Log { get; set; }
    }

    public class MeterReadingDocumentConfirmation
    {
       public string ID { get; set; }
       public UtiltiesMeasurementTaskConfirmation UtiltiesMeasurementTask { get; set; }
    }

    public class UtiltiesMeasurementTaskConfirmation
    {
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public UtiltiesDeviceConfirmation UtiltiesDevice { get; set; }
    }

    public class UtiltiesDeviceConfirmation
    {
        public string UtilitiesDeviceID { get; set; }
        public SmartMeterConfirmation SmartMeter { get; set; }
    }

    public class SmartMeterConfirmation
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }

    public class MeterCreateBulkConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public MeterCreateBulkConfirmationItem Item { get; set; }

    }
    public class MeterCreateBulkConfirmationItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
    */
}
