using System;
using System.Linq;
using AutoMapper;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class AcademicInformationRepository : ApplicantsRepositoryBase, IAcademicInformationRepository
    {
        public AcademicInformationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<AcademicInformationDto, AcademicInformation>();
            Mapper.CreateMap<ClassRankDto, ClassRank>();
            Mapper.CreateMap<AcademicInformation, AcademicInformationDto>();
            Mapper.CreateMap<ClassRank, ClassRankDto>();
        }

        public void UpsertAcademicInformation(AcademicInformationDto academicInformationDto)
        {
            var academicInformationFromClient = Mapper.Map<AcademicInformation>(academicInformationDto);
            var now = DateTime.UtcNow;
            academicInformationFromClient.LastUpdated = now;
            academicInformationFromClient.ClassRank.LastUpdated = now;
            using (var context = GetApplicantsDbContext())
            {
                var person = context.People.First(persons => persons.Guid == ApplicantGuid);
                var academicInformationFromServer = person.Applicant.AcademicInformation;
                if (academicInformationFromServer == null)
                {
                    person.Applicant.AcademicInformation = academicInformationFromClient;
                }
                else
                {
                    academicInformationFromServer.Gpa = academicInformationFromClient.Gpa;
                    academicInformationFromServer.CareerChoice = academicInformationFromClient.CareerChoice;
                    academicInformationFromServer.ProbableNextSchool = academicInformationFromClient.ProbableNextSchool;
                    academicInformationFromServer.LastUpdated = now;

                    if (academicInformationFromServer.ClassRank == null)
                    {
                        academicInformationFromServer.ClassRank = academicInformationFromClient.ClassRank;
                    }
                    else
                    {
                        academicInformationFromServer.ClassRank.LastUpdated = now;
                        academicInformationFromServer.ClassRank.ClassNumericalRank = academicInformationFromClient.ClassRank.ClassNumericalRank;
                        academicInformationFromServer.ClassRank.GraduatingClassSize = academicInformationFromClient.ClassRank.GraduatingClassSize;

                    }
                }
                
                context.SaveChanges();
            }
        }

        public AcademicInformationDto GetAcademicInformation()
        {

            AcademicInformation academicInformation;

            using (var context = GetApplicantsDbContext())
            {
                var person = context.People.First(persons => persons.Guid == ApplicantGuid);
                academicInformation = person.Applicant.AcademicInformation;
                if (academicInformation != null)
                {
                    //Populate the classRank object. 
                    var classRank = academicInformation.ClassRank;
                }
            }
            
            return Mapper.Map<AcademicInformationDto>(academicInformation);
        }
    }
}
