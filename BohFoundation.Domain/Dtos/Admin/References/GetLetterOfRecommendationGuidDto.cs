
namespace BohFoundation.Domain.Dtos.Admin.References
{
    public class GetLetterOfRecommendationGuidDto
    {
        private string _applicantsEmailAddress;
        private string _referencesEmailAddress;

        public string ApplicantsEmailAddress 
        { 
            get { return _applicantsEmailAddress; } 
            set { _applicantsEmailAddress = value.ToLower().Trim(); } 
        }


        public string ReferencesEmailAddress 
        { 
            get { return _referencesEmailAddress; } 
            set { _referencesEmailAddress = value.ToLower().Trim(); } 
        }
    }
}
