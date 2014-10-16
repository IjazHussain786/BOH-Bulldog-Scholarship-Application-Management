using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Extracurricular;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular;
using BohFoundation.Utilities.Context.Interfaces;
using EntityFramework.Extensions;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class ExtracurricularActivitiesRepository : ApplicantsRepositoryBase, IExtracurricularActivitiesRepository
    {
        public ExtracurricularActivitiesRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<ExtracurricularActivities, ExtracurricularActivitiesDto>();
            Mapper.CreateMap<Job, JobDto>();
            Mapper.CreateMap<Activity, ActivityDto>();

            Mapper.CreateMap<JobDto, Job>();
            Mapper.CreateMap<ActivityDto, Activity>();
        }

        public void UpsertExtracurricularActivities(ExtracurricularActivitiesDto extracurricularActivities)
        {
            using (var context = GetApplicantsDbContext())
            {
                var applicant = context.People.First(person => person.Guid == ApplicantGuid).Applicant;

                if (applicant.ExtracurricularActivities == null)
                {
                    CreateNewExtracurriculars(extracurricularActivities, applicant);
                }
                else
                {
                    DeleteOldJobsAndActivities(context);
                    MapToExistingExtracurriculars(extracurricularActivities, applicant);
                }
                context.SaveChanges();
            }
        }

        private void MapToExistingExtracurriculars(ExtracurricularActivitiesDto extracurricularActivities, Applicant applicant)
        {
            applicant.ExtracurricularActivities.LastUpdated = DateTime.UtcNow;

            applicant.ExtracurricularActivities.Jobs = ConvertToJobs(extracurricularActivities);

            applicant.ExtracurricularActivities.Activities = ConvertToActivities(extracurricularActivities);

            applicant.ExtracurricularActivities.HasNonPaidActivities =
                extracurricularActivities.HasNonPaidActivities;

            applicant.ExtracurricularActivities.PaidWork = extracurricularActivities.PaidWork;
        }

        private void CreateNewExtracurriculars(ExtracurricularActivitiesDto extracurricularActivities, Applicant applicant)
        {
            var extracurriculars = new ExtracurricularActivities
            {
                HasNonPaidActivities = extracurricularActivities.HasNonPaidActivities,
                PaidWork = extracurricularActivities.PaidWork,
                LastUpdated = DateTime.UtcNow,
                Jobs = ConvertToJobs(extracurricularActivities),
                Activities = ConvertToActivities(extracurricularActivities)
            };

            applicant.ExtracurricularActivities = extracurriculars;
        }

        private void DeleteOldJobsAndActivities(ApplicantRepositoryDbContext context)
        {
            context.Jobs.Where(job => job.ExtracurricularActivities.Applicant.Person.Guid == ApplicantGuid)
                .Delete();

            context.Activities.Where(
                activity => activity.ExtracurricularActivities.Applicant.Person.Guid == ApplicantGuid)
                .Delete();
        }

        private ICollection<Activity> ConvertToActivities(ExtracurricularActivitiesDto extracurricularActivities)
        {
            return extracurricularActivities.HasNonPaidActivities 
                ? extracurricularActivities.Activities.Select(Mapper.Map<Activity>).ToList()
                : new List<Activity>();
        }

        private ICollection<Job> ConvertToJobs(ExtracurricularActivitiesDto extracurricularActivities)
        {
            return extracurricularActivities.PaidWork 
                ? extracurricularActivities.Jobs.Select(Mapper.Map<Job>).ToList() 
                : new List<Job>();
        }

        public ExtracurricularActivitiesDto GetExtracurricularActivities()
        {
            ExtracurricularActivitiesDto dto;
            
            using (var context = GetApplicantsDbContext())
            {
                var extracurriculars =
                    context.People.First(person => person.Guid == ApplicantGuid).Applicant.ExtracurricularActivities;

                dto = extracurriculars == null ? null : Mapper.Map<ExtracurricularActivitiesDto>(extracurriculars);
            }

            return dto;
        }
    }
}
