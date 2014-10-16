using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.PersonsRepository.Repositories.Interfaces
{
    public interface IContactInformationRepository
    {
        void UpsertContactInformation(ContactInformationDto contactInformation);
        ContactInformationDto GetContactInformation();
    }
}
