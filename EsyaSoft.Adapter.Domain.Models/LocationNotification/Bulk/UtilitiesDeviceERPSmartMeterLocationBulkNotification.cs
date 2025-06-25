using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

// LOCATION NOTIFICATION - BULK
namespace EsyaSoft.Adapter.Domain.Models
{
    public class UtilitiesDeviceERPSmartMeterLocationBulkNotificationRoot
    {
        public UtilitiesDeviceERPSmartMeterLocationBulkNotification UtilitiesDeviceERPSmartMeterLocationBulkNotification { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterLocationBulkNotification
    {
        public LocationNotificationMessageHeader MessageHeader { get; set; }
        //public List<UtilitiesDeviceERPSmartMeterLocationNotificationMessage> UtilitiesDeviceERPSmartMeterLocationNotificationMessage { get; set; }
        public dynamic UtilitiesDeviceERPSmartMeterLocationNotificationMessage { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterLocationNotificationMessage
    {
        public LocationNotificationMessageHeader MessageHeader { get; set; }
        public UtilitiesDevice UtilitiesDevice { get; set; }
    }

}