using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models.ManualMeterRead
{
    //internal class ManualMeterReadsCreateConfirmationBulk
    //{
    //}
    #region SAP IN - 47 - SINGLE Manual Meter Reads Create Confirmation
    public class SmartMeterMeterReadingDocumentERPResultCreateConfirmation
    {
        public ManualMeterReads_BulkCreateConfirmationMessageHeader MessageHeader { get; set; }
        public ManualMeterReads_MeterReadingDocumentConfirmation MeterReadingDocument { get; set; }
        public ManualMeterReads_BulkConfirmationLog Log { get; set; }
    }
    #endregion

    #region SAP IN - 46 - BULK Manual Meter Reads Create Confirmation
    public class SmartMeterMeterReadingDocumentERPResultBulkCreateConfirmation
    {
        public ManualMeterReads_BulkCreateConfirmationMessageHeader MessageHeader { get; set; }
        public List<SmartMeterMeterReadingDocumentERPResultCreateConfirmationMessage> LstManualMeterReadBulkConfirmationMessage { get; set; }
        public ManualMeterReads_BulkConfirmationLog Log { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPResultCreateConfirmationMessage
    {
        public ManualMeterReads_BulkCreateConfirmationMessageHeader MessageHeader { get; set; }
        public ManualMeterReads_MeterReadingDocumentConfirmation MeterReadingDocument { get; set; }
        public ManualMeterReads_BulkConfirmationLog Log { get; set; }
    }
    public class ManualMeterReads_MeterReadingDocumentConfirmation
    {
        public string ID { get; set; }
    }
    public class ManualMeterReads_BulkCreateConfirmationMessageHeader
    {
        public Guid UUID { get; set; }
        public Guid ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string RecipientBusinessSystemID { get; set; }
    }
    public class ManualMeterReads_BulkConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public ManualMeterReads_BulkConfirmationItem Item { get; set; }

    }
    public class ManualMeterReads_BulkConfirmationItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }

    #endregion
}
