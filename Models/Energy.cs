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
    
    public partial class Energy
    {
        public long ID { get; set; }
        public System.DateTime InsertionDateTime { get; set; }
        public int ResourceID { get; set; }
        public int ParameterID { get; set; }
        public double ParameterValue { get; set; }
    }
}
