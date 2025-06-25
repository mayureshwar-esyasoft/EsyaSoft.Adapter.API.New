using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotificationRoot
    {
        public SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification { get; set; }

    }
    public class SmartMeterUtilitiesMeasurementTaskERPDeviceAssignmentNotification
    {
        public MessageHeader MessageHeader { get; set; }
        public UtilitiesMeasurementTask UtilitiesMeasurementTask { get; set; }

    }


}
