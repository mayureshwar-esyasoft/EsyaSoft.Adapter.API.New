using EsyaSoft.Adapter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


//LOCATION NOTIFICATION - SINGLE
namespace EsyaSoft.Adapter.Domain.Models
{
    public class UtilitiesDeviceERPSmartMeterLocationNotificationRoot
    {
        public UtilitiesDeviceERPSmartMeterLocationNotification UtilitiesDeviceERPSmartMeterLocationNotification { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterLocationNotification
    {
        public LocationNotificationMessageHeader MessageHeader { get; set; }
        public UtilitiesDevice UtilitiesDevice { get; set; }
    }

}
