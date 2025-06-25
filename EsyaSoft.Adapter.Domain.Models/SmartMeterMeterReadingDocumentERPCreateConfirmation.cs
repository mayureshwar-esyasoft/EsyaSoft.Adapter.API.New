using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    //CONFIRMATION FOR SINGLE METER READ REQUEST
    //Class >> 3.3	34	Single	Billing Meter read Request	Meter Read Confirmation
    public class SmartMeterMeterReadingDocumentERPCreateConfirmation
    {
        public SingleMeterReadingConfirmationMessageHeader MessageHeader { get; set; }
        public SingleMeterReadingDocumentConfirmationMeterReadingDocument MeterReadingDocument { get; set; }
        public SingleMeterReadingConfirmationLog Log { get; set; }

    }

    public class SingleMeterReadingDocumentConfirmationMeterReadingDocument
    {
        public string ID { get; set; }
    }
    public class SingleMeterReadingConfirmationMessageHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? RecipientBusinessSystemID { get; set; }
    }


    public class SingleMeterReadingConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public SingleMeterReadingConfirmationItem Item { get; set; }

    }
    public class SingleMeterReadingConfirmationItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
}
