using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models.MeterReadCancelOUT
{
    internal class SmartMeterMeterReadingDocumentERPSINGLECancellationRequestAndConfirmation
    {
    }


    #region SINGLE READ CANCELLATION
    public class SmartMeterMeterReadingDocumentERPCancellationRequestRoot
    {
        public SmartMeterMeterReadingDocumentERPCancellationRequest SmartMeterMeterReadingDocumentERPCancellationRequest { get; set; }
    }

    public class SmartMeterMeterReadingDocumentERPCancellationRequest
    {
        public SingleMeterReadMessageHeader MessageHeader { get; set; }
        public SingleMeterReadingDocumentCancellation MeterReadingDocument { get; set; }
    }

    public class SingleMeterReadMessageHeader
    {
        public Guid? UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? SenderBusinessSystemID { get; set; }
    }

    public class SingleMeterReadingDocumentCancellation
    {
        public string ID { get; set; }
        public SingleUtilitiesDeviceSmartMeter UtilitiesDeviceSmartMeter { get; set; }
    }

    public class SingleUtilitiesDeviceSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }

    #endregion

    #region SINGLE READ CANCELLATION CONFIRMATION
    public class SmartMeterMeterReadingDocumentERPCancellationConfirmation
    {
        public SingleMeterReadingCancellationConfirmationMessageHeader MessageHeader { get; set; }
        public SingleMeterReadingDocumentCancellationConfirmation MeterReadingDocument { get; set; }
        public SingleMeterReadingCancellationConfirmationLog Log { get; set; }

    }
    
    public class SingleMeterReadingCancellationConfirmationMessageHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? RecipientBusinessSystemID { get; set; }
    }
    public class SingleMeterReadingDocumentCancellationConfirmation
    {
        public string ID { get; set; }
    }
    public class SingleMeterReadingCancellationConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public SingleMeterReadingCancellationConfirmationLogItem Item { get; set; }

    }
    public class SingleMeterReadingCancellationConfirmationLogItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
    #endregion
}
