using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class TubewellSpellData
    {
        public int SpellNumber { get; set; }
        public string SpellStartTime { get; set; }
        public string SpellEndTime { get; set; }
        //public double SpellMin { get; set; }
        //public double SpellMax { get; set; }
        //public double SpellAvg { get; set; }
        //public double spellFlowUp { get; set; }
        //public double spellFlowDown { get; set; }
        public List<double> SpellDataArray = new List<double>();
        public List<string> SpellTimeArray = new List<string>();
        public int ResourceId { get; set; }
        public string resourceName { get; set; }
        //public string spellMaxTime { get; set; }
        //public string spellMinTime { get; set; }
        public double spellPeriod { get; set; }
        public int spellMode { get; set; }
        public string spellDate { get; set; }
    }
}