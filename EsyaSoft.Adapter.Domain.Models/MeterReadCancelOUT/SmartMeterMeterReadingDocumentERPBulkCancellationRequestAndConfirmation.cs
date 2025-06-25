using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    //3.10	41	Bulk	Meter Read Cancel	1	6	2	Meter Read Bulk Cancellation	Meter Read Bulk Cancellation
    #region Read Cancellation BULK
    public class CancellationRequestBulkRoot
    {
        public SmartMeterMeterReadingDocumentERPBulkCancellationRequest SmartMeterMeterReadingDocumentERPBulkCancellationRequest { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPBulkCancellationRequest
    {
        public BulkCancellationMessageHeader MessageHeader { get; set; }
        public List<SmartMeterMeterReadingDocumentERPBulkCancellationRequestMessage> SmartMeterMeterReadingDocumentERPBulkCancellationRequestMessage { get; set; }
    }

    public class BulkCancellationMessageHeader
    {
        public Guid? UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? SenderBusinessSystemID { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPBulkCancellationRequestMessage
    {
        public BulkCancellationMessageHeader MessageHeader { get; set; }
        public MeterReadingDocumentCancellation MeterReadingDocument { get; set; }
    }

    public class MeterReadingDocumentCancellation
    {
        public string ID { get; set; }
        public UtilitiesDeviceSmartMeter UtilitiesDeviceSmartMeter { get; set; }
    }

    public class UtilitiesDeviceSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }

    #endregion


    //3.12	43	Bulk Meter Read Cancel   1	6	2	Meter Read Bulk Cancellation Confirmation Meter Read Bulk Cancellation Confirmation
    #region CANCELLATION CONFIRMATION BULK
    //SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation
    public class SmartMeterMeterReadingDocumentERPBulkCancellationConfirmation
    {
        public BulkCancellationConfirmationMessageHeader MessageHeader { get; set; }
        public List<SmartMeterMeterReadingDocumentERPCancellationConfirmationMessage> LstCancellationConfirmationMessage { get; set; }
        public BulkCancellationConfirmationLog Log { get; set; }
    }
    public class BulkCancellationConfirmationMessageHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? RecipientBusinessSystemID { get; set; }
    }
    public class SmartMeterMeterReadingDocumentERPCancellationConfirmationMessage
    {
        public BulkCancellationConfirmationMessageHeader MessageHeader { get; set; }
        public MeterReadingDocumentCancellationConfirmation MeterReadingDocument { get; set; }
        public BulkCancellationConfirmationLog Log { get; set; }
    }
    public class MeterReadingDocumentCancellationConfirmation
    {
        public string ID { get; set; }
    }
    public class BulkCancellationConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public BulkCancellationConfirmationLogItem Item { get; set; }

    }
    public class BulkCancellationConfirmationLogItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
    #endregion

}
