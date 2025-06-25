using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    internal class MeterChangeCommon
    {
    }

    public class MeterChangeMessageHeader
    {
        public Guid UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }
    public class MeterChangeUtilitiesDevice
    {
        public string ID { get; set; }
        public string SerialID { get; set; }
        public string MaterialID { get; set; }
        public MeterChangeIndividualMaterialManufacturerInformation IndividualMaterialManufacturerInformation { get; set; }
        public MeterChgangeSmartMeter SmartMeter { get; set; }
    }

    public class MeterChangeIndividualMaterialManufacturerInformation
    {
        public string PartyInternalID { get; set; }
    }

    public class MeterChgangeSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
