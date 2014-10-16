using BohFoundation.Domain.Dtos.Admin.References;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Domain.Tests.Dtos.Admin.References
{
    [TestClass]
    public class GetLetterOfRecommendationGuidDtoTests
    {

        [TestInitialize]
        public void Initialize()
        {
            Dtos = new GetLetterOfRecommendationGuidDto
            {
                ApplicantsEmailAddress = " A2bc@Aol.CoM  ",
                ReferencesEmailAddress = " asd2@Am.Com  "
            };
        }

        private GetLetterOfRecommendationGuidDto Dtos { get; set; }

        [TestMethod]
        public void GetLetterOfRecommendationGuidDto_Makes_ApplicantsEmail_LowercaseAndTimed()
        {
            Assert.AreEqual("a2bc@aol.com", Dtos.ApplicantsEmailAddress);
        }

        [TestMethod]
        public void GetLetterOfRecommendationGuidDto_Makes_References_LowercaseAndTimed()
        {
            Assert.AreEqual("asd2@am.com", Dtos.ReferencesEmailAddress);
        }
    }
}
