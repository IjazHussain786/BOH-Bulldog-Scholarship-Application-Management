using System.Collections.Generic;
using System.Linq;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Notifications;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class ApplicantsesNotificationRepository : ApplicantsRepositoryBase, IApplicantsNotificationRepository
    {
        private readonly IDeadlineUtilities _deadlineUtilities;

        public ApplicantsesNotificationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters, IDeadlineUtilities deadlineUtilities)
            : base(dbConnection, claimsInformationGetters)
        {
            _deadlineUtilities = deadlineUtilities;
        }

        public ApplicantNotificationsDto GetApplicantNotifications()
        {
            using (var context = GetApplicantsDbContext())
            {
                var notifications = context.People.Where(person => person.Guid == ApplicantGuid)
                    .Select(person => new ApplicantNotificationsDto
                    {
                        LastUpdatedAcademicInformation = person.Applicant.AcademicInformation.LastUpdated,
                        LastUpdatedContactInformation = person.ContactInformation.LastUpdated,
                        LastUpdatedPersonalInformation = person.Applicant.ApplicantPersonalInformation.LastUpdated,
                        LastUpdatedTranscriptUpload = person.Applicant.AcademicInformation.Transcript.LastUpdated,
                        LastUpdatedFamilyInformation = person.Applicant.FamilyInformation.LastUpdated,
                        LastUpdatedExtracurriculars = person.Applicant.ExtracurricularActivities.LastUpdated,
                        LowGradeNotificationInformation = new LowGradeNotificationInformationDto
                        {
                            Gpa = person.Applicant.AcademicInformation.Gpa,
                            LastUpdatedLowGrade =
                                person.Applicant.AcademicInformation.LowGrades.FirstOrDefault().LastUpdated,
                            NumberOfLowGradesInformationSaved = person.Applicant.AcademicInformation.LowGrades.Count
                        },
                        ApplicantReferenceCounts = new ApplicantReferenceCountsDto
                        {
                            NumberOfReferenceInvitationsSent = person.Applicant.LettersOfRecommendation.Count,
                            NumberOfReferencesRecieved = person.Applicant.LettersOfRecommendation.Count(letterOfRecommendation => letterOfRecommendation.LastUpdated != null),
                            LastUpdated = person.Applicant.LettersOfRecommendation.Max(letterOfRecommendation => letterOfRecommendation.LastUpdated)
                        }
                    }).First();

                notifications.EssayNotifications = GetEssayNotificationDtos(context);
                notifications.DeadlineDate = _deadlineUtilities.GetApplicantsDeadlineInUtc();
                
                return notifications;
            }
        }

        private List<EssayNotificationsDto> GetEssayNotificationDtos(ApplicantRepositoryDbContext context)
        {
            var graduatingYearInt = GetApplicantsGraduatingClassYear();
            var graduatingClass = context.GraduatingClasses.FirstOrDefault(graduatingClasses => graduatingClasses.GraduatingYear == graduatingYearInt);
            
            if (graduatingClass == null) return new List<EssayNotificationsDto>();
            
            var topics =
                graduatingClass.EssayTopics;

            return topics.Select(
                topic =>
                {
                    var essayFromDb =
                        context.Essays.FirstOrDefault(
                            essay => essay.EssayTopic.Id == topic.Id && essay.Applicant.Person.Guid == ApplicantGuid);

                    var essayNotificationDto = new EssayNotificationsDto
                    {
                        EssayPrompt = topic.EssayPrompt,
                        TitleOfEssay = topic.TitleOfEssay,
                        EssayTopicId = topic.Id
                    };

                    if (essayFromDb == null)
                    {
                        essayNotificationDto.RevisionDateTime = null;
                    }
                    else
                    {
                        essayNotificationDto.RevisionDateTime = essayFromDb.RevisionDateTime;
                    }
                    return essayNotificationDto;
                })
                .ToList();
        }
    }
}
