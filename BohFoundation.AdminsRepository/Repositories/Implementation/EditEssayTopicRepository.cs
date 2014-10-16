using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BohFoundation.AdminsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;
using BohFoundation.Domain.Dtos.Person;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.AdminsRepository.Repositories.Implementation
{
    public class EditEssayTopicRepository : AdminsRepoBase, IEditEssayTopicRepository
    {
        public EditEssayTopicRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<CreateAndModifyEssayTopicDto, EssayTopic>();
        }

        public void UpsertEssayTopic(CreateAndModifyEssayTopicDto dto)
        {
            if (dto.Id == 0)
            {
                AddNewEssayTopic(dto);
            }
            else
            {
                EditEssayTopic(dto);
            }
        }

        public void AddEssayTopicToGraduatingYear(EditEssayTopicByGraduatingClassDto dto)
        {
            using (var context = GetAdminsRepositoryDbContext())
            {
                var graduatingYear =
                    context.GraduatingClasses.FirstOrDefault(
                        graduatingClass => graduatingClass.GraduatingYear == dto.ClassYear) ??
                    new GraduatingClass { GraduatingYear = dto.ClassYear };

                var topic = context.EssayTopics.First(essayTopic => essayTopic.Id == dto.EssayId);

                topic.ForWhatGraduatingYears.Add(graduatingYear);
                context.SaveChanges();
            }
        }

        public void DeleteEssayTopicFromGraduatingYear(EditEssayTopicByGraduatingClassDto dto)
        {
            using (var context = GetAdminsRepositoryDbContext())
            {
                var topic = context.EssayTopics.First(essayTopic => essayTopic.Id == dto.EssayId);
                var classYearToRemove =
                    topic.ForWhatGraduatingYears.FirstOrDefault(classYear => classYear.GraduatingYear == dto.ClassYear);

                topic.ForWhatGraduatingYears.Remove(classYearToRemove);
                
                context.SaveChanges();
            }
        }

        public ICollection<EssayTopicDto> GetEssayTopics()
        {
            var essayTopicDtos = new List<EssayTopicDto>();
            using (var context = GetAdminsRepositoryDbContext())
            {
                var essayTopics = context.EssayTopics.Where(topic => topic.Id > 0).ToList();

                essayTopicDtos.AddRange(essayTopics.Select(MapEssayTopicToDto));
            }

            return essayTopicDtos;
        }

        public ICollection<EssayTopicDto> GetEssayTopics(int year)
        {
            var essayTopicDtos = new List<EssayTopicDto>();
            using (var context = GetAdminsRepositoryDbContext())
            {
                var essayTopicsByYear = context.GraduatingClasses.First(graduatingClass => graduatingClass.GraduatingYear == year).EssayTopics;
                    
                essayTopicDtos.AddRange(essayTopicsByYear.Select(MapEssayTopicToDto));
            }
            return essayTopicDtos;
        }

        private void EditEssayTopic(CreateAndModifyEssayTopicDto dto)
        {
            using (var context = GetAdminsRepositoryDbContext())
            {
                var essayTopic = context.EssayTopics.First(essayTopics => essayTopics.Id == dto.Id);
                essayTopic.EssayPrompt = dto.EssayPrompt;
                essayTopic.TitleOfEssay = dto.TitleOfEssay;
                essayTopic.RevisionDateTime = DateTime.UtcNow;
                essayTopic.LastRevisionAuthor = context.People.First(people => people.Guid == AdminsGuid).Admin;
                context.SaveChanges();
            }
        }

        private void AddNewEssayTopic(CreateAndModifyEssayTopicDto dto)
        {
            var essayTopic = Mapper.Map<EssayTopic>(dto);
            essayTopic.RevisionDateTime = DateTime.UtcNow;

            using (var context = GetAdminsRepositoryDbContext())
            {
                essayTopic.LastRevisionAuthor = context.Admins.First(admin => admin.Person.Guid == AdminsGuid);
                context.EssayTopics.Add(essayTopic);
                context.SaveChanges();
            }
        }


        private EssayTopicDto MapEssayTopicToDto(EssayTopic topic)
        {
            var dto = new EssayTopicDto
            {
                EssayPrompt = topic.EssayPrompt,
                Id = topic.Id,
                RevisionDateTime = topic.RevisionDateTime,
                TitleOfEssay = topic.TitleOfEssay,
                LastRevisionAuthor =
                    new NameDto
                    {
                        FirstName = topic.LastRevisionAuthor.Person.Name.FirstName,
                        LastName = topic.LastRevisionAuthor.Person.Name.LastName
                    },
                ForWhatGraduatingYears = new List<int>()
            };

            foreach (var graduatingClass in topic.ForWhatGraduatingYears)
            {
                dto.ForWhatGraduatingYears.Add(graduatingClass.GraduatingYear);
            }
            return dto;
        }
    }
}
