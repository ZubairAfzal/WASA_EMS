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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WASA_EMS_Entities : DbContext
    {
        public WASA_EMS_Entities()
            : base("name=WASA_EMS_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<MessageIn> MessageIns { get; set; }
        public DbSet<tblEnergy> tblEnergies { get; set; }
        public DbSet<tblParameter> tblParameters { get; set; }
        public DbSet<tblPowerStatu> tblPowerStatus { get; set; }
        public DbSet<tblRemoteSensor> tblRemoteSensors { get; set; }
        public DbSet<tblResource> tblResources { get; set; }
        public DbSet<tblTemplate> tblTemplates { get; set; }
        public DbSet<tblTemplateParameter> tblTemplateParameters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<tblNotification> tblNotifications { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Energy> Energies { get; set; }
        public DbSet<fmsFile> fmsFiles { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceCategory> ResourceCategories { get; set; }
        public DbSet<ResourceParameter> ResourceParameters { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<ValueParameter> ValueParameters { get; set; }
        public DbSet<tblParamForm1> tblParamForm1 { get; set; }
        public DbSet<tblResourceScheduler1> tblResourceScheduler1 { get; set; }
        public DbSet<tblResourceRangeSelector> tblResourceRangeSelectors { get; set; }
    }
}
