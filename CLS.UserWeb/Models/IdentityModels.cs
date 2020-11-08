using CLS.Core.Data;
using CLS.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CLS.UserWeb.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public virtual DbSet<CLSUser> CLSUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Invoke the Identity version of this method to configure relationships 
            // in the AspNetIdentity models/tables
            base.OnModelCreating(modelBuilder);

            // Add a configuration for our new table.  Choose one end of the relationship
            // and tell it how it's supposed to work
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(e => e.CLSUsers)        // ApplicationUser has many MyUser
            //    .WithOptional(e => e.ApplicationUser) // allowed nulls in table
            //    .HasForeignKey(e => e.IdentityID);  // MyUser includes this specified foreign key
            // for an ApplicationUser
        }
    }
}
