using System.Data.Entity;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.EntityFrameworkBaseClass.MRExtensions;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.MembershipProvider.Repositories.Contexts
{
    public class MembershipRebootContext : BaseContext<MembershipRebootContext>
    {
        public MembershipRebootContext()
        {
        }

        public MembershipRebootContext(string nameOfConnection) : base(nameOfConnection) { }

        public DbSet<RelationalUserAccount> Users { get; set; }
        public DbSet<RelationalGroup> Groups { get; set; }
        public DbSet<RelationalUserClaim> Claims { get; set; } 


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<RelationalUserAccount>();
            modelBuilder.ConfigureMembershipRebootGroups<RelationalGroup>();
        }
    }
}
