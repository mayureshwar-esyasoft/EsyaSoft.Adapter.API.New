using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class UtilitiesDeviceERPSmartMeterChangeRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterChangeRequest UtilitiesDeviceERPSmartMeterChangeRequest { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterChangeRequest
    {
        public MeterChangeMessageHeader MessageHeader { get; set; }
        public MeterChangeUtilitiesDevice UtilitiesDevice { get; set; }
    }

    

    
}
