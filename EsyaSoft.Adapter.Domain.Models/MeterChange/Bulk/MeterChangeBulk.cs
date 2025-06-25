using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class UtilitiesDeviceERPSmartMeterBulkChangeRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterBulkChangeRequest UtilitiesDeviceERPSmartMeterBulkChangeRequest { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterBulkChangeRequest
    {
        public MeterChangeMessageHeader MessageHeader { get; set; }
        public List<UtilitiesDeviceERPSmartMeterChangeRequestMessage> UtilitiesDeviceERPSmartMeterChangeRequestMessage { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterChangeRequestMessage
    {
        public MeterChangeMessageHeader MessageHeader { get; set; }
        public MeterChangeUtilitiesDevice UtilitiesDevice { get; set; }
    }
}
