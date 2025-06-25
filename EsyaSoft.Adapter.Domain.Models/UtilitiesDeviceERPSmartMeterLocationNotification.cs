//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
///*  
// *  <MessageHeader>
//<UUID>005056a7-5d1c-1eef-86b1-4d132c3ddb4a</UUID>
//<CreationDateTime>2024-05-24T04:59:31Z</CreationDateTime>
//<SenderBusinessSystemID>P10470</SenderBusinessSystemID>
//</MessageHeader>
//<UtilitiesDevice logicalLocationListCompleteTransmissionIndicator="true" locationListCompleteTransmissionIndicator="true">
//<ID>47185562</ID>
//<LogicalLocation>
//<StartDate>2022-11-14</StartDate>
//<EndDate>2024-04-22</EndDate>
//<LogicalInstallationPointID>1000000569708</LogicalInstallationPointID>
//</LogicalLocation>
//<LogicalLocation>
//<StartDate>2024-05-24</StartDate>
//<EndDate>9999-12-31</EndDate>
//<LogicalInstallationPointID>1000005677155</LogicalInstallationPointID>
//</LogicalLocation>
//<Location>
//<StartDate>2022-11-14</StartDate>
//<EndDate>2024-04-22</EndDate>
//<InstallationPointID>6001083473</InstallationPointID>
//<InstallationPointAddressInformation>
//<StreetPostalCode>400104</StreetPostalCode>
//<CityName>MUMBAI</CityName>
//<StreetName>L II BEST U T BLD L 2 PHASE IV BEST</StreetName>
//<CountryCode>IN</CountryCode>
//<RegionCode>MAH</RegionCode>
//<TimeZoneCode>INDIA</TimeZoneCode>
//</InstallationPointAddressInformation>
//<InstallationPointHierarchyRelationship>
//<ParentInstallationPointID>1000015582</ParentInstallationPointID>
//</InstallationPointHierarchyRelationship>
//<ModificationInformation>
//<InstallationDate>2022-11-14</InstallationDate>
//<RemoveDate>2024-04-23</RemoveDate>
//<TimeZoneCode>INDIA</TimeZoneCode>
//</ModificationInformation>
//</Location>
//<Location>
//<StartDate>2024-05-24</StartDate>
//<EndDate>9999-12-31</EndDate>
//<InstallationPointID>6001513514</InstallationPointID>
//<InstallationPointAddressInformation>
//<StreetPostalCode>400095</StreetPostalCode>
//<CityName>MUMBAI</CityName>
//<StreetName>SHOP1 KHARODI</StreetName>
//<CountryCode>IN</CountryCode>
//<RegionCode>MAH</RegionCode>
//<TimeZoneCode>INDIA</TimeZoneCode>
//</InstallationPointAddressInformation>
//<InstallationPointHierarchyRelationship>
//<ParentInstallationPointID>1000057615</ParentInstallationPointID>
//</InstallationPointHierarchyRelationship>
//<ModificationInformation>
//<InstallationDate>2024-05-24</InstallationDate>
//<TimeZoneCode>INDIA</TimeZoneCode>
//</ModificationInformation>
//</Location>
//<SmartMeter>
//<UtilitiesAdvancedMeteringSystemID>AEML</UtilitiesAdvancedMeteringSystemID>
//</SmartMeter>
//</UtilitiesDevice>
//</n0:UtilitiesDeviceERPSmartMeterLocationNotification>
// *   
// *   
// *   */
//namespace EsyaSoft.Adapter.Domain.Models
//{
//    public class UtilitiesDeviceERPSmartMeterLocationNotification
//    {
//        public MeterLocationNotificationHeader MessageHeader { get; set; }
//        public MeterLocationNotificationUtilitiesDevice UtilitiesDevice { get; set; }
//    }

//    public class MeterLocationNotificationHeader
//    {
//        public Guid? UUID { get; set; }
//        public DateTime CreationDateTime { get; set; }
//        public string? SenderBusinessSystemID { get; set; }
//    }

//    public class MeterLocationNotificationUtilitiesDevice
//    {
//        public string? ID { get; set; }
//        public List<LogicalLocationInfo> LogicalLocation { get; set; }

//        public List<LocationInfo> Location { get; set; }
//        public SmartMeterCreate SmartMeter { get; set; }

//    }

//    public class LogicalLocationInfo
//    {
//        public DateTime StartDate { get; set; }
//        public DateTime EndDate { get; set; }
//        public long? LogicalInstallationPointID { get; set; }
//    }

//    public class LocationInfo
//    {
//        public DateTime StartDate { get; set; }
//        public DateTime EndDate { get; set; }
//        public long? InstallationPointID { get; set; }

//        public List<InstallationPointAddressInfo> InstallationPointAddressInformation { get; set; }
//        public List<ModificationInfo> ModificationInformation { get; set; }

//    }

//    public class InstallationPointAddressInfo
//    {
//        public string? StreetPostalCode { get; set; }
//        public string? CityName { get; set; }
//        public string? StreetName { get; set; }
//        public string? CountryCode { get; set; }
//        public string? RegionCode { get; set; }
//        public string? TimeZoneCode { get; set; }

//    }

//    public class ModificationInfo
//    {
//        public DateTime InstallationDate { get; set; }
//        public DateTime RemoveDate { get; set; }
//        public string? TimeZoneCode { get; set; }
//    }
//}

