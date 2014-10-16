using BohFoundation.Domain.Dtos.Applicant.References;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Domain.Tests.Dtos.Reference
{
    [TestClass]
    public class ApplicantReferenceInputDtoTests
    {
        private ApplicantReferenceInputDto _dto;

        [TestInitialize]
        public void Initialize()
        {
            _dto = new ApplicantReferenceInputDto
            {
                ReferenceEmail = "A@EMAIL.COM"
            };
        }

        [TestMethod]
        public void ApplicantReferenceInputDto_Should_Make_Email_LowerCase()
        {
            Assert.AreEqual("a@email.com", _dto.ReferenceEmail);
        }
    }
}
