using System.Data.Entity;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.EntityFrameworkBaseClass;

namespace BohFoundation.ApplicationEvaluatorsRepository.DbContext
{
    public class ApplicationEvaluatorRepositoryDbContext : BaseContext<ApplicationEvaluatorRepositoryDbContext>
    {
        public ApplicationEvaluatorRepositoryDbContext(string nameOfConnection) : base(nameOfConnection)
        {
        }

        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantRating> ApplicantRatings { get; set; }
        public DbSet<ApplicationEvaluator> ApplicationEvaluators { get; set; }
        public DbSet<Essay> Essays { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DomainDbModelBuilder.CreateModel(modelBuilder);
        }
    }
}
