using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.RatingSummary;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.Enums;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations
{
    public class GetTopRatedApplicantsRepository : ApplicationEvaluatorsRepositoryBase, IGetTopRatedApplicantsRepository
    {
        public GetTopRatedApplicantsRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public Top5ApplicantsDto GetTop5Applicants(int graduatingClass)
        {
            var top5ApplicantsDto = new Top5ApplicantsDto();

            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                top5ApplicantsDto.TopApplicants = context.Applicants
                    .Where(applicant =>
                        applicant.ApplicantPersonalInformation.GraduatingClass.GraduatingYear == graduatingClass)
                    .Select(applicant => new TopApplicantRatingSummaryDto
                    {
                        ApplicantsName  = new NameDto
                        {
                            FirstName   = applicant.Person.Name.FirstName,
                            LastName    = applicant.Person.Name.LastName
                        },
                        ApplicantsGuid  = applicant.Person.Guid,
                        AverageRating   = (RatingEnum)(applicant.ApplicantRatings.Average(applicantRating => (int)applicantRating.OverallRating.RatingEnum)),
                        ApplicantEvaluatorsRatings = applicant.ApplicantRatings
                            .Where(applicantRating => applicantRating.OverallRating != null)
                            .Select(applicantRating => new IndividualRatingDto
                            {
                                ApplicantEvaluatorsName = new NameDto
                                {
                                    FirstName   = applicantRating.ApplicationEvaluator.Person.Name.FirstName,
                                    LastName    = applicantRating.ApplicationEvaluator.Person.Name.LastName
                                },
                                OverallRating   = new GenericRatingDto
                                {
                                    Explanation = applicantRating.OverallRating.Explanation,
                                    RatingEnum  = applicantRating.OverallRating.RatingEnum 
                                } 
                            })
                        .OrderBy(applicantEvaluatorsRating => applicantEvaluatorsRating.ApplicantEvaluatorsName.FirstName)
                        .ToList()
                        
                    }).OrderByDescending(applicant => applicant.AverageRating).Take(5).ToList();
            }

            return top5ApplicantsDto;
        }
    }
}
