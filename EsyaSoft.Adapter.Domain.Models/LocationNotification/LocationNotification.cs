using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


//Location Notification COMMON
namespace EsyaSoft.Adapter.Domain.Models
{
    public class LocationNotificationMessageHeader
    {
        public Guid UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class UtilitiesDevice
    {
        public bool LogicalLocationListCompleteTransmissionIndicator { get; set; }

        public bool LocationListCompleteTransmissionIndicator { get; set; }

        public string ID { get; set; }
        public LogicalLocation LogicalLocation { get; set; }
        public Location Location { get; set; }
        public SmartMeter SmartMeter { get; set; }
    }

    public class LogicalLocation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LogicalInstallationPointID { get; set; }
    }

    public class Location
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InstallationPointID { get; set; }
        public InstallationPointAddressInformation InstallationPointAddressInformation { get; set; }
        public InstallationPointHierarchyRelationship InstallationPointHierarchyRelationship { get; set; }
        public ModificationInformation ModificationInformation { get; set; }
    }

    public class InstallationPointAddressInformation
    {
        public string StreetPostalCode { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string TimeZoneCode { get; set; }
    }

    public class InstallationPointHierarchyRelationship
    {
        public string ParentInstallationPointID { get; set; }
    }

    public class ModificationInformation
    {
        public DateTime InstallationDate { get; set; }
        public DateTime RemoveDate { get; set; }
        public string TimeZoneCode { get; set; }
    }

    public class LocationNotificationSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
