//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WASA_EMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parameter
    {
        public int ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterUnit { get; set; }
        public int ParameterOrder { get; set; }
        public string AggFunc { get; set; }
        public Nullable<double> ParameterScale { get; set; }
    }
}
