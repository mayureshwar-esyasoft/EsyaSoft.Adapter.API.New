using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    internal class ConnectionDisconnectionINMessage
    {
    }

    #region SINGLE - IN
    public class SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateConfirmation
    {
        public ConDecon_MessageHeader_IN MessageHeader { get; set; }
        public UtilitiesConnectionStatusChangeRequest_IN UtilitiesConnectionStatusChangeRequest { get; set; }
        public ConDecon_Log Log { get; set; }
    }

    public class UtilitiesConnectionStatusChangeRequest_IN {
        public string ID { get; set; }
        public int CategoryCode { get; set; }
        public DeviceConnectionStatus_IN DeviceConnectionStatus { get; set; }
    }
    public class DeviceConnectionStatus_IN
    { 
        public DateTime ProcessingDateTime { get; set; }
        public int UtilitiesDeviceConnectionStatusProcessingResultCode { get; set; }
        public string UtilitiesDeviceID {  get; set; }
    }


    public class ConDecon_MessageHeader_IN
    {
        public Guid UUID { get; set; }
        public Guid ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string RecipientBusinessSystemID { get; set; }
    }

    public class ConDecon_Log
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public ConDecon_LogItem Item { get; set; }

    }
    public class ConDecon_LogItem
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
    #endregion
}
