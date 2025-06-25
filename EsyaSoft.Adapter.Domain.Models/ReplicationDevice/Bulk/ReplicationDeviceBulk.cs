using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using EsyaSoft.Adapter.Domain.Models.ReplicationDevice;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SingleOrArrayConverter<T> : JsonConverter<List<T>>
{
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Deserialize as a JSON array
            return JsonSerializer.Deserialize<List<T>>(ref reader, options);
        }
        else
        {
            // Deserialize single object and wrap it into a List
            var singleObject = JsonSerializer.Deserialize<T>(ref reader, options);
            return new List<T> { singleObject };
        }
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}

namespace EsyaSoft.Adapter.Domain.Models
{

    public class UtilitiesDeviceERPSmartMeterReplicationBulkRequestRoot
    {
        public UtilitiesDeviceERPSmartMeterReplicationBulkRequest UtilitiesDeviceERPSmartMeterReplicationBulkRequest { get; set; }
    }
        public class UtilitiesDeviceERPSmartMeterReplicationBulkRequest
    {
        public ReplicationMessageHeader MessageHeader { get; set; }
        //public List<UtilitiesDeviceERPSmartMeterReplicationRequestMessage> UtilitiesDeviceERPSmartMeterReplicationRequestMessage { get; set; }
        public dynamic UtilitiesDeviceERPSmartMeterReplicationRequestMessage { get; set; }
    }

    public class UtilitiesDeviceERPSmartMeterReplicationRequestMessage
    {
        public ReplicationMessageHeader MessageHeader { get; set; }
        public ReplicationUtilitiesDevice UtilitiesDevice { get; set; }
    }

    public class ReplicationMessageHeader
    {
        public string UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class ReplicationUtilitiesDevice
    {
        public bool RegisterListCompleteTransmissionIndicator { get; set; }
        public bool LogicalLocationListCompleteTransmissionIndicator { get; set; }
        public bool LocationListCompleteTransmissionIndicator { get; set; }
        public bool RelationshipListCompleteTransmissionIndicator { get; set; }
        public string ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SerialID { get; set; }
        public string MaterialID { get; set; }
        public ReplicationIndividualMaterialManufacturerInformation ManufacturerInfo { get; set; }
        //public List<ReplicationRegister> Register { get; set; }
        public dynamic Register { get; set; }

        //public ReplicationRegister Registers { get; set; }
        public ReplicationLogicalLocation LogicalLocation { get; set; }
        public ReplicationLocation Location { get; set; }
        public ReplicationSmartMeter SmartMeter { get; set; }
    }

    public class ReplicationIndividualMaterialManufacturerInformation
    {
        public string PartyInternalID { get; set; }
    }

    public class ReplicationRegister : Register
    {
        public bool DeviceAssignmentListCompleteTransmissionIndicator { get; set; }
    }

    public class ReplicationSpecifications
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MeasureUnitCode { get; set; }
        public ReplicationDecimalValuePrecision DecimalValuePrecision { get; set; }
        public decimal MeterReadingResultAdjustmentFactorValue { get; set; }
    }

    public class ReplicationDecimalValuePrecision
    {
        public int TotalDigitNumberValue { get; set; }
        public int FractionDigitNumberValue { get; set; }
    }

    public class ReplicationLogicalLocation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LogicalInstallationPointID { get; set; }
    }

    public class ReplicationLocation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InstallationPointID { get; set; }
        public ReplicationInstallationPointAddressInformation InstallationPointAddressInformation { get; set; }
        public ReplicationModificationInformation ModificationInformation { get; set; }
        public InstallationPointHierarchyRelationship InstallationPointHierarchyRelationship { get; set; }
    }

    public class ReplicationInstallationPointAddressInformation
    {
        public string StreetPostalCode { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string TimeZoneCode { get; set; }
    }

    public class ReplicationModificationInformation
    {
        public DateTime InstallationDate { get; set; }
        public string TimeZoneCode { get; set; }
    }

    public class ReplicationInstallationPointHierarchyRelationship
    {
        public string ParentInstallationPointID { get; set; }
    }

    public class ReplicationSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }

}
