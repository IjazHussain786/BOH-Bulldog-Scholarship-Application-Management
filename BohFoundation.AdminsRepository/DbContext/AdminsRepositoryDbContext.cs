using System.Data.Entity;
using BohFoundation.Domain.EntityFrameworkModels.Admins;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;

namespace BohFoundation.AdminsRepository.DbContext
{
    public class AdminsRepositoryDbContext : BaseContext<AdminsRepositoryDbContext>
    {
        public AdminsRepositoryDbContext(string nameOfConnection) : base(nameOfConnection)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ApplicationEvaluator> ApplicationEvaluators { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<EssayTopic> EssayTopics { get; set; }
        public DbSet<GraduatingClass> GraduatingClasses { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DomainDbModelBuilder.CreateModel(modelBuilder);
        }
    }
}
