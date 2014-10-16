using System;
using System.Linq;
using BohFoundation.ApplicationEvaluatorsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.Domain.Dtos.Applicant.Family;
using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.Essay;
using BohFoundation.Domain.Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.LetterOfRecommendation;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicationEvaluatorsRepository.Repositories.Implementations
{
    public class GetCompletedApplicationRepository : ApplicationEvaluatorsRepositoryBase, IGetCompletedApplicationRepository
    {
        public GetCompletedApplicationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
        }

        public ApplicantsGeneralInformationDto GetGeneralInformation(Guid applicantsGuid)
        {
            ApplicantsGeneralInformationDto dto;

            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                dto = context.Applicants

                    .Where(applicant => applicant.Person.Guid == applicantsGuid)
                    .Select(applicant => new ApplicantsGeneralInformationDto
                    {
                       Name = new NameDto
                        {
                            FirstName = applicant.Person.Name.FirstName,
                            LastName = applicant.Person.Name.LastName
                        },

                        ContactInformation = new ContactInformationDto
                        {
                            EmailAddress = applicant.Person.ContactInformation.EmailAddress,
                            PhoneInformation = new PhoneInformationDto
                            {
                                BestTimeToContactByPhone =
                                    applicant.Person.ContactInformation.PhoneInformation.BestTimeToContactByPhone,
                                PhoneNumber = applicant.Person.ContactInformation.PhoneInformation.PhoneNumber
                            },
                            Address = new AddressDto
                            {
                                City            = applicant.Person.ContactInformation.Address.City,
                                State           = applicant.Person.ContactInformation.Address.State,
                                ZipCode         = applicant.Person.ContactInformation.Address.ZipCode,
                                StreetAddress1  = applicant.Person.ContactInformation.Address.StreetAddress1,
                                StreetAddress2  = applicant.Person.ContactInformation.Address.StreetAddress2
                            }
                        },

                        FamilyInformation = new FamilyInformationDto
                        {
                            HighestAttainedDegreeInHome = applicant.FamilyInformation.HighestAttainedDegreeInHome,
                            YearlyHouseholdIncomeRange  = applicant.FamilyInformation.YearlyHouseholdIncomeRange,
                            NumberOfPeopleInHousehold   = applicant.FamilyInformation.NumberOfPeopleInHousehold
                        },

                        PersonalInformation = new ApplicantPersonalInformationDto
                        {
                            Birthdate       = applicant.ApplicantPersonalInformation.Birthdate,
                            GraduatingYear  = applicant.ApplicantPersonalInformation.GraduatingClass.GraduatingYear
                        },

                       YourFinalOverallRating = applicant.ApplicantRatings
                           .Where(applicantRatings => applicantRatings.ApplicationEvaluator.Person.Guid == ApplicationEvaluatorsGuid)
                           .Select(applicantRating => new GenericRatingDto
                           {
                               Explanation = applicantRating.OverallRating.Explanation,
                               RatingEnum = applicantRating.OverallRating.RatingEnum
                           }).FirstOrDefault(),

                        YourFirstImpressionRating = applicant.ApplicantRatings
                            .Where(applicantRatings => applicantRatings.ApplicationEvaluator.Person.Guid == ApplicationEvaluatorsGuid)
                            .Select(applicantRating => new GenericRatingDto
                            {
                                Explanation = applicantRating.FirstImpressionRating.Explanation,
                                RatingEnum = applicantRating.FirstImpressionRating.RatingEnum
                            }).FirstOrDefault(),

                    ApplicantGuid = applicantsGuid
                    
                    }).First();
            }
            
            return dto;
        }

        public ExtracurricularActivitiesDto GetExtracurricularActivities(Guid applicantsGuid)
        {
            ExtracurricularActivitiesDto dto;

            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                dto =
                    context.Applicants.Where(applicant => applicant.Person.Guid == applicantsGuid)
                        .Select(applicant => new ExtracurricularActivitiesDto
                         {                       
                            HasNonPaidActivities    = applicant.ExtracurricularActivities.HasNonPaidActivities,
                            PaidWork                = applicant.ExtracurricularActivities.PaidWork,

                            Jobs = applicant.ExtracurricularActivities.Jobs.Select(job => new JobDto
                            {
                                Employer = job.Employer,
                                Position = job.Position,
                                ShortSummaryOfWorkResponsibilities = job.ShortSummaryOfWorkResponsibilities
                            })
                            .OrderBy(job => job.Employer)
                            .ToList(),

                            Activities =
                                applicant.ExtracurricularActivities.Activities.Select(activity => new ActivityDto
                                {
                                    Name = activity.Name,
                                    ShortSummaryOfWhatIsInvolved = activity.ShortSummaryOfWhatIsInvolved
                                })
                                .OrderBy(activity => activity.Name)
                                .ToList()
                        }).First();
            }

            return dto;
        }

        public CompletedAcademicInformationDto GetAcademicInformation(Guid applicantsGuid)
        {
            CompletedAcademicInformationDto dto;

            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                dto =
                    context.Applicants.Where(applicant => applicant.Person.Guid == applicantsGuid)
                        .Select(applicant => new CompletedAcademicInformationDto
                        {
                            BasicAcademicInformation = new AcademicInformationDto
                            {
                                ClassRank = new ClassRankDto
                                {
                                    GraduatingClassSize = applicant.AcademicInformation.ClassRank.GraduatingClassSize,
                                    ClassNumericalRank  = applicant.AcademicInformation.ClassRank.ClassNumericalRank
                                },
                                Gpa                 = applicant.AcademicInformation.Gpa,
                                CareerChoice        = applicant.AcademicInformation.CareerChoice,
                                ProbableNextSchool  = applicant.AcademicInformation.ProbableNextSchool
                            },

                            LowGrades = applicant.AcademicInformation.LowGrades.Select(lowGrade => new LowGradeDto
                            {
                                ClassTitle          = lowGrade.ClassTitle,
                                Explanation         = lowGrade.Explanation,
                                Grade               = lowGrade.Grade,
                                WasAp               = lowGrade.WasAp,
                                YearOfHighSchool    = lowGrade.YearOfHighSchool
                            })
                            .OrderBy(lowGrade => lowGrade.ClassTitle)
                            .ToList(),

                            TranscriptBlobReference = new TranscriptBlobReferenceDto
                            {
                                BlobContainerName           = applicant.AcademicInformation.Transcript.BlobContainerName,
                                ReferenceToTranscriptPdf    = applicant.AcademicInformation.Transcript.ReferenceToTranscriptPdf
                            },

                            TranscriptDoesNotMatchDatabaseValues    = applicant.Metadata.TranscriptDoesNotMatchDatabaseValues,
                            TranscriptMatchesDatabaseValues         = applicant.Metadata.TranscriptMatchesDatabaseValues
                        }).First();
            }

            return dto;
        }

        public CollectionsOfEssaysAndLettersOfRecommendationDto GetCollectionsOfEssaysAndLettersOfRecommendation(Guid applicantsGuid)
        {
            CollectionsOfEssaysAndLettersOfRecommendationDto dto;

            using (var context = GetApplicationEvaluatorRepositoryDbContext())
            {
                dto =
                    context.Applicants.Where(applicant => applicant.Person.Guid == applicantsGuid)
                        .Select(applicant => new CollectionsOfEssaysAndLettersOfRecommendationDto
                        {
                            EssaySummaries = applicant.Essays.Select(essay => new EssaySummaryDto
                            {
                                EssayTopicId            = essay.EssayTopic.Id,
                                TitleOfEssay            = essay.EssayTopic.TitleOfEssay,
                                EssayPrompt             = essay.EssayTopic.EssayPrompt,
                                EssayCharacterLength    = essay.CharacterLength,
                                EssayKeyValues = new AzureTableStorageEntityKeyDto
                                {
                                    PartitionKey = essay.PartitionKey,
                                    RowKey = essay.RowKey
                                }
                            })
                            .OrderBy(essaySummary => essaySummary.TitleOfEssay)
                            .ToList(),

                            LetterOfRecommendationSummaries =
                                applicant.LettersOfRecommendation.Select(
                                    letterOfRecommendation => new LetterOfRecommendationSummaryDto
                                    {
                                        ReferencePersonalInformation = new ReferencePersonalInformationDto
                                        {
                                            Name = new NameDto
                                            {
                                                FirstName = letterOfRecommendation.Reference.Person.Name.FirstName,
                                                LastName = letterOfRecommendation.Reference.Person.Name.LastName
                                            },
                                            PhoneInformationDto = new PhoneInformationDto
                                            {
                                                BestTimeToContactByPhone =
                                                    letterOfRecommendation.Reference.Person.ContactInformation
                                                        .PhoneInformation.BestTimeToContactByPhone,
                                                PhoneNumber =
                                                    letterOfRecommendation.Reference.Person.ContactInformation
                                                        .PhoneInformation.PhoneNumber
                                            },
                                            Occupation = letterOfRecommendation.Reference.Occupation,
                                            EmailAddress =
                                                letterOfRecommendation.Reference.Person.ContactInformation.EmailAddress
                                        },
                                        ReferenceRelationshipToApplicant =
                                            letterOfRecommendation.ReferenceRelationshipToApplicant,
                                        LetterOfRecommendationKeyValues = new AzureTableStorageEntityKeyDto
                                        {
                                            PartitionKey = letterOfRecommendation.LetterOfRecommendationPartitionKey,
                                            RowKey = letterOfRecommendation.LetterOfRecommendationRowKey
                                        }
                                    })
                                    .OrderBy(
                                        letterOfRecommendation =>
                                            letterOfRecommendation.ReferencePersonalInformation.Name.LastName)
                                    .ToList()
                        }).First();

               
                //There was a problem in the queury where it would return nulls if there was more than one applicant. This is how I solved the problem. Not a great solution. todo make better.
                foreach (var essaySummary in dto.EssaySummaries)
                {
                    var applicantEvaluator =
                        context.ApplicationEvaluators.First(
                            applicantEvaluators => applicantEvaluators.Person.Guid == ApplicationEvaluatorsGuid);
                    
                    var applicantsRatings = applicantEvaluator.ApplicantRating.FirstOrDefault(applicantRating => applicantRating.Applicant.Person.Guid == applicantsGuid);

                    if (applicantsRatings == null) return dto;
                    
                    essaySummary.YourRating =
                        applicantsRatings.EssayRatings.Where(
                            essayRating => essayRating.Essay.EssayTopic.Id == essaySummary.EssayTopicId)
                            .Select(essayRating => new GenericRatingDto
                            {
                                Explanation = essayRating.Rating.Explanation,
                                RatingEnum  = essayRating.Rating.RatingEnum
                            }).FirstOrDefault();
                }
            }

            return dto;
        }
    }
}
