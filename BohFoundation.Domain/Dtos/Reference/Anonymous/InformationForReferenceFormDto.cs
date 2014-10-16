using System;
using BohFoundation.Domain.Dtos.Person;

namespace BohFoundation.Domain.Dtos.Reference.Anonymous
{
    public class InformationForReferenceFormDto
    {
        public NameDto ApplicantsName { get; set; }
        public string ApplicantsRelationshipToYou { get; set; }
        public NameDto ReferencesName { get; set; }
        public DateTime? PhoneNumberLastUpdated { get; set; }
        public bool LetterOfRecommendationAlreadyRecieved { get; set; }
    }
}
