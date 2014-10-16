using System.Collections.ObjectModel;
using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.DbContext;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations
{
    public class RateApplicationRespository : ApplicationEvaluatorsRepositoryBase, IRateApplicationRespository
    {
        public RateApplicationRespository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public void UpsertEssayRating(EssayRatingDto essayRatingDto)
        {
            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                var applicantRating = GetApplicantRatingFromDbOrInitializeOne(new RatingWithApplicantsGuidDto{ApplicantsGuid = essayRatingDto.ApplicantsGuid, Rating = essayRatingDto.Rating}, context);
                
                if (applicantRating.EssayRatings == null)
                {
                    applicantRating.EssayRatings = new Collection<EssayRating>();
                }

                var essayRating =
                    applicantRating.EssayRatings.FirstOrDefault(
                        essayRatings => essayRatings.Essay.EssayTopic.Id == essayRatingDto.EssayTopicId);

                if (essayRating == null)
                {
                    essayRating = new EssayRating
                    {
                        Essay = context.Essays.First(essay => essay.EssayTopic.Id == essayRatingDto.EssayTopicId),
                        Rating = new GenericRating
                        {
                            Explanation = essayRatingDto.Rating.Explanation,
                            RatingEnum = essayRatingDto.Rating.RatingEnum
                        }
                    };

                    applicantRating.EssayRatings.Add(essayRating);
                }
                else
                {
                    essayRating.Rating.RatingEnum = essayRatingDto.Rating.RatingEnum;
                    essayRating.Rating.Explanation = essayRatingDto.Rating.Explanation;
                }
                context.SaveChanges();
            }
        }

        public void UpsertFirstImpression(RatingWithApplicantsGuidDto firstImpressionRatingDto)
        {
            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                var applicantRating = GetApplicantRatingFromDbOrInitializeOne(firstImpressionRatingDto, context);

                if (applicantRating.FirstImpressionRating == null)
                {
                    applicantRating.FirstImpressionRating = new GenericRating
                    {
                        Explanation = firstImpressionRatingDto.Rating.Explanation,
                        RatingEnum = firstImpressionRatingDto.Rating.RatingEnum
                    };
                }
                else
                {
                    applicantRating.FirstImpressionRating.RatingEnum = firstImpressionRatingDto.Rating.RatingEnum;
                    applicantRating.FirstImpressionRating.Explanation = firstImpressionRatingDto.Rating.Explanation;
                }

                context.SaveChanges();
            }
        }

        public void UpsertFinalOverallRating(RatingWithApplicantsGuidDto finalOverallRatingDto)
        {
            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                var applicantRating = GetApplicantRatingFromDbOrInitializeOne(finalOverallRatingDto, context);

                if (applicantRating.OverallRating == null)
                {
                    applicantRating.OverallRating = new GenericRating
                    {
                        Explanation = finalOverallRatingDto.Rating.Explanation,
                        RatingEnum = finalOverallRatingDto.Rating.RatingEnum
                    };
                }
                else
                {
                    applicantRating.OverallRating.RatingEnum = finalOverallRatingDto.Rating.RatingEnum;
                    applicantRating.OverallRating.Explanation = finalOverallRatingDto.Rating.Explanation;
                }

                context.SaveChanges();
            }    
        }

        private ApplicantRating GetApplicantRatingFromDbOrInitializeOne(RatingWithApplicantsGuidDto dto,
            ApplicationEvaluatorRepositoryDbContext context)
        {
            var applicantRating =
                context.ApplicantRatings.FirstOrDefault(
                    applicantRatings =>
                        applicantRatings.Applicant.Person.Guid == dto.ApplicantsGuid &&
                        applicantRatings.ApplicationEvaluator.Person.Guid == ApplicationEvaluatorsGuid);

            if (applicantRating != null) return applicantRating;

            applicantRating = CreateNewApplicantRating(dto, context);
            context.ApplicantRatings.Add(applicantRating);
            
            return applicantRating;
        }

        private ApplicantRating CreateNewApplicantRating(RatingWithApplicantsGuidDto dto, ApplicationEvaluatorRepositoryDbContext context)
        {
            return new ApplicantRating
            {
                Applicant = context.Applicants.First(applicants => applicants.Person.Guid == dto.ApplicantsGuid),
                ApplicationEvaluator =
                    context.ApplicationEvaluators.First(
                        applicationEvaluator => applicationEvaluator.Person.Guid == ApplicationEvaluatorsGuid)
            };
        }
    }
}
