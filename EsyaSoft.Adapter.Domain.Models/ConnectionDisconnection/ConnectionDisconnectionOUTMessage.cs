using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    internal class ConnectionDisconnectionOUTMessage
    {
    }
    #region COMMON 
    public class ConDconOUTMessageHeader
    {
        public string UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class ConDconOUTStatusChangeRequest
    {
        public string ID { get; set; }

        public string CategoryCode { get; set; }

        public string UtilitiesServiceDisconnectionReasonCode { get; set; }

        public DateTime PlannedProcessingDateTime { get; set; }

        public bool ImmediateStatusChangeIndicator { get; set; }

        public ConDconOUTDeviceConnectionStatus DeviceConnectionStatus { get; set; }
    }

    public class ConDconOUTDeviceConnectionStatus
    {
        public string UtilitiesDeviceID { get; set; }

        public ConDconOUTSmartMeter SmartMeter { get; set; }
    }
    public class ConDconOUTSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
    #endregion
    //2.1	24	Single	Connection/Disconnection Request	2			Connect/Disconnect	The interface is used to send Connect/Disconnect requests from SAP to MDMS.
    #region Deprecated
    #region Single - OUT
    public class SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequestRoot
    {
        public SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest { get; set; }
    }
    public class SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest
    {
        public ConDecon_MessageHeader_OUT MessageHeader { get; set; }
        public UtilitiesConnectionStatusChangeRequest_OUT UtilitiesConnectionStatusChangeRequest { get; set; }
    }
    public class UtilitiesConnectionStatusChangeRequest_OUT
    {
        public string ID { get; set; }
        public int CategoryCode { get; set; }
        public string UtilitiesServiceDisconnectionReasonCode { get; set; }
        public DateTime PlannedProcessingDateTime { get; set; }
        public string ImmediateStatusChangeIndicator { get; set; }

        public DeviceConnectionStatus_OUT DeviceConnectionStatus { get; set; }
    }
    public class DeviceConnectionStatus_OUT
    {
        public string UtilitiesDeviceID { get; set; }
        public ConDecon_SmartMeter SmartMeter { get; set; }
    }
    public class ConDecon_SmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
    public class ConDecon_MessageHeader_OUT
    {
        public Guid UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }
    #endregion
    #endregion


    //public class SmartMeterUtilitiesConnectionStatusChangeRequestERPCreateRequest
    //    {
    //        public ConDconOUTMessageHeader MessageHeader { get; set; }
    //        public ConDconOUTStatusChangeRequest UtilitiesConnectionStatusChangeRequest { get; set; }
    //    }

        

        

        
        

}
