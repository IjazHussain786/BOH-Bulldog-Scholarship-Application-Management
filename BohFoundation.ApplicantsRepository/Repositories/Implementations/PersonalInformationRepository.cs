using System;
using System.Linq;
using AutoMapper;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.PersonalInformation;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class PersonalInformationRepository : ApplicantsRepositoryBase, IPersonalInformationRepository
    {
        public PersonalInformationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<ApplicantPersonalInformation, ApplicantPersonalInformationDto>();
        }

        public void UpsertPersonalInformation(ApplicantPersonalInformationDto applicantPersonalInformationDto)
        {
            var personalInformationFromUser = CreatePersonalInformation(applicantPersonalInformationDto);

            using (var context = GetApplicantsDbContext())
            {
                var personFromDb = context.People.First(person => person.Guid == ApplicantGuid);
                var applicantInformation = personFromDb.Applicant.ApplicantPersonalInformation;
                if (applicantInformation == null)
                {
                    personFromDb.Applicant.ApplicantPersonalInformation = personalInformationFromUser;
                }
                else
                {
                    applicantInformation.Birthdate = personalInformationFromUser.Birthdate;
                    applicantInformation.LastUpdated = personalInformationFromUser.LastUpdated;
                }

                //The Graduating Classes Are Unique! So, we need to queury it and then add it to the object. If it doesn't exist, we create one for that year. 
                var graduatingClass =
                    context.GraduatingClasses.FirstOrDefault(
                        gc =>
                            gc.GraduatingYear == applicantPersonalInformationDto.GraduatingYear);

                personFromDb.Applicant.ApplicantPersonalInformation.GraduatingClass = graduatingClass ??
                                                       new GraduatingClass { GraduatingYear = applicantPersonalInformationDto.GraduatingYear };

                context.SaveChanges();
            }
        }

        private ApplicantPersonalInformation CreatePersonalInformation(ApplicantPersonalInformationDto applicantPersonalInformationDto)
        {
            var personalInformationFromUser = new ApplicantPersonalInformation
            {
                Birthdate = MakeBirthdayHaveNoHours(applicantPersonalInformationDto),
                LastUpdated = DateTime.UtcNow
            };
            return personalInformationFromUser;
        }

        private DateTime MakeBirthdayHaveNoHours(ApplicantPersonalInformationDto personalInformation)
        {
            return new DateTime(personalInformation.Birthdate.Year, personalInformation.Birthdate.Month, personalInformation.Birthdate.Day);
        }

        public ApplicantPersonalInformationDto GetPersonalInformation()
        {
            ApplicantPersonalInformation applicantPersonalInformation;
            var graduatingYear = 0;

            using (var context = GetApplicantsDbContext())
            {
                applicantPersonalInformation =
                    context.People.First(person => person.Guid == ApplicantGuid).Applicant.ApplicantPersonalInformation;
                
                if (applicantPersonalInformation != null)
                {
                    if (applicantPersonalInformation.GraduatingClass != null)
                    {
                        graduatingYear = applicantPersonalInformation.GraduatingClass.GraduatingYear;
                    }
                }
            }

            var applicantPersonalInformationDto =
                Mapper.Map<ApplicantPersonalInformationDto>(applicantPersonalInformation);

            if (graduatingYear != 0)
            {
                applicantPersonalInformationDto.GraduatingYear = graduatingYear;
            }
            return applicantPersonalInformationDto;
        }
    }
}
