using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class UtilitiesDeviceERPSmartMeterRegisterCreateRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterRegisterCreateRequest UtilitiesDeviceERPSmartMeterRegisterCreateRequest { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterRegisterCreateRequest
    {
        public RegisterCreateMessageHeader MessageHeader { get; set; }
        public RegisterCreateUtilitiesDevice UtilitiesDevice { get; set; }
    }

}
