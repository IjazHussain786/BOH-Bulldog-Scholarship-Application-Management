using System;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Entities;
using BohFoundation.AzureStorage.TableStorage.Implementations.Essay.Helpers;
using BohFoundation.Domain.Dtos.Applicant.Essay;
using BohFoundation.Domain.Dtos.Common.AzureQueuryObjects;
using BohFoundation.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.AzureStorage.Tests.TableStorage.Essays
{
    [TestClass]
    public class AzureEssayRepositoryTests
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            Setup();
            FirstGet();
            FirstUpsert();
            SecondGet();
            SecondUpsert();
            ThirdGet();
        }

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            UserGuid = TestHelpersCommonFields.GuidOne;
            
            var random = new Random();
            IdOfTopicEssay = random.Next();
            GraduatingYear = random.Next();

            var createRowKey = new EssayRowKeyGenerator();
            RowKey = createRowKey.CreateRowKeyForEssay(UserGuid, IdOfTopicEssay);

            Essay =
                "ajksdfnjkjansdf 0 asdfhuja we89uasdfkjnk kasdjkfnak sjdkfnasf09u2309j2jnk kasdfkjnalkw34r0923 aksdnfklanskef203 asdfjn 23npoiasdfnasdfnkjn23908 sdjakfnklj sadlfkjn 2389ohasdnf lkjnasdf283a nkjnsadf 02fjdasklfn21309 nasdfnaskjdf0";

            Prompt = "Are there hats?";

            AzureEssayRepo = new AzureEssayRepository(TestHelpersCommonFields.AzureConnectionName);

            GetEssayEntity = new AzureTableStorageEntityKeyDto {PartitionKey = GraduatingYear.ToString(), RowKey = RowKey};

        }

        private static AzureTableStorageEntityKeyDto GetEssayEntity { get; set; }
        private static AzureEssayRepository AzureEssayRepo { get; set; }
        private static string Prompt { get; set; }
        private static string Essay { get; set; }
        private static string RowKey { get; set; }
        private static int GraduatingYear { get; set; }
        private static Guid UserGuid { get; set; }
        private static int IdOfTopicEssay { get; set; }

        private static void FirstGet()
        {
            FirstGetResult = AzureEssayRepo.GetEssay(GetEssayEntity);
        }

        #region FirstGet

        private static EssayDto FirstGetResult { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_FirstGet_Should_Return_Null()
        {
            Assert.IsNull(FirstGetResult);
        }

        #endregion

        private static void FirstUpsert()
        {
            var entity = CreateEssayEntity(0);
            AzureEssayRepo.UpsertEssay(entity);
        }

        private static void SecondGet()
        {
            SecondGetResult = AzureEssayRepo.GetEssay(GetEssayEntity);
        }

        private static EssayDto SecondGetResult { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_SecondGet_Should_Return_NotNull()
        {
            Assert.IsNotNull(SecondGetResult);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_SecondGet_Should_Return_Right_Essay()
        {
            Assert.AreEqual(Essay + 0, SecondGetResult.Essay);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_SecondGet_Should_Return_Right_Prompt()
        {
            Assert.AreEqual(Prompt + 0, SecondGetResult.EssayPrompt);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_SecondGet_Should_Return_Right_Time()
        {
            TestHelpersTimeAsserts.RecentTime(SecondGetResult.RevisionDateTime);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_SecondGet_Should_Return_Right_TopicId()
        {
            Assert.AreEqual(IdOfTopicEssay, SecondGetResult.EssayTopicId);
        }

        private static void SecondUpsert()
        {
            var entity = CreateEssayEntity(1);
            AzureEssayRepo.UpsertEssay(entity);
        }

        private static void ThirdGet()
        {
            ThirdGetResult = AzureEssayRepo.GetEssay(GetEssayEntity);
        }

        private static EssayDto ThirdGetResult { get; set; }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_ThirdGet_Should_Return_Right_Essay()
        {
            Assert.AreEqual(Essay + 1, ThirdGetResult.Essay);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_ThirdGet_Should_Return_Right_Prompt()
        {
            Assert.AreEqual(Prompt + 1, ThirdGetResult.EssayPrompt);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_ThirdGet_Should_Return_Right_Time()
        {
            TestHelpersTimeAsserts.IsGreaterThanOrEqual(ThirdGetResult.RevisionDateTime, SecondGetResult.RevisionDateTime);
        }

        [TestMethod, TestCategory("Integration")]
        public void AzureEssayRepository_ThirdGet_Should_Return_Right_TopicId()
        {
            Assert.AreEqual(IdOfTopicEssay, ThirdGetResult.EssayTopicId);
        }

        private static EssayAzureTableEntityDto CreateEssayEntity(int i)
        {
            return new EssayAzureTableEntityDto { PartitionKey = GraduatingYear.ToString(), RowKey = RowKey,Essay = Essay + i, EssayPrompt = Prompt + i, RevisionDateTime = DateTime.UtcNow, EssayTopicId = IdOfTopicEssay};
        }
    }
}