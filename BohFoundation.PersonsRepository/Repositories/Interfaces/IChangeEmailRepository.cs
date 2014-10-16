using System;

namespace BohFoundation.PersonsRepository.Repositories.Interfaces
{
    public interface IChangeEmailRepository
    {
        void ChangeEmailAddress(string emailAddress, Guid usersGuid);
    }
}
