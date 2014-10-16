using System;
using System.Linq;
using BohFoundation.PersonsRepository.DbContext;
using BohFoundation.PersonsRepository.Repositories.Interfaces;

namespace BohFoundation.PersonsRepository.Repositories.Implementations
{
    public class ChangeEmailRepository : IChangeEmailRepository 
    {
        private readonly string _dbConnection;

        public ChangeEmailRepository(string dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void ChangeEmailAddress(string emailAddress, Guid userGuid)
        {
            using (var context = new PersonsRepositoryDbContext(_dbConnection))
            {
                var contactInformation =
                    context.People.Where(person => person.Guid == userGuid)
                        .Select(person => person.ContactInformation).FirstOrDefault();

                if (contactInformation == null) return;

                contactInformation.EmailAddress = emailAddress;
                contactInformation.LastUpdated = DateTime.UtcNow;

                context.SaveChanges();
            }
        }
    }
}
