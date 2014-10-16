using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class ApplicantsReferencesRepository : ApplicantsRepositoryBase, IApplicantsReferencesRepository 
    {
        public ApplicantsReferencesRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public ICollection<ReferenceDto> GetReferences()
        {
            var referenceDtos = new List<ReferenceDto>();

            using (var context = GetApplicantsDbContext())
            {
                referenceDtos.AddRange(context.People.Where(person => person.Guid == ApplicantGuid).SelectMany(person => person.Applicant.LettersOfRecommendation)
                    .Select(letterOfRecommendation => 
                    new ReferenceDto
                    {
                        ReferenceEmailAddress = letterOfRecommendation.Reference.Person.ContactInformation.EmailAddress, 
                        ReferenceLetterReceived = letterOfRecommendation.LetterOfRecommendationRowKey != null, 
                        ReferenceName = new NameDto
                        {
                            FirstName = letterOfRecommendation.Reference.Person.Name.FirstName, 
                            LastName = letterOfRecommendation.Reference.Person.Name.LastName
                        }
                    }));
            }

            return referenceDtos;
        }

        public void AddReference(ApplicantReferenceForEntityFrameworkDto referenceInput)
        {
            var letterOfRecommendation = new LetterOfRecommendation
            {
                GuidSentToReference = referenceInput.GuidLink,
                ReferenceRelationshipToApplicant = referenceInput.RelationshipToReference
            };

            using (var context = new ApplicantRepositoryDbContext(DbConnection))
            {
                var contactInformation =
                    context.ContactInformations.FirstOrDefault(
                        contactInformations => contactInformations.EmailAddress == referenceInput.ReferenceEmail);

                var newReference = contactInformation == null ? CreateNewReferencePerson(referenceInput) : contactInformation.Person.Reference;

                letterOfRecommendation.Reference = newReference;

                var applicant = context.Applicants.First(applicants => applicants.Person.Guid == ApplicantGuid);
                applicant.LettersOfRecommendation.Add(letterOfRecommendation);
                context.SaveChanges();
            }
        }

        private Reference CreateNewReferencePerson(ApplicantReferenceForEntityFrameworkDto referenceInput)
        {
            var reference = new Reference
            {
                Person = new Person
                {
                    ContactInformation = new ContactInformation
                    {
                        EmailAddress = referenceInput.ReferenceEmail,
                        LastUpdated = DateTime.UtcNow
                    },
                    Guid = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow
                }
            };
            return reference;
        }
    }
}
