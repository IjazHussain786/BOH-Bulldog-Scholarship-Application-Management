using System;
using BohFoundation.AdminsRepository.DbContext;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.Domain.EntityFrameworkModels.Admins;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.Enums;

namespace BohFoundation.PersonsRepository.Repositories.Implementations
{
    public class CreatePersonRepository : ICreatePersonRepository
    {
        private readonly string _dbConnection;

        public CreatePersonRepository(string dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void CreatePerson(Guid membershipGuid, Name personsName, MemberTypesEnum memberType)
        {
            var person = new Person { Name = personsName, Guid = membershipGuid, DateCreated = DateTime.UtcNow };
            person.Name.LastUpdated = DateTime.UtcNow;

            if (memberType == MemberTypesEnum.Applicant)
            {
                AddApplicantToDatabase(person);
            }
            if (memberType == MemberTypesEnum.ApplicationEvaluator)
            {
                AddApplicationEvaluatorToDatabase(person);
            }
            if (memberType == MemberTypesEnum.Admin)
            {
                AddAdminToDatabase(person);
            }
        }

        private void AddAdminToDatabase(Person person)
        {
            var admin = new Admin {Person = person};
            var applicationEvaluator = new ApplicationEvaluator {Person = person};

            using (var context = new AdminsRepositoryDbContext(_dbConnection))
            {
                context.Admins.Add(admin);
                context.ApplicationEvaluators.Add(applicationEvaluator);
                context.SaveChanges();
            }

        }

        private void AddApplicationEvaluatorToDatabase(Person person)
        {
            var applicationEvaluator = new ApplicationEvaluator {Person = person};

            using (var context = new AdminsRepositoryDbContext(_dbConnection))
            {
                context.ApplicationEvaluators.Add(applicationEvaluator);
                context.SaveChanges();
            }
        }

        private void AddApplicantToDatabase(Person person)
        {
            var applicant = new Applicant
            {
                Person = person
            };

            using (var context = new ApplicantRepositoryDbContext(_dbConnection))
            {
                context.Applicants.Add(applicant);
                context.SaveChanges();
            }
        }
    }
}
