using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace EsyaSoft.Adapter.Domain.Models.ResultConfirmationFromSAPOUT
{

    /// <summary>
    ///38	Single	Billing Meter read Request	Meter Read SAP Result Confirmation
    ///39	Bulk Billing Meter read Request/On Demand Meter read request Meter Read SAP Bulk Result Confirmation
    /// </summary>
    public class ResultConfirmationSAPOut
    {
    }

    #region COMMON PAYLOAD OBJECTS
    public class ResultConfirmationMessageHeader
    {
        public Guid UUID { get; set; }
        public Guid ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class ResultConfirmationMeterReadingDocument
    {
        public string ID { get; set; }
        public ResultConfirmationUtiltiesMeasurementTask UtiltiesMeasurementTask { get; set; }
    }

    public class ResultConfirmationUtiltiesMeasurementTask
    {
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public ResultConfirmationUtiltiesDevice UtiltiesDevice { get; set; }
    }

    public class ResultConfirmationUtiltiesDevice
    {
        public string UtilitiesDeviceID { get; set; }
        public ResultConfirmationSmartMeter SmartMeter { get; set; }
    }

    public class ResultConfirmationSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }

    public class ResultConfirmationLog
    {
        public string BusinessDocumentProcessingResultCode { get; set; }
        public string MaximumLogItemSeverityCode { get; set; }
        
        //public List<ResultConfirmationItem> Item { get; set; }
        public ResultConfirmationItem Item { get; set; }
    }

    public class ResultConfirmationItem
    {
        public string TypeID { get; set; }
        public string SeverityCode { get; set; }
        public string Note { get; set; }
        //public string WebURI { get; set; }
    }

    #endregion

    #region BULK Result Confirmation From SAP - 39
    public class MeterReadingDocumentERPResultBulkCreateConfirmationRoot
    {
        public MeterReadingDocumentERPResultBulkCreateConfirmation MeterReadingDocumentERPResultBulkCreateConfirmation { get; set; }
    }
    public class MeterReadingDocumentERPResultBulkCreateConfirmation
    {
        public ResultConfirmationMessageHeader MessageHeader { get; set; }
        public List<ERPResultCreateConfirmationMessage> MeterReadingDocumentERPResultCreateConfirmationMessage { get; set; }
        public ResultConfirmationLog Log { get; set; }
    }

    public class ERPResultCreateConfirmationMessage
    {
        public ResultConfirmationMessageHeader MessageHeader { get; set; }
        public ResultConfirmationMeterReadingDocument MeterReadingDocument { get; set; }
        public ResultConfirmationLog Log { get; set; }
    }
    #endregion

    #region SINGLE Result Confirmation From SAP - 38

    public class MeterReadingDocumentERPResultCreateConfirmationRoot
    {
        public MeterReadingDocumentERPResultCreateConfirmation MeterReadingDocumentERPResultCreateConfirmation { get; set; }
    }
    public class MeterReadingDocumentERPResultCreateConfirmation
    {
        public ResultConfirmationMessageHeader MessageHeader { get; set; }
        public ResultConfirmationMeterReadingDocument MeterReadingDocument { get; set; }
        public ResultConfirmationLog Log { get; set; }
    }
    #endregion

}
