using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest { get; set; }
    }
        public class UtilitiesDeviceERPSmartMeterRegisterBulkChangeRequest
    {
        public RegisterMessageHeader MessageHeader { get; set; }
        //public List<UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage> UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage { get; set; }
        public dynamic UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage { get; set; }
    }

    public class UtilitiesDeviceERPSmartMeterRegisterChangeRequestMessage
    {
        public RegisterMessageHeader MessageHeader { get; set; }

        public RegisterUtilitiesDeviceBulk UtilitiesDevice { get; set; }
    }

    public class RegisterUtilitiesDeviceBulk
    {
        public string ID { get; set; }

        public bool RegisterListCompleteTransmissionIndicator { get; set; }

        //public List<RegisterRegister> Register { get; set; }
        public dynamic Register { get; set; }

        public RegisterSmartMeter SmartMeter { get; set; }
    }
}
