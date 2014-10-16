using System.Data.Entity;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;

namespace BohFoundation.EntityFrameworkBaseClass
{
    public class DatabaseRootContext : DbContext
    {
        public DatabaseRootContext(string nameOfConnection)
            : base("name=" + nameOfConnection)
        {
            
        }

        //Membership Reboot
        public DbSet<RelationalUserAccount> Users { get; set; }
        public DbSet<RelationalGroup> Groups { get; set; }
        public DbSet<RelationalUserClaim> Claims { get; set; } 


        //My Common Entities
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Name> Names { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }

        //Applicant Entities
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantPersonalInformation> ApplicantPersonalInformations { get; set; }
        public DbSet<FamilyInformation> FamilyInformations { get; set; }
        public DbSet<AcademicInformation> AcademicInformations { get; set; }
        public DbSet<LowGrade> LowGrades { get; set; }
        public DbSet<Essay> Essays { get; set; } 
        

        //Admin Entities
        public DbSet<EssayTopic> EssayTopics { get; set; }
        public DbSet<GraduatingClass> GraduatingClasses { get; set; } 

        //Reference Entities
        public DbSet<LetterOfRecommendation> LetterOfRecommendations { get; set; }
        public DbSet<Reference> References { get; set; } 

        //Application Evaluators Entities
        public DbSet<ApplicationEvaluator> ApplicationEvaluators { get; set; }
        public DbSet<ApplicantRating> ApplicantRatings { get; set; }
        public DbSet<EssayRating> EssayRatings { get; set; } 
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<RelationalUserAccount>();
            modelBuilder.ConfigureMembershipRebootGroups<RelationalGroup>();
            
            DomainDbModelBuilder.CreateModel(modelBuilder);
        }
    }
}
