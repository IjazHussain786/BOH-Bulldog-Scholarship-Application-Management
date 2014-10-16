using System;
using System.Linq;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.References;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.AdminsRepository.Repositories.Implementation
{
    public class GetLetterOfRecommendationGuidRepository : AdminsRepoBase, IGetLetterOfRecommendationGuidRepository
    {
        public GetLetterOfRecommendationGuidRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public GuidSentToReferenceDto GetLetterOfRecommendationGuid(GetLetterOfRecommendationGuidDto dto)
        {
            GuidSentToReferenceDto guidSendSentToReferenceDto;

            using (var context = GetAdminsRepositoryDbContext())
            {
                var applicant =
                    context.Applicants.FirstOrDefault(
                        applicants => applicants.Person.ContactInformation.EmailAddress == dto.ApplicantsEmailAddress);


                if (applicant == null) return new GuidSentToReferenceDto { ErrorMessage = "No applicant with that email address is in the db." };

                var letterOfRecommendation =
                    applicant.LettersOfRecommendation.FirstOrDefault(
                        letter => letter.Reference.Person.ContactInformation.EmailAddress == dto.ReferencesEmailAddress);

                if (letterOfRecommendation == null) return new GuidSentToReferenceDto { ErrorMessage = "That applicant doesn't have a letter of recommendation started from that reference." };

                guidSendSentToReferenceDto = new GuidSentToReferenceDto
                {
                    AlreadyUpdated = letterOfRecommendation.LastUpdated != null,
                    GuidSentToReference = letterOfRecommendation.GuidSentToReference
                };
            }

            return guidSendSentToReferenceDto;
        }
    }
}
