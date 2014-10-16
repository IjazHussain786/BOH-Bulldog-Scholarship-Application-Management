using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BohFoundation.PersonsRepository.Repositories.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using FakeItEasy;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.CommonStaticItemsForTests
{
    public static class MembershipProviderCommonFakes
    {
        public static UserAccountService<RelationalUserAccount> UserAccountService { get; set; }
        public static RelationalUserAccount RelationalUserAccount { get; set; }
        public static IClaimsInformationGetters ClaimsInformationGetters { get; set; }
        public static ICreateUserClaimCollection CreateUserClaimCollection { get; set; }
        public static ICreatePersonRepository PersonsRepository { get; set; }
        public static ICreateClaimsListFromAccount CreateClaimsListFromAccount { get; set; }

        public static void CreateNewInstances()
        {
            UserAccountService = A.Fake<UserAccountService<RelationalUserAccount>>();
            RelationalUserAccount = A.Fake<RelationalUserAccount>();
            CreateUserClaimCollection = A.Fake<ICreateUserClaimCollection>();
            ClaimsInformationGetters = A.Fake<IClaimsInformationGetters>();
            PersonsRepository = A.Fake<ICreatePersonRepository>();
            CreateClaimsListFromAccount = A.Fake<ICreateClaimsListFromAccount>();
        }
    }
}
