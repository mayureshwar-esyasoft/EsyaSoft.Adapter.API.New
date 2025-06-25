using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{

    public class UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequest UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequest { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterRegisterBulkCreateRequest
    {
        public RegisterCreateMessageHeader MessageHeader { get; set; }
        public List<UtilitiesDeviceERPSmartMeterRegisterCreateRequestMessage> UtilitiesDeviceERPSmartMeterRegisterCreateRequestMessage { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterRegisterCreateRequestMessage
    {
        public RegisterCreateMessageHeader MessageHeader { get; set; }
        public RegisterCreateUtilitiesDevice UtilitiesDevice { get; set; }
    }
}