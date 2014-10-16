using System;
using System.Linq;
using AutoMapper;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.ReferencesRepository.DbContext;
using BohFoundation.ReferencesRepository.Repositories.Interfaces;

namespace BohFoundation.ReferencesRepository.Repositories.Implementations
{
    public class AnonymousLetterOfRecommendationRepository : IAnonymousLetterOfRecommendationRepository, ILetterOfRecommendationRowValueSqlRepository
    {
        private readonly string _dbConnection;

        public AnonymousLetterOfRecommendationRepository(string dbConnection)
        {
            _dbConnection = dbConnection;

            Mapper.CreateMap<NameDto, Name>();
            Mapper.CreateMap<PhoneInformationDto, PhoneInformation>();
        }

        public InformationForReferenceFormDto GetInformationForReferenceForm(GuidForLetterOfRecommendationDto model)
        {
            InformationForReferenceFormDto informationForReferencesFormDto;

            using (var context = GetReferencesRepositoryDbContext())
            {
                informationForReferencesFormDto =
                    context.LettersOfRecommendation.Where(
                        letterOfRecommendation =>
                            letterOfRecommendation.GuidSentToReference == model.GuidSentToReference)
                        .Select(letterOfRecommendation => new InformationForReferenceFormDto
                        {
                            ApplicantsName = new NameDto
                            {
                                FirstName = letterOfRecommendation.Applicant.Person.Name.FirstName,
                                LastName = letterOfRecommendation.Applicant.Person.Name.LastName
                            },
                            ApplicantsRelationshipToYou = letterOfRecommendation.ReferenceRelationshipToApplicant,
                            ReferencesName = new NameDto
                            {
                                FirstName = letterOfRecommendation.Reference.Person.Name.FirstName,
                                LastName = letterOfRecommendation.Reference.Person.Name.LastName
                            },
                            PhoneNumberLastUpdated =
                                letterOfRecommendation.Reference.Person.ContactInformation.PhoneInformation.LastUpdated,
                            LetterOfRecommendationAlreadyRecieved =
                                letterOfRecommendation.LetterOfRecommendationRowKey != null
                        }).First();
            }

            return informationForReferencesFormDto;
        }

        public void UpsertReferencesPersonalInformation(ReferencePersonalInformationDto model)
        {
            var now = DateTime.UtcNow;

            using (var context = GetReferencesRepositoryDbContext())
            {
                var reference =
                    context.LettersOfRecommendation.First(
                        letterOfRecommendation =>
                            letterOfRecommendation.GuidSentToReference == model.GuidSentToReference).Reference;
                
                reference.Occupation = model.Occupation;

                UpsertName(reference, model);
                UpsertPhoneInformation(reference, model);
                
                reference.Person.Name.LastUpdated = now;
                reference.Person.ContactInformation.PhoneInformation.LastUpdated = now;
                
                context.SaveChanges();
            }
        }

        private void UpsertPhoneInformation(Reference reference, ReferencePersonalInformationDto model)
        {
            if (reference.Person.ContactInformation.PhoneInformation == null)
            {
                reference.Person.ContactInformation.PhoneInformation = Mapper.Map<PhoneInformation>(model.PhoneInformationDto);
            }
            else
            {
                reference.Person.ContactInformation.PhoneInformation.BestTimeToContactByPhone = model.PhoneInformationDto.BestTimeToContactByPhone;
                reference.Person.ContactInformation.PhoneInformation.PhoneNumber = model.PhoneInformationDto.PhoneNumber;
            }
        }

        private void UpsertName(Reference reference, ReferencePersonalInformationDto model)
        {
            if (reference.Person.Name == null)
            {
                reference.Person.Name = Mapper.Map<Name>(model.Name); 
            }
            else
            {
                reference.Person.Name.FirstName = model.Name.FirstName;
                reference.Person.Name.LastName = model.Name.LastName;
            }
        }

        public void UpsertLetterOfRecommendationKeyValues(LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto model)
        {
            using (var context = GetReferencesRepositoryDbContext())
            {
                var letterOfRecommendation = context.LettersOfRecommendation.First(
                    letter => letter.GuidSentToReference == model.LetterOfRecommendationGuid);

                letterOfRecommendation.LastUpdated = DateTime.UtcNow;
                letterOfRecommendation.LetterOfRecommendationRowKey = model.RowKey;
                letterOfRecommendation.LetterOfRecommendationPartitionKey = model.PartitionKey;

                context.SaveChanges();
            }
        }

        private ReferencesRepositoryDbContext GetReferencesRepositoryDbContext()
        {
            return new ReferencesRepositoryDbContext(_dbConnection);
        }
    }
}
