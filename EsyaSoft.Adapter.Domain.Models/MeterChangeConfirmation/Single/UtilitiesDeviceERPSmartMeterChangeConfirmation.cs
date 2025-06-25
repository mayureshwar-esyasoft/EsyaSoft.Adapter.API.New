using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    
    public class UtilitiesDeviceERPSmartMeterChangeConfirmation
    {
        public MeterChangeConfirmationMessageHeaderSingle MessageHeader { get; set; }
        public MeterChangeConfirmationUtilitiesDeviceSingle UtilitiesDevice { get; set; }
        public MeterChangeConfirmationLogSingle Log { get; set; }
    }
    public class MeterChangeConfirmationMessageHeaderSingle
    {
        public Guid UUID { get; set; }
        public Guid ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string RecipientBusinessSystemID { get; set; }
    }
    public class MeterChangeConfirmationUtilitiesDeviceSingle
    {
        public string ID { get; set; }
    }

    public class MeterChangeConfirmationLogSingle
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public MeterChangeConfirmationLogItemSingle Item { get; set; }
    }
    public class MeterChangeConfirmationLogItemSingle
    {
        public string TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string Note { get; set; }
    }
}
