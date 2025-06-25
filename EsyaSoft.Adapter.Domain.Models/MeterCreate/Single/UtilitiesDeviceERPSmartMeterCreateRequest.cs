using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{

    public class UtilitiesDeviceERPSmartMeterCreateRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterCreateRequest UtilitiesDeviceERPSmartMeterCreateRequest { get; set; }
    }
    public class UtilitiesDeviceERPSmartMeterCreateRequest
    {
        public MeterCreateRequestMessageHeader MessageHeader { get; set; }
        public MeterCreateRequestUtilitiesDevice UtilitiesDevice { get; set; }
    }
    public class MeterCreateRequestMessageHeader
    {
        public Guid? UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? SenderBusinessSystemID { get; set; }
    }
    public class MeterCreateRequestUtilitiesDevice
    {
        public string? ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? SerialID { get; set; }
        public string? MaterialID { get; set; }
        public IndividualMaterialManufacturerInformationCreate IndividualMaterialManufacturerInformation { get; set; }
        public SmartMeterCreate SmartMeter { get; set; }

    }

    public class IndividualMaterialManufacturerInformationCreate
    {
        public string? PartyInternalID { get; set; }
    }
    public class SmartMeterCreate
    {
        public string? UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
