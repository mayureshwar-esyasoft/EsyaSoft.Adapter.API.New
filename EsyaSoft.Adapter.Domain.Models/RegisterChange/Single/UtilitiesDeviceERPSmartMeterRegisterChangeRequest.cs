using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models.RegisterChange.Single
{
    public class UtilitiesDeviceERPSmartMeterRegisterChangeRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterRegisterChangeRequest UtilitiesDeviceERPSmartMeterRegisterChangeRequest { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterRegisterChangeRequest
    {
        public RegisterMessageHeader MessageHeader { get; set; }
        public RegisterUtilitiesDeviceSingle UtilitiesDevice { get; set; }
    }

    public class RegisterUtilitiesDeviceSingle
    {
        public string ID { get; set; }
        public bool RegisterListCompleteTransmissionIndicator { get; set; }
        public dynamic Register { get; set; }
        public RegisterSmartMeter SmartMeter { get; set; }
    }
}