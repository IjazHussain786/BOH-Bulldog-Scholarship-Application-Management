using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations
{
    public class GetListOfFinalizedApplicantsRepository : ApplicationEvaluatorsRepositoryBase, IGetListOfFinalizedApplicantsRepository
    {
        public GetListOfFinalizedApplicantsRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public AllFinalizedApplicantsForAGraduatingYearDto GetAllFinalizedApplicantsByGraduatingYear(int year)
        {
            var dto = new AllFinalizedApplicantsForAGraduatingYearDto {GraduatingYear = year};
            
            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                dto.ApplicantSummaries =
                    context.Applicants.Where(
                        
                    applicants => 
                            applicants.ApplicantPersonalInformation.GraduatingClass.GraduatingYear == year && 
                            applicants.Metadata.ApplicationFinalized && 
                            !applicants.Metadata.NotCompletedButFinalizedApplication)

                        .Select(applicant => new ApplicantSummaryDto
                        {
                            ApplicantName = new NameDto
                            {
                                FirstName = applicant.Person.Name.FirstName,
                                LastName = applicant.Person.Name.LastName
                            },
                            ApplicantGuid = applicant.Person.Guid,
                            Gpa = applicant.AcademicInformation.Gpa,
                            IncomeRange = applicant.FamilyInformation.YearlyHouseholdIncomeRange,
                            YourRating = applicant.ApplicantRatings.FirstOrDefault(rating => rating.ApplicationEvaluator.Person.Guid == ApplicationEvaluatorsGuid).OverallRating.RatingEnum
                        })
                        .OrderBy(x => x.ApplicantName.LastName)
                        .ToList();
            }

            return dto;
        }
    }
}
