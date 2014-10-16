using System;
using System.Text;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations.Helpers;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Context.Interfaces.Context;
using BohFoundation.Utilities.Utilities.Interfaces;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MiddleTier.Tests.ApplicantsOrchestration
{
    [TestClass]
    public class CreateEmailBodyForApplicantReferenceRequestTests
    {
        private IRandomObjectGenerator _objectGenerator;
        private CreateEmailBodyForApplicantReferenceRequest _createEmailBody;
        private IHttpContextInformationGetters _httpContextInformation;
        

        [TestInitialize]
        public void Initialize()
        {
            _objectGenerator = A.Fake<IRandomObjectGenerator>();
            _httpContextInformation = A.Fake<IHttpContextInformationGetters>();
            _createEmailBody = new CreateEmailBodyForApplicantReferenceRequest(_objectGenerator, _httpContextInformation);
            Guid = Guid.NewGuid();

            TestHelpersCommonFields.InitializeFields();

            Salutation = "Whats up?";
            Signature = "Thats it";

            ApplicantReferenceInputDto = new ApplicantReferenceInputDto
            {
                MessageParagraph = TestHelpersCommonFields.Paragraph, 
                ReferenceEmail = TestHelpersCommonFields.Email, 
                RelationshipToReference = TestHelpersCommonFields.ExceptionMessage,
                Salutation = Salutation,
                Signature = Signature
            };

            Result = CreateBody();
        }

        private string Salutation { get; set; }
        private string Signature { get; set; }
        private ApplicantReferenceForEntityFrameworkDto Result { get; set; }
        private Guid Guid { get; set; }
        private ApplicantReferenceInputDto ApplicantReferenceInputDto { get; set; }

        [TestMethod]
        public void CreateEmailBodyForApplicantReferenceRequest_CreateBody_Should_Call_ObjectGenerator()
        {
            A.CallTo(() => _objectGenerator.GenerateNewGuid()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void CreateEmailBodyForApplicantReferenceRequest_CreateBody_Should_Return_GuidLink()
        {
            Assert.AreEqual(Guid, Result.GuidLink);
        }

        [TestMethod]
        public void CreateEmailBodyForApplicantReferenceRequest_CreateBody_Should_Return_Email()
        {
            Assert.AreEqual(TestHelpersCommonFields.Email.ToLowerInvariant(), Result.ReferenceEmail);
        }

        [TestMethod]
        public void CreateEmailBodyForApplicantReferenceRequest_CreateBody_Should_Return_Relationship()
        {
            Assert.AreEqual(TestHelpersCommonFields.ExceptionMessage, Result.RelationshipToReference);
        }

        [TestMethod]
        public void CreateEmailBodyForApplicantReferenceRequest_CreateBody_Should_Call_httpContextInformation_Once()
        {
            A.CallTo(()=> _httpContextInformation.GetRequestHttpBaseUrl()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void CreateEmailBodyForApplicantReferenceRequest_CreateBody_Should_Return_CorrectBody()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Salutation);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append(TestHelpersCommonFields.Paragraph);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append(Signature);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append("Please click the following link to fill out the recommendation: " + BaseUrl + "/Reference/LetterOfRecommendation/Anon/" + Guid);
            Assert.AreEqual(stringBuilder.ToString(), Result.MessageParagraph);
        }

        private const string BaseUrl = "domain";

        private ApplicantReferenceForEntityFrameworkDto CreateBody()
        {
            A.CallTo(() => _objectGenerator.GenerateNewGuid()).Returns(Guid);
            A.CallTo(() => _httpContextInformation.GetRequestHttpBaseUrl()).Returns(BaseUrl);

            return _createEmailBody.CreateBody(ApplicantReferenceInputDto);
        }
    }
}
