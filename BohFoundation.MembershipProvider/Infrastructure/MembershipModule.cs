using BohFoundation.MembershipProvider.UserManagement.Admin.Implementation;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Implementation;
using BohFoundation.MembershipProvider.UserManagement.Helpers.Interfaces;
using BohFoundation.MembershipProvider.UserManagement.Manage.Implementation;
using BohFoundation.MembershipProvider.UserManagement.Manage.Interfaces;
using Ninject.Modules;

namespace BohFoundation.MembershipProvider.Infrastructure
{
    public class MembershipModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICreateUserClaimCollection>().To<CreateUserClaimCollection>();
            Bind<IRegisterUserService>().To<RegisterUserService>();
            Bind<IChangeEmailService>().To<ChangeEmailService>();
            Bind<ILoginServices>().To<LoginServices>();
            Bind<ICreateClaimsListFromAccount>().To<CreateClaimsListFromAccount>();
            Bind<IChangePasswordFromEmailedKeyService>().To<ChangePasswordFromEmailKeyService>();
            Bind<IChangePasswordService>().To<ChangePasswordService>();
            Bind<IInviteApplicationEvaluator>().To<InviteApplicationEvaluator>();
            Bind<IConfirmApplicationEvaluator>().To<ConfirmApplicationEvaluator>();
            Bind<IChangeApplicantToFinalizedApplicantService>().To<ChangeApplicantToFinalizedApplicantService>();
        }
    }
}
