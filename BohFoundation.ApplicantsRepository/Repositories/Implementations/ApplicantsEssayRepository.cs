using System;
using System.Linq;
using BohFoundation.ApplicantsRepository.DbContext;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay;
using BohFoundation.AzureStorage.TableStorage.Interfaces.Essay.Helpers;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class ApplicantsEssayRepository: ApplicantsRepositoryBase, IApplicantsEssayRepository
    {
        private readonly IAzureEssayRepository _azureEssayRepository;
        private readonly IEssayRowKeyGenerator _rowKeyGenerator;
        private DateTime _now;

        public ApplicantsEssayRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters, IAzureEssayRepository azureEssayRepository, IEssayRowKeyGenerator rowKeyGenerator) : base(dbConnection, claimsInformationGetters)
        {
            _azureEssayRepository = azureEssayRepository;
            _rowKeyGenerator = rowKeyGenerator;
        }

        public EssayDto GetEssay(int essayTopicId)
        {
            var azureTableStorageReference = new AzureTableStorageEntityKeyDto
            {
                PartitionKey = GetApplicantsGraduatingClassYear().ToString(),
                RowKey = _rowKeyGenerator.CreateRowKeyForEssay(ApplicantGuid, essayTopicId)
            };

            return _azureEssayRepository.GetEssay(azureTableStorageReference);
        }

        public void UpsertEssay(EssayDto essayDto)
        {
            _now = DateTime.UtcNow;
            
            var azureEssayDto = CreateEssayAzureTableEntityDto(essayDto);
            
            var context = GetApplicantsDbContext();

            var essay = GetEssayFromSqlDb(essayDto, context);

            if (essay == null)
            {
                CreateNewEssayInDb(essayDto, context);
            }
            else
            {
                essay.RevisionDateTime = _now;
                essay.CharacterLength = essayDto.Essay.Length;
            }

            _azureEssayRepository.UpsertEssay(azureEssayDto);
            context.SaveChanges();
            
            context.Dispose();
        }
        
        private Essay GetEssayFromSqlDb(EssayDto essayDto, ApplicantRepositoryDbContext context)
        {
            return
                context.Essays.FirstOrDefault(
                        essays =>
                            essays.Applicant.Person.Guid == ApplicantGuid &&
                            essays.EssayTopic.Id == essayDto.EssayTopicId);
        }

        private void CreateNewEssayInDb(EssayDto essayDto, ApplicantRepositoryDbContext context)
        {
            var topic = context.EssayTopics.First(essayTopic => essayTopic.Id == essayDto.EssayTopicId);
            var applicant = context.Applicants.First(app => app.Person.Guid == ApplicantGuid);

            var essay = new Essay
            {
                CharacterLength = essayDto.Essay.Length,
                RowKey = _rowKeyGenerator.CreateRowKeyForEssay(ApplicantGuid, essayDto.EssayTopicId),
                PartitionKey = GetApplicantsGraduatingClassYear().ToString(),
                RevisionDateTime = _now,
                EssayTopic = topic,
                Applicant = applicant
            };

            context.Essays.Add(essay);
        }

        private EssayAzureTableEntityDto CreateEssayAzureTableEntityDto(EssayDto essayDto)
        {
            var azureEssayDto = new EssayAzureTableEntityDto
            {
                Essay = essayDto.Essay,
                EssayPrompt = essayDto.EssayPrompt,
                RowKey = _rowKeyGenerator.CreateRowKeyForEssay(ApplicantGuid, essayDto.EssayTopicId),
                PartitionKey = GetApplicantsGraduatingClassYear().ToString(),
                EssayTopicId = essayDto.EssayTopicId,
                RevisionDateTime = _now
            };
            return azureEssayDto;
        }
    }
}
