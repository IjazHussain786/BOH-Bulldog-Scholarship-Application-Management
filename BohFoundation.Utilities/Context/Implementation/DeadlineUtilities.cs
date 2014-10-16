using System;
using System.Configuration;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Utilities.Interfaces;

namespace BohFoundation.Utilities.Context.Implementation
{
    public class DeadlineUtilities : IDeadlineUtilities
    {
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        private readonly IGetTime _getTime;

        public DeadlineUtilities(IClaimsInformationGetters claimsInformationGetters, IGetTime getTime)
        {
            _claimsInformationGetters = claimsInformationGetters;
            _getTime = getTime;
        }

        public bool IsAfterDeadline()
        {
            return _getTime.GetUtcNow() > GetApplicantsDeadlineInUtc();
        }

        public DateTime GetApplicantsDeadlineInUtc()
        {
            var config = ConfigurationManager.AppSettings;

            var month = int.Parse(config.GetValues("deadlineMonth")[0]);
            var day = int.Parse(config.GetValues("deadlineDay")[0]);
            var hour = int.Parse(config.GetValues("deadlineHour")[0]);
            var timezonestring = config.GetValues("deadlineTimeZone")[0];
            var year = _claimsInformationGetters.GetApplicantsGraduatingYear();

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezonestring);

            return TimeZoneInfo.ConvertTimeToUtc(new DateTime(year, month, day, hour, 0, 0), timeZone);
        }
    }
}
