using System;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;

namespace BohFoundation.PersonsRepository.Repositories.Interfaces
{
    public interface ICreatePersonRepository
    {
        void CreatePerson(Guid membershipGuid, Name personsName, MemberTypesEnum memberType);
    }
}
