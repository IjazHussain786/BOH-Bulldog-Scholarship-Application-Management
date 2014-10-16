using System;
using System.Collections.Generic;
using System.Linq;
using BohFoundation.AdminsRepository.DbContext;
using BohFoundation.AdminsRepository.Repositories.Implementation;
using BohFoundation.Domain.Dtos.Admin.EssayTopics;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.Enums;
using BohFoundation.EntityFrameworkBaseClass;
using BohFoundation.PersonsRepository.Repositories.Implementations;
using BohFoundation.TestHelpers;
using EntityFramework.Extensions;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.AdminsRepository.Tests.IntegrationTests
{
    [TestClass]
    public class EditEssayTopicRepositoryIntegrationTests
    {
        private static EditEssayTopicRepository _editEssayRepository;

        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            Setup();
            CleanDb();
            AddTwoEssayTopics();
            UpdateFirstEssayTopic();
            AddEssayTopicsToGraduatingYears();
            GetAllEssayTopics();
            GetByYear();
            DeleteEssayTopicFromGraduatingYear();
        }

        #region Setup

        private static void Setup()
        {
            TestHelpersCommonFields.InitializeFields();
            TestHelpersCommonFakes.InitializeFakes();

            CreateEditEssayRepository();
        }

        private static void CreateEditEssayRepository()
        {
            _editEssayRepository = new EditEssayTopicRepository(TestHelpersCommonFields.DatabaseName,
                TestHelpersCommonFakes.ClaimsInformationGetters);
        }

        private static void MakeAdmin(Guid adminGuid)
        {
            var name = new Name
            {
                FirstName = TestHelpersCommonFields.FirstName + adminGuid,
                LastName = TestHelpersCommonFields.LastName + adminGuid
            };

            var personsRepo = new CreatePersonRepository(TestHelpersCommonFields.DatabaseName);

            A.CallTo(() => TestHelpersCommonFakes.ClaimsInformationGetters.GetUsersGuid()).Returns(adminGuid);
            CreateEditEssayRepository();

            personsRepo.CreatePerson(adminGuid, name, MemberTypesEnum.Admin);
        }

        private static Guid Admin1Guid { get; set; }

        #endregion

        #region AddTwoEssayTopics

        private static void AddTwoEssayTopics()
        {
            Admin1Guid = Guid.NewGuid();
            MakeAdmin(Admin1Guid);

            NumberOfEssayTopics = 2;
            var arrayOfTopics = new List<CreateAndModifyEssayTopicDto>();

            for (int i = 0; i < NumberOfEssayTopics; i++)
            {
                arrayOfTopics.Add(new CreateAndModifyEssayTopicDto
                {
                    EssayPrompt = TestHelpersCommonFields.EssayPrompt + i,
                    TitleOfEssay = TestHelpersCommonFields.EssayTitle + i
                });
            }

            foreach (var dto in arrayOfTopics)
            {
                _editEssayRepository.UpsertEssayTopic(dto);
            }

            using (var context = GetAdminsRepositoryDbContext())
            {
                AddTwoEssayTopicsResult = context.EssayTopics.Where(essayTopic => essayTopic.Id > 0 && (essayTopic.LastRevisionAuthor.Person.Guid == Admin1Guid)).ToList();

                foreach (var essayTopic in AddTwoEssayTopicsResult)
                {
                    var populateAdmin = essayTopic.LastRevisionAuthor;
                    var populatePerson = populateAdmin.Person;

                    var populateGraduatingYears = essayTopic.ForWhatGraduatingYears;
                }
            }
        }

        private static int NumberOfEssayTopics { get; set; }
        private static ICollection<EssayTopic> AddTwoEssayTopicsResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_Two_Objects()
        {
            Assert.AreEqual(NumberOfEssayTopics, AddTwoEssayTopicsResult.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_Positive_Ids()
        {
            foreach (var essayTopic in AddTwoEssayTopicsResult)
            {
                TestHelpersCommonAsserts.IsGreaterThanZero(essayTopic.Id);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_Right_Title()
        {
            var asArray = AddTwoEssayTopicsResult.ToArray();

            for (int i = 0; i < NumberOfEssayTopics; i++)
            {
                Assert.AreEqual(TestHelpersCommonFields.EssayTitle + 0, asArray[0].TitleOfEssay);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_Right_Prompts()
        {
            var asArray = AddTwoEssayTopicsResult.ToArray();

            for (int i = 0; i < NumberOfEssayTopics; i++)
            {
                Assert.AreEqual(TestHelpersCommonFields.EssayPrompt + 0, asArray[0].EssayPrompt);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_Right_Admin_Association()
        {
            foreach (var essayTopic in AddTwoEssayTopicsResult)
            {
                Assert.AreEqual(Admin1Guid, essayTopic.LastRevisionAuthor.Person.Guid);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_Right_Time()
        {
            foreach (var essayTopic in AddTwoEssayTopicsResult)
            {
                TestHelpersTimeAsserts.RecentTime(essayTopic.RevisionDateTime);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Should_Have_No_Graduating_Years()
        {
            foreach (var essayTopic in AddTwoEssayTopicsResult)
            {
                Assert.AreEqual(0, essayTopic.ForWhatGraduatingYears.Count);
            }
        }

        #endregion

        #region UpdateTopic

        private static void UpdateFirstEssayTopic()
        {
            Admin2Guid = Guid.NewGuid();

            MakeAdmin(Admin2Guid);

            FirstEssayTopicId = AddTwoEssayTopicsResult.First().Id;

            var dto = new CreateAndModifyEssayTopicDto
            {
                EssayPrompt = TestHelpersCommonFields.EssayPrompt + ModifiedPostFix,
                TitleOfEssay = TestHelpersCommonFields.EssayTitle + ModifiedPostFix,
                Id = FirstEssayTopicId
            };

            _editEssayRepository.UpsertEssayTopic(dto);

            using (var context = GetAdminsRepositoryDbContext())
            {
                UpdateFirstEssayTopicResult =
                    context.EssayTopics.FirstOrDefault(essayTopic => essayTopic.Id == FirstEssayTopicId);

                var populateAdmin = UpdateFirstEssayTopicResult.LastRevisionAuthor;
                var populatePerson = populateAdmin.Person;
                var populateGraduatingYears = UpdateFirstEssayTopicResult.ForWhatGraduatingYears;
            }
        }

        private static Guid Admin2Guid { get; set; }
        private static EssayTopic UpdateFirstEssayTopicResult { get; set; }
        private static int FirstEssayTopicId { get; set; }
        private const string ModifiedPostFix = "MODIFIED";

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Update_Should_Have_Same_Id_As_First_Upsert()
        {
            Assert.AreEqual(FirstEssayTopicId, UpdateFirstEssayTopicResult.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Update_Should_Have_Right_Title()
        {
            Assert.AreEqual(TestHelpersCommonFields.EssayTitle + ModifiedPostFix,
                UpdateFirstEssayTopicResult.TitleOfEssay);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Update_Should_Have_Right_Prompts()
        {
            Assert.AreEqual(TestHelpersCommonFields.EssayPrompt + ModifiedPostFix,
                UpdateFirstEssayTopicResult.EssayPrompt);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Update_Should_Have_Right_Admin_Association()
        {
            Assert.AreEqual(Admin2Guid, UpdateFirstEssayTopicResult.LastRevisionAuthor.Person.Guid);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Update_Should_Have_Right_Time()
        {
            TestHelpersTimeAsserts.RecentTime(UpdateFirstEssayTopicResult.RevisionDateTime);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_UpsertEssayTopic_Update_Should_Have_No_Graduating_Years()
        {
            Assert.AreEqual(0, UpdateFirstEssayTopicResult.ForWhatGraduatingYears.Count);
        }

        #endregion

        #region AddEssayTopicsToGraduatingYears

        private static void AddEssayTopicsToGraduatingYears()
        {
            Gy2013 = 2013;
            Gy2014 = 2014;
            Gy506 = 506;
            SecondEssayTopicId = AddTwoEssayTopicsResult.Last().Id;

            GraduatingYears = new[] {Gy2013, Gy2014, Gy506};

            foreach (var graduatingYear in GraduatingYears)
            {
                var dto = new EditEssayTopicByGraduatingClassDto {ClassYear = graduatingYear, EssayId = FirstEssayTopicId};
                _editEssayRepository.AddEssayTopicToGraduatingYear(dto);
            }

            var secondDto = new EditEssayTopicByGraduatingClassDto {ClassYear = Gy506, EssayId = SecondEssayTopicId};
            _editEssayRepository.AddEssayTopicToGraduatingYear(secondDto);

            using (var context = GetAdminsRepositoryDbContext())
            {
                FirstEssayTopicAfterAddYears =
                    context.EssayTopics.First(essayTopic => essayTopic.Id == FirstEssayTopicId);

                var loadFirstYears = FirstEssayTopicAfterAddYears.ForWhatGraduatingYears;

                SecondEssayTopicAfterAddYear =
                    context.EssayTopics.First(essayTopic => essayTopic.Id == SecondEssayTopicId);

                var loadSecondYears = SecondEssayTopicAfterAddYear.ForWhatGraduatingYears;
            }
        }

        public static int[] GraduatingYears { get; set; }

        private static EssayTopic SecondEssayTopicAfterAddYear { get; set; }
        private static EssayTopic FirstEssayTopicAfterAddYears { get; set; }

        private static int SecondEssayTopicId { get; set; }
        private static int Gy506 { get; set; }
        private static int Gy2014 { get; set; }
        private static int Gy2013 { get; set; }


        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_AddEssayTopicToGraduatingYear_FirstTopic_Should_Have_Three_Graduating_Years
            ()
        {
            Assert.AreEqual(3, FirstEssayTopicAfterAddYears.ForWhatGraduatingYears.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_AddEssayTopicToGraduatingYear_SecondTopic_Should_Have_One_Graduating_Years()
        {
            Assert.AreEqual(1, SecondEssayTopicAfterAddYear.ForWhatGraduatingYears.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            EditEssayTopicRepository_AddEssayTopicToGraduatingYear_FirstTopic_Should_Have_The_Right_Graduating_Years()
        {
            foreach (var graduatingYear in GraduatingYears)
            {
                Assert.IsNotNull(FirstEssayTopicAfterAddYears.ForWhatGraduatingYears.First(
                    gradClass => gradClass.GraduatingYear == graduatingYear));
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void
            EditEssayTopicRepository_AddEssayTopicToGraduatingYear_SecondTopic_Should_Have_The_Right_Graduating_Years()
        {
            var gradYear = SecondEssayTopicAfterAddYear.ForWhatGraduatingYears.First();
            Assert.AreEqual(Gy506, gradYear.GraduatingYear);
        }

        #endregion

        #region GetAllEssayTopics

        private static void GetAllEssayTopics()
        {
            GetAllEssayTopicsResult = _editEssayRepository.GetEssayTopics();
            GetAllFirstEssayTopicResult = GetAllEssayTopicsResult.FirstOrDefault(topic => topic.Id == FirstEssayTopicId);
            GetAllSecondEssayTopicResult =
                GetAllEssayTopicsResult.FirstOrDefault(topic => topic.Id == SecondEssayTopicId);
        }

        private static EssayTopicDto GetAllSecondEssayTopicResult { get; set; }
        private static EssayTopicDto GetAllFirstEssayTopicResult { get; set; }
        private static ICollection<EssayTopicDto> GetAllEssayTopicsResult { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_Should_Have_Two_Objects()
        {
            Assert.AreEqual(2, GetAllEssayTopicsResult.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_SameId()
        {
            Assert.AreEqual(FirstEssayTopicId, GetAllFirstEssayTopicResult.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_TitleOfEssay()
        {
            Assert.AreEqual(TestHelpersCommonFields.EssayTitle + ModifiedPostFix,
                GetAllFirstEssayTopicResult.TitleOfEssay);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_EssayPrompt()
        {
            Assert.AreEqual(TestHelpersCommonFields.EssayPrompt + ModifiedPostFix,
                GetAllFirstEssayTopicResult.EssayPrompt);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_RevisionDateTime()
        {
            Assert.AreEqual(UpdateFirstEssayTopicResult.RevisionDateTime, GetAllFirstEssayTopicResult.RevisionDateTime);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_ForWhatGraduatingYears()
        {
            foreach (var graduatingYear in GraduatingYears)
            {
                Assert.IsTrue(GetAllFirstEssayTopicResult.ForWhatGraduatingYears.Contains(graduatingYear));
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_LastRevisionAuthor_FirstName()
        {
            Assert.AreEqual(TestHelpersCommonFields.FirstName + Admin2Guid,
                GetAllFirstEssayTopicResult.LastRevisionAuthor.FirstName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayOne_Should_Have_LastRevisionAuthor_LastName()
        {
            Assert.AreEqual(TestHelpersCommonFields.LastName + Admin2Guid,
                GetAllFirstEssayTopicResult.LastRevisionAuthor.LastName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_SameId()
        {
            Assert.AreEqual(SecondEssayTopicId, GetAllSecondEssayTopicResult.Id);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_TitleOfEssay()
        {
            Assert.AreEqual(TestHelpersCommonFields.EssayTitle + 1, GetAllSecondEssayTopicResult.TitleOfEssay);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_EssayPrompt()
        {
            Assert.AreEqual(TestHelpersCommonFields.EssayPrompt + 1, GetAllSecondEssayTopicResult.EssayPrompt);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_RevisionDateTime()
        {
            Assert.AreEqual(AddTwoEssayTopicsResult.First(topic => topic.Id == SecondEssayTopicId).RevisionDateTime,
                GetAllSecondEssayTopicResult.RevisionDateTime);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_ForWhatGraduatingYears()
        {
            Assert.IsTrue(GetAllSecondEssayTopicResult.ForWhatGraduatingYears.Contains(Gy506));

        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_LastRevisionAuthor_FirstName()
        {
            Assert.AreEqual(TestHelpersCommonFields.FirstName + Admin1Guid,
                GetAllSecondEssayTopicResult.LastRevisionAuthor.FirstName);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetAllEssayTopics_EssayTwo_Should_Have_LastRevisionAuthor_LastName()
        {
            Assert.AreEqual(TestHelpersCommonFields.LastName + Admin1Guid,
                GetAllSecondEssayTopicResult.LastRevisionAuthor.LastName);
        }

        #endregion

        #region GetByYear

        private static void GetByYear()
        {
            GetByYear506Result = _editEssayRepository.GetEssayTopics(Gy506);
            GetByYear2013Result = _editEssayRepository.GetEssayTopics(Gy2013);
        }

        private static ICollection<EssayTopicDto> GetByYear2013Result { get; set; }
        private static ICollection<EssayTopicDto> GetByYear506Result { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetEssayTopicsUsedByYear_506_Should_Have_2_Results()
        {
            Assert.AreEqual(2, GetByYear506Result.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_GetEssayTopicsUsedByYear_2013_Should_Have_1_Results()
        {
            Assert.AreEqual(1, GetByYear2013Result.Count);
        }

        #endregion
        
        private static void DeleteEssayTopicFromGraduatingYear()
        {
            _editEssayRepository.DeleteEssayTopicFromGraduatingYear(new EditEssayTopicByGraduatingClassDto{ClassYear = Gy506, EssayId = SecondEssayTopicId});
            _editEssayRepository.DeleteEssayTopicFromGraduatingYear(new EditEssayTopicByGraduatingClassDto{ClassYear = Gy506, EssayId = FirstEssayTopicId});

            using (var context = GetAdminsRepositoryDbContext())
            {
                FirstEssayAfterDeleteYear = context.EssayTopics.First(topic => topic.Id == FirstEssayTopicId);
                var loadFirstYears = FirstEssayAfterDeleteYear.ForWhatGraduatingYears;

                SecondEssayAfterDeleteYear = context.EssayTopics.First(topic => topic.Id == SecondEssayTopicId);
                var loadSecondYears = SecondEssayAfterDeleteYear.ForWhatGraduatingYears;
            }
        }

        private static EssayTopic FirstEssayAfterDeleteYear { get; set; }
        private static EssayTopic SecondEssayAfterDeleteYear { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_DeleteEssayTopicFromGraduatingYear_FirstEssayTopic_Should_Have_2_Years()
        {
            Assert.AreEqual(2, FirstEssayAfterDeleteYear.ForWhatGraduatingYears.Count);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_DeleteEssayTopicFromGraduatingYear_FirstEssayTopic_Should_Not_Have_506()
        {
            Assert.IsNull(FirstEssayAfterDeleteYear.ForWhatGraduatingYears.FirstOrDefault(graduatingClass => graduatingClass.GraduatingYear == Gy506));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void EditEssayTopicRepository_DeleteEssayTopicFromGraduatingYear_SecondEssayTopic_Should_Have_0_Years()
        {
            Assert.AreEqual(0, SecondEssayAfterDeleteYear.ForWhatGraduatingYears.Count);
        }

        #region ClassCleanupAndUtiltilies

        [ClassCleanup]
        public static void CleanDb()
        {
            using (var context = new DatabaseRootContext(TestHelpersCommonFields.DatabaseName))
            {
                context.EssayRatings.Where(essayRating => essayRating.Id > 0).Delete();
                context.Essays.Where(essay => essay.Id > 0).Delete();
                context.EssayTopics.Where(essayTopic => essayTopic.Id > 0).Delete();
                context.ApplicantPersonalInformations.Where(info => info.Id > 0).Delete();
                context.GraduatingClasses.Where(gradClass => gradClass.Id > 0).Delete();
            }
        }

        private static AdminsRepositoryDbContext GetAdminsRepositoryDbContext()
        {
            return new AdminsRepositoryDbContext(TestHelpersCommonFields.DatabaseName);
        }

        #endregion

    }
}
