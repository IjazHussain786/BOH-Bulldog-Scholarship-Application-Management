using System;
using BohFoundation.Domain.Dtos.Applicant.Family;
using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication
{
    public class ApplicantsGeneralInformationDto
    {
        public NameDto Name { get; set; }
        public ContactInformationDto ContactInformation { get; set; }
        public FamilyInformationDto FamilyInformation { get; set; }
        public ApplicantPersonalInformationDto PersonalInformation { get; set; }

        public GenericRatingDto YourFirstImpressionRating { get; set; }
        public GenericRatingDto YourFinalOverallRating { get; set; }
        public Guid ApplicantGuid { get; set; }
    }
}
