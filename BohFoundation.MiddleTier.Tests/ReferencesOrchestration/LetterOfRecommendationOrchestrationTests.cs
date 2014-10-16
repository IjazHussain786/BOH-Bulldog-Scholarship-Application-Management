using BohFoundation.AzureStorage.TableStorage.Interfaces.LettersOfRecommendation;
using BohFoundation.Domain.Dtos.Reference.Anonymous;
using BohFoundation.MiddleTier.ReferencesOrchestration.Implementations;
using BohFoundation.MiddleTier.ReferencesOrchestration.Interfaces.Helpers.BohFoundation.ReferencesRepository.OrchestrationLayer.Interfaces.Helper;
using BohFoundation.ReferencesRepository.Repositories.Interfaces;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MiddleTier.Tests.ReferencesOrchestration
{
    [TestClass]
    public class LetterOfRecommendationOrchestrationTests
    {
        private AnonymousLetterOfRecommendationOrchestration _letterOfRecommendationOrchestration;
        private IGenerateLetterOfRecommendationKey _generateLetterOfRecommendationKey;
        private IAzureLettersOfRecommendationRepository _azureRepository;
        private ILetterOfRecommendationRowValueSqlRepository _sqlRepository;

        [TestInitialize]
        public void Initialize()
        {
            _sqlRepository = A.Fake<ILetterOfRecommendationRowValueSqlRepository>();
            _azureRepository = A.Fake<IAzureLettersOfRecommendationRepository>();
            _generateLetterOfRecommendationKey = A.Fake<IGenerateLetterOfRecommendationKey>();

            _letterOfRecommendationOrchestration = new AnonymousLetterOfRecommendationOrchestration(_sqlRepository, _azureRepository, _generateLetterOfRecommendationKey);

            ItemThatOrginallyGoesIntoMethod = new LetterOfRecommendationDto();
            ItemToPassToRepos = new LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto();

            A.CallTo(
                () =>
                    _generateLetterOfRecommendationKey.GenerateKeyValueForLettersOfRecommendation(
                        A<LetterOfRecommendationDto>.Ignored)).Returns(ItemToPassToRepos);
        }

        private LetterOfRecommendationDto ItemThatOrginallyGoesIntoMethod { get; set; }
        private LetterOfRecommendationKeyValueForEntityFrameworkAndAzureDto ItemToPassToRepos { get; set; }

        [TestMethod]
        public void LetterOfRecommendationOrchestration_AddLetterOfRecommendation_Should_Call_Generate_Once()
        {
            CallLetterOfRecommendationOrchestration();
            A.CallTo(
                () => _generateLetterOfRecommendationKey.GenerateKeyValueForLettersOfRecommendation(ItemThatOrginallyGoesIntoMethod))
                .MustHaveHappened();
        }

        [TestMethod]
        public void LetterOfRecommendationOrchestration_AddLetterOfRecommendation_Should_Call_AzureRepo()
        {
            CallLetterOfRecommendationOrchestration();
            A.CallTo(() => _azureRepository.UpsertLetterOfRecommendation(ItemToPassToRepos)).MustHaveHappened();
        }

        [TestMethod]
        public void LetterOfRecommendationOrchestration_AddLetterOfRecommendation_Should_Call_SqlRepo()
        {
            CallLetterOfRecommendationOrchestration();
            A.CallTo(() => _sqlRepository.UpsertLetterOfRecommendationKeyValues(ItemToPassToRepos)).MustHaveHappened();
        }

        private void CallLetterOfRecommendationOrchestration()
        {
            _letterOfRecommendationOrchestration.AddLetterOfRecommendation(ItemThatOrginallyGoesIntoMethod);
        }
    }
}