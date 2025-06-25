using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    //public class UtilitiesDeviceERPSmartMeterCreateConfirmationModel 
    //[XmlRoot("glob")]
    public class UtilitiesDeviceERPSmartMeterCreateConfirmation
    {
        public ConfirmationHeader MessageHeader { get; set; }
        public ConfirmationUtilitiesDevice UtilitiesDevice { get; set; }
        public ConfirmationLog Log { get; set; }


    }
    public class ConfirmationUtilitiesDevice
    {
        public string? ID { get; set; }
    }
    public class ConfirmationHeader
    {
        public Guid? UUID { get; set; }
        public Guid? ReferenceUUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? RecipientBusinessSystemID { get; set; }
    }
    public class ConfirmationLog
    {
        public int BusinessDocumentProcessingResultCode { get; set; }
        public int MaximumLogItemSeverityCode { get; set; }
        public Item Item { get; set; }

    }
    public class Item
    {
        public string? TypeID { get; set; }
        public int SeverityCode { get; set; }
        public string? Note { get; set; }
    }
}
