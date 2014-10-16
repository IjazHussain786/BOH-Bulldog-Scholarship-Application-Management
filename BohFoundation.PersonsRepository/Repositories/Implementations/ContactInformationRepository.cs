using System;
using System.Linq;
using AutoMapper;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.PersonsRepository.DbContext;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.PersonsRepository.Repositories.Implementations
{
    public class ContactInformationRepository : IContactInformationRepository
    {
        private readonly string _dbConnection;
        private readonly Guid _usersGuid;

        public ContactInformationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters)
        {
            _dbConnection = dbConnection;
            _usersGuid = claimsInformationGetters.GetUsersGuid();

            Mapper.CreateMap<ContactInformationDto, ContactInformation>();
            Mapper.CreateMap<AddressDto, Address>();
            Mapper.CreateMap<PhoneInformationDto, PhoneInformation>();

            Mapper.CreateMap<ContactInformation, ContactInformationDto>();
            Mapper.CreateMap<Address, AddressDto>();
            Mapper.CreateMap<PhoneInformation, PhoneInformationDto>();
        }

        public void UpsertContactInformation(ContactInformationDto contactInformation)
        {
            var contactInformationFromUser = CreateContactInformation(contactInformation);

            using (var context = new PersonsRepositoryDbContext(_dbConnection))
            {
                var personFromDb = context.People.First(person => person.Guid == _usersGuid);
                var contactInformationFromServer = personFromDb.ContactInformation;

                if (contactInformationFromServer == null)
                {
                    personFromDb.ContactInformation = contactInformationFromUser;
                }
                else
                {
                    MapFromUserToDb(contactInformationFromServer, contactInformationFromUser);
                }
                context.SaveChanges();
            }
        }

        private void MapFromUserToDb(ContactInformation contactInformationFromServer, ContactInformation contactInformationFromUser)
        {
            var now = DateTime.UtcNow;
            contactInformationFromServer.EmailAddress = contactInformationFromUser.EmailAddress;
            contactInformationFromServer.LastUpdated = now;

            contactInformationFromServer.Address.LastUpdated = now;
            contactInformationFromServer.Address.StreetAddress1 = contactInformationFromUser.Address.StreetAddress1;
            contactInformationFromServer.Address.StreetAddress2 = contactInformationFromUser.Address.StreetAddress2;
            contactInformationFromServer.Address.ZipCode = contactInformationFromUser.Address.ZipCode;
            contactInformationFromServer.Address.State = contactInformationFromUser.Address.State;
            contactInformationFromServer.Address.City = contactInformationFromUser.Address.City;

            contactInformationFromServer.PhoneInformation.PhoneNumber =
                contactInformationFromUser.PhoneInformation.PhoneNumber;
            contactInformationFromServer.PhoneInformation.BestTimeToContactByPhone =
                contactInformationFromUser.PhoneInformation.BestTimeToContactByPhone;
            contactInformationFromServer.PhoneInformation.LastUpdated = now;
        }

        private ContactInformation CreateContactInformation(ContactInformationDto contactInformation)
        {
            var mappedContactInfo = Mapper.Map<ContactInformation>(contactInformation);
            
            var timeNow = DateTime.UtcNow;
            mappedContactInfo.LastUpdated = timeNow;
            mappedContactInfo.Address.LastUpdated = timeNow;
            mappedContactInfo.PhoneInformation.LastUpdated = timeNow;
            
            return mappedContactInfo;
        }

        public ContactInformationDto GetContactInformation()
        {
            ContactInformation contactInformationFromServer;

            using (var context = new PersonsRepositoryDbContext(_dbConnection))
            {
                var personFromServer = context.People.First(person => person.Guid == _usersGuid);
                if (personFromServer.ContactInformation == null)
                {
                    return null;
                }
                contactInformationFromServer = personFromServer.ContactInformation;
                var addressFromServer = contactInformationFromServer.Address;
                var phoneInformation = contactInformationFromServer.PhoneInformation;
            }

            var contactDto = Mapper.Map<ContactInformationDto>(contactInformationFromServer);

            return contactDto;

        }
    }
}
