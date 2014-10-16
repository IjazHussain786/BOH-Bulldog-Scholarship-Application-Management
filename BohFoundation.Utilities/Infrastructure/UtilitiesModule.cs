using BohFoundation.Utilities.Context.Implementation;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Context.Interfaces.Context;
using BohFoundation.Utilities.Email.Implementation;
using BohFoundation.Utilities.Email.Implementation.Helpers;
using BohFoundation.Utilities.Email.Interfaces.Email;
using BohFoundation.Utilities.Email.Interfaces.Email.Helpers;
using BohFoundation.Utilities.Utilities.Implementations;
using BohFoundation.Utilities.Utilities.Interfaces;
using Ninject.Modules;

namespace BohFoundation.Utilities.Infrastructure
{
    public class UtilitiesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IClaimsInformationGetters>().To<ClaimsInformationGetters>();
            Bind<IEmailService>().To<SendGridEmailService>();
            Bind<ISendGridClient>().To<SendGridClient>();
            Bind<IHttpContextInformationGetters>().To<HttpContextInformationGetters>();
            Bind<ISendEmailFromOffice365>().To<SendEmailFromOffice365>();
            Bind<IRandomObjectGenerator>().To<RandomObjectGenerator>();
            Bind<IGetTime>().To<GetTime>();
            Bind<IDeadlineUtilities>().To<DeadlineUtilities>();
        }
    }
}
