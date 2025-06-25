using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsyaSoft.Adapter.Domain.Models
{
    //internal class RegisterCommon
    //{
    //}

    public class RegisterMessageHeader
    {
        public string UUID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string SenderBusinessSystemID { get; set; }
    }


    public class RegisterRegister
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UtilitiesMeasurementTaskID { get; set; }

        public string UtilitiesObjectIdentificationSystemCodeText { get; set; }

        public int UtiltiesMeasurementTaskCategoryCode { get; set; }

        public int UtilitiesDivisionCategoryCode { get; set; }

        public string TimeZoneCode { get; set; }

        public RegisterSpecifications Specifications { get; set; }
    }

    public class RegisterSpecifications
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string MeasureUnitCode { get; set; }

        public RegisterDecimalValuePrecision DecimalValuePrecision { get; set; }

        public double MeterReadingResultAdjustmentFactorValue { get; set; }
    }

    public class RegisterDecimalValuePrecision
    {
        public int TotalDigitNumberValue { get; set; }

        public int FractionDigitNumberValue { get; set; }
    }

    public class RegisterSmartMeter
    {
        public string UtilitiesAdvancedMeteringSystemID { get; set; }
    }
}
