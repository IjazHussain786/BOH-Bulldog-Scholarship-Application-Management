using System;
using System.Linq;
using AutoMapper;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Family;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class FamilyInformationRepository : ApplicantsRepositoryBase, IFamilyInformationRepository
    {
        public FamilyInformationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<FamilyInformationDto, FamilyInformation>();
            Mapper.CreateMap<FamilyInformation, FamilyInformationDto>();
        }

        public void UpsertFamilyInformation(FamilyInformationDto familyInformationDto)
        {
            using (var context = GetApplicantsDbContext())
            {
                var familyInformationFromServer = context.People.First(person => person.Guid == ApplicantGuid).Applicant.FamilyInformation;
                if (familyInformationFromServer == null)
                {
                    InsertNewFamilyInformation(familyInformationDto, context);
                }
                else
                {
                    UpdateFamilyInformation(familyInformationDto, familyInformationFromServer);
                }
                context.SaveChanges();
            }
        }

        private void InsertNewFamilyInformation(FamilyInformationDto dto, ApplicantRepositoryDbContext context)
        {
            var familyInformation = Mapper.Map<FamilyInformation>(dto);
            familyInformation.LastUpdated = DateTime.UtcNow;
            context.People.First(person => person.Guid == ApplicantGuid).Applicant.FamilyInformation =
                familyInformation;
        }

        private void UpdateFamilyInformation(FamilyInformationDto dto, FamilyInformation fromServer)
        {
            fromServer.LastUpdated = DateTime.UtcNow;
            fromServer.HighestAttainedDegreeInHome = dto.HighestAttainedDegreeInHome;
            fromServer.NumberOfPeopleInHousehold = dto.NumberOfPeopleInHousehold;
            fromServer.YearlyHouseholdIncomeRange = dto.YearlyHouseholdIncomeRange;
        }

        public FamilyInformationDto GetFamilyInformation()
        {
            FamilyInformation familyInformation;

            using (var context = GetApplicantsDbContext())
            {
                familyInformation = context.People.Where(person => person.Guid == ApplicantGuid).Select(person => person.Applicant.FamilyInformation).FirstOrDefault();
            }

            return Mapper.Map<FamilyInformationDto>(familyInformation);
        }
    }
}
