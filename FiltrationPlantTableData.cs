using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class FiltrationPlantTableData
    {
        [Display(Name = "Location Name")]
        public string locationName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public List<double> pumpStatus1 { get; set; }

        public List<double> manualStatus { get; set; }

        public List<double> pumpStatus2 { get; set; }
        public double P1WorkingInHours { get; set; }
        public double P2WorkingInHours { get; set; }
        public string workingHoursTodayP1 { get; set; }
        public string workingHoursTodayP2 { get; set; }
        public double WorkingInHours { get; set; }
        public string WorkingHoursToday { get; set; }
        public List<double> Voltage { get; set; }
        public List<double> Current { get; set; }
        public List<double> PF { get; set; }
        public List<double> FreqHz { get; set; }
        public List<double> PKVA { get; set; }
        public List<double> PKVAR { get; set; }
        public List<double> PKW { get; set; }
        public List<double> waterFlow { get; set; }
        public List<double> ChlorineLevel { get; set; }
        public List<double> TankLevel1ft { get; set; }
        public List<double> TankLevel2ft { get; set; }
        public List<double> TDS { get; set; }
        public string waterDischarge { get; set; }
        public string chartString { get; set; }
        public List<string> SpellTimeArray = new List<string>();
        public List<double> SpellDataArray = new List<double>();
        public List<double> SpellDataArray2 = new List<double>();

        public double avgCurrentWorking { get; set; }
        public double avgCurrentNonWorking { get; set; }
        public double avgVoltageWorking { get; set; }
        public double avgVoltageNonWorking { get; set; }
        public double avgPFWorking { get; set; }
        public double avgPFNonWorking { get; set; }
        public double avgFrequencyWorking { get; set; }
        public double avgFrequencyNonWorking { get; set; }
        public double avgFrequency { get; set; }
        public double minFrequency { get; set; }
        public double maxFrequency { get; set; }
        public double avgPKVAWorking { get; set; }
        public double avgPKVANonWorking { get; set; }
        public double avgPKVARWorking { get; set; }
        public double avgPKVARNonWorking { get; set; }
        public double avgPKWWorking { get; set; }
        public double avgPKWNonWorking { get; set; }
        public double avgWaterFlow { get; set; }
        public double minTank1 { get; set; }
        public double maxTank1 { get; set; }
        public double minTank2 { get; set; }
        public double maxTank2 { get; set; }
        public double avgTDS { get; set; }
        public double minTDS { get; set; }
        public double maxTDS { get; set; }
        public string parameterName { get; set; }
        public double totalHours { get; set; }
        public double workingInHours { get; set; }
        public double nonWorkingInHours { get; set; }
        public double workingInHoursRemote { get; set; }
        public double workingInHoursManual { get; set; }
        public double workingInHoursScheduling { get; set; }
        public double availableHours { get; set; }
        public double nonAvailableHours { get; set; }

        public string totalHoursString { get; set; }
        public string workingInHoursString { get; set; }
        public string nonWorkingInHoursString { get; set; }
        public string workingInHoursRemoteString { get; set; }
        public string workingInHoursManualString { get; set; }
        public string workingInHoursSchedulingString { get; set; }
        public string availableHoursString { get; set; }
        public string nonAvailableHoursString { get; set; }


        public double minValue { get; set; }
        public double maxValue { get; set; }
        public double avgVale { get; set; }

        public double avgOfAvailableHours { get; set; }
        public double avgOfNonAvailableHours { get; set; }

        public List<string> LogTime { get; set; }

        public string logDate { get; set; }
    }
}