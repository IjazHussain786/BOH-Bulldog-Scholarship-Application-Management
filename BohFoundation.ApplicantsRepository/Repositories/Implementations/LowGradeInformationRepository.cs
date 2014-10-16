using System;
using System.Collections.Generic;
using EntityFramework.Extensions;
using System.Linq;
using AutoMapper;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class LowGradeInformationRepository : ApplicantsRepositoryBase, ILowGradeInformationRepository
    {
        public LowGradeInformationRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<LowGradeDto, LowGrade>();
            Mapper.CreateMap<LowGrade, LowGradeDto>();
        }

        public void UpsertLowGradeInformation(ICollection<LowGradeDto> lowGradeDtos)
        {
            var lowGrades = ConvertToLowGrades(lowGradeDtos);

            using (var context = GetApplicantsDbContext())
            {
                context.LowGrades.Where(lowGrade => lowGrade.AcademicInformation.Applicant.Person.Guid == ApplicantGuid).Delete();

                var academicInformation = context.AcademicInformations.First(
                    academicInformations => academicInformations.Applicant.Person.Guid == ApplicantGuid);

                academicInformation.LowGrades = lowGrades;

                context.SaveChanges();
            }
        }

        private List<LowGrade> ConvertToLowGrades(IEnumerable<LowGradeDto> lowGradeDtos)
        {
            var lowGrades = lowGradeDtos.Select(Mapper.Map<LowGrade>).ToList();

            var now = DateTime.UtcNow;

            foreach (var lowGrade in lowGrades)
            {
                lowGrade.LastUpdated = now;
            }

            return lowGrades;
        }

        public LowGradesWithGpaDto GetLowGradeInformation()
        {
            LowGradesWithGpaDto lowGradesWithGpa;

            using (var context = GetApplicantsDbContext())
            {
                var lowGrades =
                    context.LowGrades.Where(
                        lowGrade => lowGrade.AcademicInformation.Applicant.Person.Guid == ApplicantGuid).Select(Mapper.Map<LowGradeDto>).ToList();

                var gpa =
                    context.AcademicInformations.First(person => person.Applicant.Person.Guid == ApplicantGuid).Gpa;

                lowGradesWithGpa = new LowGradesWithGpaDto {Gpa = gpa, LowGrades = lowGrades};

            }
            return lowGradesWithGpa;
        }
    }
}
