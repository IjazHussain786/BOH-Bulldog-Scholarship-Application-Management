using System.Data.Entity;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.EntityFrameworkBaseClass;

namespace BohFoundation.PersonsRepository.DbContext
{
    public class PersonsRepositoryDbContext : BaseContext<PersonsRepositoryDbContext>
    {
        public PersonsRepositoryDbContext(string nameOfConnection) : base(nameOfConnection)
        {
        }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DomainDbModelBuilder.CreateModel(modelBuilder);
        }
    }
}
