using PMSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PMSite.DAL
{
    public class PMSiteContext : DbContext
    {      
        public PMSiteContext() : base("name=PMSiteContext")
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //creating many-to-many relation
            modelBuilder.Entity<Project>().HasMany(c => c.Developers).WithMany(i => i.Projects).Map(t => t.MapLeftKey("ProjectID").MapRightKey("DeveloperID").ToTable("ProjectDeveloper"));
         
        }
    
    }



}
