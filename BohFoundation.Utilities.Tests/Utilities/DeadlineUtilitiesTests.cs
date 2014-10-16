using System;
using BohFoundation.Utilities.Context.Implementation;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Utilities.Interfaces;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Utilities.Tests.Utilities
{
    [TestClass]
    public class DeadlineUtilitiesTests
    {
        private IClaimsInformationGetters _claimsInformationGetters;
        private DeadlineUtilities _deadlineVerifier;
        private IGetTime _getTime;

        [TestInitialize]
        public void Initialize()
        {
            _claimsInformationGetters = A.Fake<IClaimsInformationGetters>();
            _getTime = A.Fake<IGetTime>();
            _deadlineVerifier = new DeadlineUtilities(_claimsInformationGetters, _getTime);

            CreateDeadline();
        }

        private void CreateDeadline()
        {
            const string timeZoneString = "Central Standard Time";
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
            
            const int month = 5;
            const int day = 31;
            const int hour = 0;
            var year = DateTime.UtcNow.Year;

            var deadline = new DateTime(year, month, day, hour, 0, 0);


            DeadLine = TimeZoneInfo.ConvertTimeToUtc(deadline, timezone);
        }

        private DateTime DeadLine { get; set; }

        [TestMethod]
        public void DeadlineVerifier_IsAfterDeadline_OneYearFromNow_Returns_False()
        {
            A.CallTo(() => _getTime.GetUtcNow()).Returns(DateTime.UtcNow);
            A.CallTo(() => _claimsInformationGetters.GetApplicantsGraduatingYear()).Returns(DateTime.Now.Year + 1);
            Assert.IsFalse(IsAfterDeadline());
        }

        [TestMethod]
        public void DeadlineVerifier_IsAfterDeadline_OneYearBeforeNow_Returns_True()
        {
            A.CallTo(() => _getTime.GetUtcNow()).Returns(DateTime.UtcNow);
            A.CallTo(() => _claimsInformationGetters.GetApplicantsGraduatingYear()).Returns(DateTime.Now.Year - 1);
            Assert.IsTrue(IsAfterDeadline());
        }

        [TestMethod]
        public void DeadlineVerifier_IsAfterDeadline_OneMillisecondSecondBeforeDeadline_Returns_False()
        {
            var now = DeadLine.AddMilliseconds(-1);
            A.CallTo(() => _claimsInformationGetters.GetApplicantsGraduatingYear()).Returns(DateTime.Now.Year);
            A.CallTo(() => _getTime.GetUtcNow()).Returns(now);
            Assert.IsFalse(IsAfterDeadline());
        }

        [TestMethod]
        public void DeadlineVerifier_IsAfterDeadline_OneMillisecondSecondAfterDeadline_Returns_True()
        {
            var now = DeadLine.AddMilliseconds(1);
            A.CallTo(() => _claimsInformationGetters.GetApplicantsGraduatingYear()).Returns(DateTime.Now.Year);
            A.CallTo(() => _getTime.GetUtcNow()).Returns(now);
            Assert.IsTrue(IsAfterDeadline());
        }

        [TestMethod]
        public void DeadlineUtilities_GetApplicantsDeadline_Should_Return_Deadline()
        {
            A.CallTo(() => _claimsInformationGetters.GetApplicantsGraduatingYear()).Returns(DateTime.Now.Year);
            Assert.AreEqual(DeadLine, _deadlineVerifier.GetApplicantsDeadlineInUtc());
        }

        [TestMethod]
        public void DeadlineUtilities_GetApplicantsDeadline_Should_Return_Deadline_Minus_One_Year()
        {
            var newDeadline = DeadLine.AddYears(-1);
            A.CallTo(() => _claimsInformationGetters.GetApplicantsGraduatingYear()).Returns(DateTime.Now.Year - 1);
            Assert.AreEqual(newDeadline, _deadlineVerifier.GetApplicantsDeadlineInUtc());
        }

        public bool IsAfterDeadline()
        {
            return _deadlineVerifier.IsAfterDeadline();
        }
    }
}
