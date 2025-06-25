using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{

    /*
    internal class RegisterCreateCommon
    {
    }
    */
    public class RegisterCreateMessageHeader
    {
        public Guid UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }

    public class RegisterCreateUtilitiesDevice
    {
        public string ID { get; set; }
        //public List<Register> Register { get; set; }
        public dynamic Register { get; set; }
        public RegisterCreateSmartMeter SmartMeter { get; set; }
    }

    public class Register
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UtilitiesMeasurementTaskID { get; set; }
        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }
        public string UtilitiesMeasurementTaskCategoryCode { get; set; }
        public string UtilitiesDivisionCategoryCode { get; set; }
        public string TimeZoneCode { get; set; }
        public Specifications Specifications { get; set; }
    }

    public class Specifications
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MeasureUnitCode { get; set; }
        public DecimalValuePrecision DecimalValuePrecision { get; set; }
        public float MeterReadingResultAdjustmentFactorValue { get; set; }
    }

    public class DecimalValuePrecision
    {
        public int TotalDigitNumberValue { get; set; }
        public int FractionDigitNumberValue { get; set; }
    }

    public class RegisterCreateSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
