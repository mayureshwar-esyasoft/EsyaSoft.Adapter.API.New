using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EsyaSoft.Adapter.Domain.Models
{
    //internal class MeasurementCTPTCommon
    //{
    //}


    public class MeasurementMessageHeader
    {
        public string UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class UtilitiesMeasurementTask
    {
        public bool DeviceAssignmentListCompleteTransmissionIndicator { get; set; }
        public string ID { get; set; }
        //public List<MeasurementDeviceAssignment> DeviceAssignments { get; set; }
        public MeasurementDeviceAssignment DeviceAssignment { get; set; }
        public MeasurementUtilitiesDevice UtilitiesDevice { get; set; }
    }

    public class MeasurementDeviceAssignment
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int TypeCode { get; set; }
        public double UtilitiesQuantityAdjustmentFactorValue { get; set; }
        public string AssignedUtilitiesDeviceID { get; set; }
    }
    public class MeasurementUtilitiesDevice
    {
        public string UtilitiesDeviceID { get; set; }
        public MeasurementSmartMeter SmartMeter { get; set; }
    }

    public class MeasurementSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
