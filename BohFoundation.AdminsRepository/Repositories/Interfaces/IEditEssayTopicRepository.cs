using System.Collections.Generic;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;

namespace BohFoundation.AdminsRepository.Repositories.Interfaces
{
    public interface IEditEssayTopicRepository
    {
        void UpsertEssayTopic(CreateAndModifyEssayTopicDto dto);
        void AddEssayTopicToGraduatingYear(EditEssayTopicByGraduatingClassDto dto);
        void DeleteEssayTopicFromGraduatingYear(EditEssayTopicByGraduatingClassDto dto);
        ICollection<EssayTopicDto> GetEssayTopics();
        ICollection<EssayTopicDto> GetEssayTopics(int year);
    }
}
