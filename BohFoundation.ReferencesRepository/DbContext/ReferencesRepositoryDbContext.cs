using System.Data.Entity;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.EntityFrameworkBaseClass;

namespace BohFoundation.ReferencesRepository.DbContext
{
    public class ReferencesRepositoryDbContext : BaseContext<ReferencesRepositoryDbContext>
    {
        public ReferencesRepositoryDbContext(string nameOfConnection) : base(nameOfConnection)
        {
        }

        public DbSet<LetterOfRecommendation> LettersOfRecommendation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DomainDbModelBuilder.CreateModel(modelBuilder);
        }
    }
}
