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
    
    public partial class Company
    {
        public Company()
        {
            this.Users = new HashSet<User>();
        }
    
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}
