using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    public class MeterReadRequestBulk
    {
    }
    /*
    [XmlRoot(ElementName = "MessageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "UUID")]
        public string UUID { get; set; }
        [XmlElement(ElementName = "CreationDateTime")]
        public string CreationDateTime { get; set; }
        [XmlElement(ElementName = "SenderBusinessSystemID")]
        public string SenderBusinessSystemID { get; set; }
    }

    [XmlRoot(ElementName = "SmartMeter")]
    public class SmartMeter
    {
        [XmlElement(ElementName = "UtilitiesAdvancedMeteringSystemID")]
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }

    [XmlRoot(ElementName = "UtiltiesDevice")]
    public class UtiltiesDevice
    {
        [XmlElement(ElementName = "UtilitiesDeviceID")]
        public string UtilitiesDeviceID { get; set; }
        [XmlElement(ElementName = "SmartMeter")]
        public SmartMeter SmartMeter { get; set; }
    }

    [XmlRoot(ElementName = "UtiltiesMeasurementTask")]
    public class UtiltiesMeasurementTask
    {
        [XmlElement(ElementName = "UtilitiesMeasurementTaskID")]
        public string UtilitiesMeasurementTaskID { get; set; }
        [XmlElement(ElementName = "UtilitiesObjectIdentificationSystemCodeText")]
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        [XmlElement(ElementName = "UtiltiesDevice")]
        public UtiltiesDevice UtiltiesDevice { get; set; }
    }

    [XmlRoot(ElementName = "MeterReadingDocument")]
    public class MeterReadingDocument
    {
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
        [XmlElement(ElementName = "MeterReadingReasonCode")]
        public string MeterReadingReasonCode { get; set; }
        [XmlElement(ElementName = "ScheduledMeterReadingDate")]
        public string ScheduledMeterReadingDate { get; set; }
        [XmlElement(ElementName = "UtilitiesAdvancedMeteringDataSourceTypeCode")]
        public string UtilitiesAdvancedMeteringDataSourceTypeCode { get; set; }
        [XmlElement(ElementName = "UtiltiesMeasurementTask")]
        public UtiltiesMeasurementTask UtiltiesMeasurementTask { get; set; }
    }

    [XmlRoot(ElementName = "SmartMeterMeterReadingDocumentERPCreateRequestMessage")]
    public class SmartMeterMeterReadingDocumentERPCreateRequestMessage
    {
        [XmlElement(ElementName = "MessageHeader")]
        public MessageHeader MessageHeader { get; set; }
        [XmlElement(ElementName = "MeterReadingDocument")]
        public MeterReadingDocument MeterReadingDocument { get; set; }
    }

    //[XmlRoot(ElementName = "SmartMeterMeterReadingDocumentERPBulkCreateRequest", Namespace = "http://sap.com/xi/SAPGlobal20/Global")]
    [XmlRoot(ElementName = "SmartMeterMeterReadingDocumentERPBulkCreateRequest")]
    public class SmartMeterMeterReadingDocumentERPBulkCreateRequest
    {
        [XmlElement(ElementName = "MessageHeader")]
        public MessageHeader MessageHeader { get; set; }
        [XmlElement(ElementName = "SmartMeterMeterReadingDocumentERPCreateRequestMessage")]
        public List<SmartMeterMeterReadingDocumentERPCreateRequestMessage> SmartMeterMeterReadingDocumentERPCreateRequestMessage { get; set; }
        //[XmlAttribute(AttributeName = "n0", Namespace = "http://www.w3.org/2000/xmlns/")]
        //public string N0 { get; set; }
        //[XmlAttribute(AttributeName = "prx", Namespace = "http://www.w3.org/2000/xmlns/")]
        //public string Prx { get; set; }
    }
    */
}
