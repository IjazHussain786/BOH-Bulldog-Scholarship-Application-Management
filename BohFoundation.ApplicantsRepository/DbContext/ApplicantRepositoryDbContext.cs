using System.Data.Entity;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;

namespace BohFoundation.ApplicantsRepository.DbContext
{
    public class ApplicantRepositoryDbContext : BaseContext<ApplicantRepositoryDbContext>
    {
        public ApplicantRepositoryDbContext(string nameOfConnection) : base(nameOfConnection)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<AcademicInformation> AcademicInformations { get; set; }
        public DbSet<LowGrade> LowGrades { get; set; }
        public DbSet<GraduatingClass> GraduatingClasses { get; set; }
        public DbSet<EssayTopic> EssayTopics { get; set; }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DomainDbModelBuilder.CreateModel(modelBuilder);
        }
    }
}
