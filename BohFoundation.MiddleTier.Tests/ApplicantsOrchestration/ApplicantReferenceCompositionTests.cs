using System;
using System.Collections.Generic;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces.Helpers;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Email.Interfaces.Email;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MiddleTier.Tests.ApplicantsOrchestration
{
    [TestClass]
    public class ApplicantReferenceCompositionTests
    {
        private IEmailService _emailService;
        private IApplicantsReferencesRepository _applicantsReferenceRepository;
        private ApplicantReferenceOrchestration _applicantsReferenceComposition;
        private ICreateEmailBodyForApplicantReferenceRequest _createEmailBody;

        [TestInitialize]
        public void Initialize()
        {
            TestHelpersCommonFields.InitializeFields();
            Guid = TestHelpersCommonFields.GuidOne;

            _emailService = A.Fake<IEmailService>();
            _applicantsReferenceRepository = A.Fake<IApplicantsReferencesRepository>();
            _createEmailBody = A.Fake<ICreateEmailBodyForApplicantReferenceRequest>();

            _applicantsReferenceComposition = new ApplicantReferenceOrchestration(_applicantsReferenceRepository,
                _emailService, _createEmailBody);

            ApplicantReferenceInputDto = new ApplicantReferenceInputDto();
            ApplicantReferenceForEntityFrameworkDto = new ApplicantReferenceForEntityFrameworkDto{ReferenceEmail = TestHelpersCommonFields.Email, GuidLink = Guid, MessageParagraph = TestHelpersCommonFields.ExceptionMessage};
        }

        private Guid Guid { get; set; }
        private ApplicantReferenceForEntityFrameworkDto ApplicantReferenceForEntityFrameworkDto { get; set; }
        private ApplicantReferenceInputDto ApplicantReferenceInputDto { get; set; }

        [TestMethod]
        public void ApplicantReferenceComposition_GetReferences_Should_Return_What_Repo_Returns()
        {
            A.CallTo(() => _applicantsReferenceRepository.GetReferences())
                .Returns(new List<ReferenceDto> {new ReferenceDto(), new ReferenceDto(), new ReferenceDto()});

            Assert.AreEqual(3, _applicantsReferenceComposition.GetReferences().Count);
        }

        [TestMethod]
        public void ApplicantReferenceComposition_AddReference_Should_Call_CreateEmailBody()
        {
            AddReference();
            A.CallTo(() => _createEmailBody.CreateBody(ApplicantReferenceInputDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantReferenceComposition_AddReference_Should_Call_Repo_WithObject_Returned_From_Create_Email_Body()
        {
            AddReference();
            A.CallTo(() => _applicantsReferenceRepository.AddReference(ApplicantReferenceForEntityFrameworkDto)).MustHaveHappened();
        }

        [TestMethod]
        public void ApplicantReferenceComposition_AddReference_Should_Call_SendEmailToOneUser_WithObject_Returned_From_Create_Email_Body()
        {
            AddReference();
            A.CallTo(() => _emailService.SendEmailToOneUser(A<SendEmailWithSubjectAndBodyDto>.That.Matches(sendEmailDto => 
                sendEmailDto.Body == TestHelpersCommonFields.ExceptionMessage && 
                sendEmailDto.Subject == "Reference Request For The Bulldog Scholarship" && 
                sendEmailDto.RecipientEmailAddress == TestHelpersCommonFields.Email.ToLowerInvariant()))).MustHaveHappened();
        }

        private void AddReference()
        {
            A.CallTo(() => _createEmailBody.CreateBody(ApplicantReferenceInputDto))
                .Returns(ApplicantReferenceForEntityFrameworkDto);

            _applicantsReferenceComposition.AddReference(ApplicantReferenceInputDto);
        }
    }
}
